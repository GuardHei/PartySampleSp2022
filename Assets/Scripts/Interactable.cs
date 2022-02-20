using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    [Header("Settings")]
    public bool oneTime;
    public bool isInteractable;
    public bool hasReverseInteraction;
    public UnityEvent<Interactable> onInteraction;
    public UnityEvent<Interactable> onReverseInteraction;

    [Header("Runtime (Do not change)")]
    [SerializeField]
    private int _interactionCount;

    public bool IsInteractable => isInteractable && (!oneTime || _interactionCount == 0);

    public void Interact() {
        if (!IsInteractable) return;
        _interactionCount++;
        if (!hasReverseInteraction) onInteraction?.Invoke(this);
        else {
            var interaction = _interactionCount % 2 == 1 ? onInteraction : onReverseInteraction;
            interaction?.Invoke(this);
        }
    }
}