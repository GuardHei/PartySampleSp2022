using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    private int currAmmo;

    public int maxAmmo;
    public float reloadSpeed;
    private double reloadTimer = 0.0f;

    private void Start()
    {
        currAmmo = maxAmmo;
    }

    public void Attack(bool charge)
    {
        if (currAmmo > 0 && Time.timeAsDouble > reloadTimer) {
            base.WeaponAttack(charge, "RMBCharge");
            currAmmo -= 1;
        }
    }

    public void Reload() 
    {
        currAmmo = maxAmmo;
        reloadTimer = Time.timeAsDouble + reloadSpeed;
    }

}
