using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    [Header("Settings")]
    public bool oneTime;
    public bool isInteractable;
    public bool hasReverseInteraction;
    public float maxInteractionDistance = 10.0f;
    [Multiline]
    public string interactionText;
    [Multiline]
    public string reverseInteractionText;
    public UnityEvent<Interactable> onInteraction;
    public UnityEvent<Interactable> onReverseInteraction;

    [Header("Runtime (Do not change)")]
    [SerializeField]
    private int _interactionCount;

    public bool IsInteractable => isInteractable && (!oneTime || _interactionCount == 0);

    public void Interact() {
        if (!IsInteractable) return;
        if (oneTime && _interactionCount > 0) return;
        _interactionCount++;
        if (!hasReverseInteraction) onInteraction?.Invoke(this);
        else {
            var interaction = _interactionCount % 2 == 1 ? onInteraction : onReverseInteraction;
            interaction?.Invoke(this);
        }
    }
}