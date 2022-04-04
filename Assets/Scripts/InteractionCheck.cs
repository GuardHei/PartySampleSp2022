using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class InteractionCheck : MonoBehaviour {

    [Header("Settings")]
    public KeyCode key;
    public LayerMask mask;
    public Transform coneOrigin;
    public float overlapDistance = .5f;
    public float coneDistance = 2.0f;
    public float coneAngle = 15.0f;
    public GameObject interactionHUDRoot;
    public TextMeshProUGUI interactionTextMesh;

    [Header("Runtime Debug (Do not change)")]
    [SerializeField]
    private Interactable _bestPick;

    private readonly StringBuilder _interactionTextBase = new();

    public bool HasInteractable => _bestPick && _bestPick.isActiveAndEnabled;

    private void Update() {
        var lastPick = _bestPick;
        _bestPick = null;
        
        var origin = coneOrigin == null ? transform : coneOrigin;

        Debug.DrawRay(origin.position, origin.forward, Color.magenta);
        // if (!Utilities.ConeCast(origin.position, origin.forward, coneAngle, out var hit, coneDistance)) return;
        if (Physics.Raycast(origin.position, origin.forward, out var hit, coneDistance)) {
            var layer = 1 << hit.transform.gameObject.layer;
            if ((mask.value & layer) != 0) {
                if (hit.transform.TryGetComponent<Interactable>(out var interactable)) {
                    if (interactable.IsInteractable && interactable.maxInteractionDistance >= hit.distance) _bestPick = interactable;
                }
            }
        }

        if (!HasInteractable) {
            if (interactionHUDRoot.activeSelf) interactionHUDRoot.SetActive(false);
        } else {
            if (!interactionHUDRoot.activeSelf) interactionHUDRoot.SetActive(true);
            if (lastPick != _bestPick) {
                _interactionTextBase.Clear();
                _interactionTextBase.Append("<b>[");
                _interactionTextBase.Append(key.ToString());
                _interactionTextBase.Append("]</b> ");
                _interactionTextBase.Append(_bestPick.InteractionText);
                interactionTextMesh.text = _interactionTextBase.ToString();
            }
            if (Input.GetKeyUp(key)) Interact();
        }
    }

    public void Interact() {
        if (HasInteractable) _bestPick.Interact();
    }
}