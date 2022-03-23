using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //hitbox object, also add checks for crazy meter and attack rate
    [Header("MetaData")]
    public float attack;
    public float fireRate;
    public int crazyThreshold;
    public float pierce; //possible idea, reduction to armor
    public string weaponName;
    private double nextFire = 0.0f;
    [Header("HitBoxes")]
    public GameObject LMBBox; //Make sure to attach hitbox script
    public GameObject LMBChargeBox;
    public GameObject RMBBox;
    public GameObject RMBChargeBox;
    [Header("ParentRigidbody")]
    public Rigidbody parentRB;

    public void Attack(string fireType)
    {
        if (CheckConstraints())
        {
            switch (fireType)
            {
                case "Fire1":
                    CreateHitbox(LMBBox);
                    break;
                case "Fire2":
                    CreateHitbox(RMBBox);
                    break;
                case "Fire3":
                    if (CheckCrazy()) 
                    {
                        CreateHitbox(LMBChargeBox);
                    }
                    break;
                case "Fire4":
                    if (CheckCrazy())
                    {
                        CreateHitbox(RMBChargeBox);
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

    private void CreateHitbox(GameObject hitbox) 
    {
        GameObject obj = Instantiate(hitbox, parentRB.GetRelativePointVelocity(new Vector3(0, 0, 5)), Quaternion.identity); //arbitrary amount in front of player
    }
}
