using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //hitbox object, also add checks for crazy meter and attack rate
    [Header("MetaData")]
    public int attackDamage;
    public Vector3 attackEndPosition = Vector3.forward;
    public Vector3 attackEndRotation = new Vector3(.0f, 90f, .0f);
    public AnimationCurve attackCurve;
    public float attackDuration = .2f;
    public float fireRate;
    public int crazyThreshold;
    public float pierce; //possible idea, reduction to armor
    public string weaponName;
    private double nextFire = 0.0f;
    private CoroutineTask animationTask;
    [Header("HitBoxes")]
    public GameObject LMBBoxPrefab; //prefabs
    public GameObject LMBChargeBoxPrefab;
    public GameObject RMBBoxPrefab;
    public GameObject RMBChargeBoxPrefab;

    private GameObject LMBBox;
    private GameObject LMBChargeBox;
    private GameObject RMBBox;
    private GameObject RMBChargeBox;
    
    //For ranged attacks
    public GameObject bulletHitBox;
    public Vector3 offset = new Vector3(.0f, .0f, .6f);
    public float attackSpeed = 1.0f;
    public Camera cam;

    public PlayerMadness playerMadness;

    // Start is called before the first frame update
    void Start()
    {
        animationTask = new CoroutineTask(this);
        InstantiateBoxes();
        InitializeHitboxes();
    }

    private void InstantiateBoxes()
    {
        LMBBox = Instantiate(LMBBoxPrefab, this.transform, false);
        LMBChargeBox = Instantiate(LMBChargeBoxPrefab, this.transform, false);
        RMBBox = Instantiate(RMBBoxPrefab, this.transform, false);
        RMBChargeBox = Instantiate(RMBChargeBoxPrefab, this.transform, false);
    }
    private void InitializeHitboxes()
    {
        LMBBox.SetActive(false);
        LMBChargeBox.SetActive(false);
        RMBBox.SetActive(false);
        RMBChargeBox.SetActive(false);
        bulletHitBox.SetActive(true);

        HitBox LMBHb = LMBBox.GetComponent<HitBox>();
        HitBox LMBChargeHb = LMBChargeBox.GetComponent<HitBox>();
        HitBox RMBHb = RMBBox.GetComponent<HitBox>();
        HitBox RMBChargeHb = RMBChargeBox.GetComponent<HitBox>();
        HitBox bulletHb = bulletHitBox.GetComponent<HitBox>();

        LMBHb.attack = attackDamage; //Change Later to be different
        LMBChargeHb.attack = attackDamage;
        RMBHb.attack = attackDamage;
        RMBChargeHb.attack = attackDamage;
        bulletHb.attack = attackDamage;

    }

    public void WeaponAttack(bool charge, string type)
    {
        if (CheckConstraints())
        {
            switch (type)
            {
                case "LMB":
                    EnableHitbox(LMBBox);
                    break;
                case "RMB":
                    EnableHitbox(RMBBox);
                    break;
                case "LMBCharge":
                    if (CheckCrazy())
                    {
                        EnableHitbox(LMBChargeBox);
                    }
                    break;
                case "RMBCharge":
                    //Consume player madness. Using crazyThreshold as madness consumer per attack
                    if (playerMadness.CurrentMadness > crazyThreshold)
                    {
                        playerMadness.consumeMadness(50, false);
                        //RMBChargeBox = Instantiate(RMBChargeBoxPrefab, this.transform, false);
                        //EnableHitbox(RMBChargeBox);
                        //Ranged attack
                        rangedAttack();
                        //animationTask.StartCoroutine(ExeAttackAnim(RMBChargeBox));
                    }
                    break;
            }
        }
    }


    private bool CheckCrazy()
    {
        //return PlayerStats.GetIntAttribute("curr craziness") > crazyThreshold;
        return playerMadness.CurrentMadness >= crazyThreshold;
    }
        
    private bool CheckConstraints()
    {
        if (Time.timeAsDouble > nextFire) 
        {
            nextFire = Time.timeAsDouble + fireRate;
            return true;
        }
        return false;
    }

    private void EnableHitbox(GameObject hb) 
    {
        hb.SetActive(true);
        animationTask.StartCoroutine(ExeAttackAnim(hb));
    }

    private IEnumerator ExeAttackAnim(GameObject hb)
    {
        float attackAnimDuration = attackDuration;
        float startTime = Time.time;
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;
        Vector3 targetPosition = attackEndPosition;
        Quaternion targetRotation = Quaternion.Euler(attackEndRotation);
        while (true)
        {
            float currTime = Time.time;
            float animProgress = (currTime - startTime) / attackAnimDuration;
            animProgress = attackCurve.Evaluate(animProgress);
            animProgress = Mathf.Clamp01(animProgress);
            transform.localPosition = (Vector3.Lerp(startPosition, targetPosition, animProgress));
            transform.localRotation = (Quaternion.Lerp(startRotation, targetRotation, animProgress));
            // print(animProgress.ToString("F3"));
            if (animProgress == 1f)
            {
                hb.SetActive(false);
                transform.localPosition = startPosition;
                transform.localRotation = startRotation;
                yield break;// exit
            }
            yield return CoroutineTask.WaitForNextFrame;
        }
    }

    private void rangedAttack()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 target;
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;
        }
        else
        {
            target = ray.GetPoint(300);
        }
        Debug.Log("Ranged attack: " + target + " | " + hit);
        
        var atk = Instantiate(bulletHitBox, transform.TransformPoint(offset), transform.rotation);
        var autoMove = atk.GetComponent<AutoMove>();
        autoMove.velocity = attackSpeed * (target -  PlayerStats.player.transform.position).normalized;
        autoMove.duration = attackDuration;
        //Physics.IgnoreCollision(autoMove.GetComponent<Collider>(), GetComponent<Collider>(), true);
        //onAttack?.Invoke();
    }
}
