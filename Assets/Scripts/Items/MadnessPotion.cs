using UnityEngine;

public class MadnessPotion : Item {
    
    public int recover = 10;

    public override void OnUse(int amount = 1) {
        base.OnUse(amount);
        if (PlayerStats.player && PlayerStats.player.TryGetComponent<PlayerMadness>(out var madness)) madness.gainMadness(recover * amount);
    }
}