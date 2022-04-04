using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour {

    public Health playerHealth;
    public PlayerMadness playerMadness;

    public Image healthBar;
    public Image madnessBar;

    private void Update() {
        if (playerHealth && healthBar) {
            float percentage = playerHealth.CurrentHealth / (float) playerHealth.maxHealth;
            healthBar.fillAmount = percentage;
        }

        if (playerMadness && madnessBar) {
            float percentage = playerMadness.CurrentMadness / (float)playerMadness.maxMadness;
            madnessBar.fillAmount = percentage;
        }
    }
}