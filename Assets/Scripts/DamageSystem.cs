using System;

public class DamageSystem {

    public static void CallDamageEvent(HitBox attacker, Damagable receiver) {
        if (!receiver.health) return;
        int incr = attacker.attack;
        int decr = receiver.defense;
        if (receiver.hasWeakness && attacker.damageType == receiver.weakness) decr = 0;
        int dmg = incr - decr;
        receiver.health.TakeDamage(attacker.damageType, dmg);
    }
}

[Serializable]
public enum DamageType {
    SLASH = 0,
    THRUST = 1,
    STRIKE = 2,
    MENTAL = 3
}