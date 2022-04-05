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

        HitBox LMBHb = LMBBox.GetComponent<HitBox>();
        HitBox LMBChargeHb = LMBChargeBox.GetComponent<HitBox>();
        HitBox RMBHb = RMBBox.GetComponent<HitBox>();
        HitBox RMBChargeHb = RMBChargeBox.GetComponent<HitBox>();

        LMBHb.attack = attackDamage; //Change Later to be different
        LMBChargeHb.attack = attackDamage;
        RMBHb.attack = attackDamage;
        RMBChargeHb.attack = attackDamage;
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
                    if (CheckCrazy())
                    {
                        EnableHitbox(RMBChargeBox);
                    }
                    break;
            }
        }
    }


    private bool CheckCrazy()
    {
        return PlayerStats.GetIntAttribute("curr craziness") > crazyThreshold;
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
}
