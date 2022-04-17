using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

    [Header("Settings")]
    public int maxHealth;
    public bool destroyOnDeath;
    public UnityEvent<DamageType, int, int> onDamage;
    public UnityEvent<int, int> onRecover;
    public UnityEvent<DamageType> onDeath;

    [Header("Runtime (DO NOT CHANGE)")]
    [SerializeField]
    private bool _dead;
    [SerializeField]
    private int _health;

    public bool Dead => _dead;
    public int CurrentHealth => _health;

    private void Awake() {
        _health = maxHealth;
        _dead = false;
    }

    public void TakeDamage(DamageType type, int dmg) {
        if (_dead) return;
        _health = Mathf.Max(_health - dmg, 0);
        onDamage?.Invoke(type, dmg, _health);
        if (_health == 0) Die(type);
    }

    public void Recover(int cure) {
        if (_dead) return;
        _health = Mathf.Min(_health + cure, maxHealth);
        onRecover?.Invoke(cure, _health);
    }

    public void Die(DamageType type) {
        _dead = true;
        onDeath?.Invoke(type);
        if (destroyOnDeath) SelfDestroy();
    }

    public void SelfDestroy() {
        GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, 3f);
    }
}