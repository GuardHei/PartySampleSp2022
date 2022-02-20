using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionCheck : MonoBehaviour {

    [Header("Settings")]
    public LayerMask mask;

    [Header("Runtime (Do not change)")]
    [SerializeField]
    private Interactable _bestPick;
    [SerializeField]
    private List<Interactable> _potentials = new List<Interactable>();
    [SerializeField]
    private List<Collider> _colliders = new List<Collider>();

    private List<int> _deleted = new List<int>();

    public bool HasInteractable => _bestPick;

    private void Update() {

        var minDist = Mathf.Infinity;
        var changed = false;
        
        for (var i = 0; i < _potentials.Count; i++) {
            var potential = _potentials[i];
            var collider = _colliders[i];
            if (!potential || !collider) {
                _deleted.Add(i);
                continue;
            }

            var pos = transform.position;

            var dist = (collider.ClosestPointOnBounds(pos) - pos).sqrMagnitude;
            if (dist < minDist) {
                minDist = dist;
                _bestPick = potential;
                changed = true;
            }
        }

        if (!changed) _bestPick = null;
        
        var clearFlag = false;
        for (var i = 0; i < _deleted.Count; i++) {
            _potentials.RemoveAt(i);
            _colliders.RemoveAt(i);
            clearFlag = true;
        }
        
        if (clearFlag) _deleted.Clear();
    }

    public void Interact() => _bestPick?.Interact();

    private void OnTriggerStay(Collider other) {
        int layer = 1 << other.gameObject.layer;
        if ((mask.value & layer) == 0) return;

        if (other.TryGetComponent<Interactable>(out var interactable)) {
            _potentials.Add(interactable);
            _colliders.Add(other);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        int layer = 1 << other.gameObject.layer;
        if ((mask.value & layer) == 0) return;

        if (other.TryGetComponent<Interactable>(out var interactable)) {
            _potentials.Remove(interactable);
            _colliders.Remove(other);
        }
    }
}