using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damagable : MonoBehaviour {

    [Header("Settings")]
    public bool hasWeakness;
    public DamageType weakness;
    public LayerMask damagedByLayers;
    public string damagedByTag = "Player";
    public int defense;
    public Health health;
    public List<Shield> shields = new List<Shield>();

    private void Awake() {
        if (!health) health = GetComponentInParent<Health>();
    }

    public void TakeDamage(HitBox attacker) {
        bool flag = false;
        int layer = 1 << attacker.gameObject.layer;
        if ((damagedByLayers.value & layer) != 0) flag = true;
        if (!flag && !string.IsNullOrEmpty(damagedByTag) && attacker.CompareTag(damagedByTag)) flag = true;
        if (!flag) return;

        if (shields.Any(shield => shield.CheckBlocked(attacker))) return;
        DamageSystem.CallDamageEvent(attacker, this);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Enters Damagable");
        bool flag = false;
        int layer = 1 << other.gameObject.layer;
        if ((damagedByLayers.value & layer) != 0) flag = true;
        if (!flag && !string.IsNullOrEmpty(damagedByTag) && other.CompareTag(damagedByTag)) flag = true;
        if (!flag) return;

        if (other.TryGetComponent<HitBox>(out var attacker)) {
            if (shields.Any(shield => shield.CheckBlocked(attacker))) return;
            DamageSystem.CallDamageEvent(attacker, this);
        }
    }
}