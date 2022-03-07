using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCheck : MonoBehaviour {

    [Header("Settings")]
    public KeyCode key;
    public LayerMask mask;
    public Transform coneOrigin;
    public float coneDistance = 2.0f;
    public float coneAngle = 15.0f;

    [Header("Runtime Debug (Do not change)")]
    [SerializeField]
    private Interactable _bestPick;
    [SerializeField]
    private List<Interactable> _potentials = new List<Interactable>();
    [SerializeField]
    private List<Collider> _colliders = new List<Collider>();

    public bool HasInteractable => _bestPick;

    private void Update() {
        _bestPick = null;
        
        var origin = coneOrigin == null ? transform : coneOrigin;

        if (!Utilities.ConeCast(origin.position, origin.forward, coneAngle, out var hit, coneDistance)) return;
        int layer = 1 << hit.transform.gameObject.layer;
        if ((mask.value & layer) != 0) return;
        if (!hit.transform.TryGetComponent<Interactable>(out var interactable)) return;
        if (interactable.maxInteractionDistance <= hit.distance) _bestPick = interactable;
        
        if (Input.GetKeyUp(key)) Interact();
    }

    public void Interact() => _bestPick?.Interact();
}