using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shield : MonoBehaviour {

    [Header("Settings")]
    public bool recover;
    public float recoverInterval;
    public int recoverAmount;
    public Health shieldHealth;
    public LayerMask blockedByLayers;
    public string blockedByTag = "Player";

    [Header("Runtime (Do not change)")]
    [SerializeField]
    private List<HitBox> _blocked = new List<HitBox>();

    private CoroutineTask _recoverTask;
    private List<int> _deleted = new List<int>();

    private void Awake() {
        _recoverTask = new CoroutineTask(this);
        if (!shieldHealth) shieldHealth = GetComponent<Health>();
    }

    private void OnEnable() {
        if (recover) _recoverTask.StartCoroutine(StartRecoveryTask());
    }

    private void OnDisable() {
        _recoverTask.StopCoroutine();
    }

    private void Update() {
        for (var i = 0; i < _blocked.Count; i++) {
            if (!_blocked[i]) _deleted.Add(i);
        }

        var clearFlag = false;
        for (var i = 0; i < _deleted.Count; i++) {
            _blocked.RemoveAt(i);
            clearFlag = true;
        }
        
        if (clearFlag) _deleted.Clear();
    }

    private IEnumerator StartRecoveryTask() {
        var wait = new WaitForSeconds(recoverInterval);
        while (true) {
            yield return wait;
            shieldHealth?.Recover(recoverAmount);
        }
    }

    public bool CheckBlocked(HitBox attacker) => _blocked.Contains(attacker);

    private void OnTriggerEnter(Collider other) {
        bool flag = false;
        int layer = 1 << other.gameObject.layer;
        if ((blockedByLayers.value & layer) != 0) flag = true;
        if (!flag && !string.IsNullOrEmpty(blockedByTag) && other.CompareTag(blockedByTag)) flag = true;
        if (!flag) return;
        
        if (other.TryGetComponent<HitBox>(out var attacker)) _blocked.Add(attacker);
    }
    
    private void OnTriggerExit(Collider other) {
        bool flag = false;
        int layer = 1 << other.gameObject.layer;
        if ((blockedByLayers.value & layer) != 0) flag = true;
        if (!flag && !string.IsNullOrEmpty(blockedByTag) && other.CompareTag(blockedByTag)) flag = true;
        if (!flag) return;
        
        if (other.TryGetComponent<HitBox>(out var attacker)) _blocked.Remove(attacker);
    }
}