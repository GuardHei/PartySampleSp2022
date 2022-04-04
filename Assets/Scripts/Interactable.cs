using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    [Header("Settings")]
    public bool oneTime;
    public bool isInteractable = true;
    public bool hasReverseInteraction;
    public float maxInteractionDistance = 10.0f;
    public bool autoDisable = true;
    [Multiline]
    public string interactionText;
    [Multiline]
    public string reverseInteractionText;
    public UnityEvent<Interactable> onInteraction = new UnityEvent<Interactable>();
    public UnityEvent<Interactable> onReverseInteraction = new UnityEvent<Interactable>();

    [Header("Runtime (Do not change)")]
    [SerializeField]
    private int _interactionCount;

    public bool IsInteractable => isInteractable && (!oneTime || _interactionCount == 0);
    public string InteractionText => !hasReverseInteraction ? interactionText : (_interactionCount % 2 == 1 ? interactionText : reverseInteractionText); 

    public void Interact() {
        if (!IsInteractable) return;
        if (oneTime && _interactionCount > 0) return;
        _interactionCount++;
        var interaction = onInteraction;
        if (hasReverseInteraction) interaction = _interactionCount % 2 == 1 ? onInteraction : onReverseInteraction;
        interaction?.Invoke(this);

        if (oneTime && autoDisable) gameObject.SetActive(false);
    }
}