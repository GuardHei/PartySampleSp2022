using UnityEngine;

public class MadnessPotion : Item {
    
    public int recover = 10;
    public PlayerMadness madness;

    public override void OnUse(int amount = 1) {
        base.OnUse(amount);
        if (madness) madness.gainMadness(recover * amount);
    }
}