using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //hitbox object, also add checks for crazy meter and attack rate
    [Header("MetaData")]
    public int attackDamage;
    public float attackRange;
    public float attackRadius;
    public float fireRate;
    public int crazyThreshold;
    public float pierce; //possible idea, reduction to armor
    public string weaponName;
    private double nextFire = 0.0f;
    [Header("HitBoxes")]
    public GameObject LMBBox; //prefabs
    public GameObject LMBChargeBox;
    public GameObject RMBBox;
    public GameObject RMBChargeBox;

    private CapsuleCollider LMBCollider;
    private CapsuleCollider LMBChargeCollider;
    private CapsuleCollider RMBCollider;
    private CapsuleCollider RMBChargeCollider;

    // Start is called before the first frame update
    void Start()
    {
        InitializeHitboxes();
    }


    private void InitializeHitboxes()
    {
        LMBCollider = LMBBox.GetComponent<CapsuleCollider>();
        LMBChargeCollider = LMBChargeBox.GetComponent<CapsuleCollider>();
        RMBCollider = RMBBox.GetComponent<CapsuleCollider>();
        RMBChargeCollider = RMBChargeBox.GetComponent<CapsuleCollider>();

        LMBCollider.enabled = false;
        LMBChargeCollider.enabled = false;
        RMBCollider.enabled = false;
        RMBChargeCollider.enabled = false;

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
                    EnableHitbox(LMBCollider);
                    break;
                case "RMB":
                    EnableHitbox(RMBCollider);
                    break;
                case "LMBCharge":
                    if (CheckCrazy())
                    {
                        EnableHitbox(LMBChargeCollider);
                    }
                    break;
                case "RMBCharge":
                    if (CheckCrazy())
                    {
                        EnableHitbox(RMBChargeCollider);
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

    private void EnableHitbox(Collider hb) 
    {
        hb.enabled = true;
        ExeAttackAnim(hb);

    }

    private IEnumerator ExeAttackAnim(Collider hb)
    {
        float attackAnimDuration = 1.0f;
        float startTime = Time.time;
        Vector3 targetPosition = Vector3.back;
        Vector3 targetRotation = Vector3.back;
        while (true)
        {
            float currTime = Time.time;
            float animProgress = (currTime - startTime) / attackAnimDuration;
            transform.position = (Vector3.Lerp(transform.position, targetPosition, animProgress));
            transform.rotation = (Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), animProgress));
            if (animProgress > .99f)
            {
                hb.enabled = false;
                yield break;// exit
            }
            yield return CoroutineTask.WaitForNextFrame;
        }
    }
}
