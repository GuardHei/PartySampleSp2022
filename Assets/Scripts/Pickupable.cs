using System;
using UnityEngine;

public class Pickupable : MonoBehaviour {

    public string itemName;
    public string pickupText;
    public int amount = 1;
    public float pickupDistance = 10.0f;

    public Interactable interactable;

    public void Awake() {
        if (!interactable) interactable = GetComponent<Interactable>();
        if (!interactable) interactable = gameObject.AddComponent<Interactable>();

        interactable.maxInteractionDistance = pickupDistance;
        interactable.oneTime = true;
        interactable.onInteraction.AddListener(Pickup);
        interactable.interactionText = string.IsNullOrEmpty(pickupText) ? "Pick up " + itemName : pickupText;
    }

    public void Pickup(Interactable interactable) {
        if (!string.IsNullOrEmpty(itemName)) PlayerStats.AddItem(itemName, amount);
    }
}