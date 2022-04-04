using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;

public class WeaponController : MonoBehaviour
{
    public Weapon[] weaponPrefabs;
    public Weapon[] allWeapons;
    public Weapon currWeapon;

    void Awake()
    {
        int i = 0;
        foreach (string id in PlayerStats.WeaponsOnHold)
        {
            //Weapon weapon = Array.Find(allWeapons, w => w.weaponName == id);
            //weaponPrefabs[i] = (weapon);
            i++;
        }
        //currWeapon = weaponPrefabs[0];
    }

    void Update()
    {
        AttackUpdates();
        ChangeWeaponUpdate();
    }

    private void ChangeWeaponUpdate()
    {
        if (Input.GetKeyDown("q"))
        {
            //Switch weapon
        }
    }
    private void AttackUpdates()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack(false, "LMB");
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Attack(false, "RMB");
        }
    }

    public void Attack(bool charge, string type)
    {
        currWeapon.WeaponAttack(charge, type);
    }

    public void ChangeWeapon(string weapName)
    { //unfinished - should re think logic - dont really need right now because we only have 1 weapon
        foreach (var weap in weaponPrefabs)
        {
            if (weap.weaponName == weapName)
            {
                currWeapon = weap;
            }
        }
    }
}
