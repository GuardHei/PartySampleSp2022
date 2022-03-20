using System;
using UnityEngine;

public class HealthPotion : Item {

    public int cure = 10;

    public override void OnUse(int amount = 1) {
        base.OnUse(amount);
        if (PlayerStats.player && PlayerStats.player.TryGetComponent<Health>(out var health)) health.Recover(cure * amount);
    }
}