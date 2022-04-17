using System;
using UnityEngine;

public class HealthPotion : Item {

    public int cure = 10;
    public Health health;

    public override void OnUse(int amount = 1) {
        base.OnUse(amount);
        if (health) health.Recover(cure * amount);
    }
}