using System;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Pickupable : MonoBehaviour {

    public string itemName;
    public PickupType pickupType;
    public string pickupText;
    public int amount = 1;

    public Interactable interactable;

    public void Awake() {
        if (!interactable) interactable = GetComponent<Interactable>();
    }

    public void Pickup(Interactable interactable) {
        if (string.IsNullOrEmpty(itemName)) return;
        Debug.Log("Pick up " + itemName + "!");
        switch (pickupType) {
            case PickupType.Item: PlayerStats.AddItem(itemName, amount);
                break;
            case PickupType.Weapon: PlayerStats.AddWeapon(itemName);
                break;
            case PickupType.Armor: PlayerStats.AddArmor(itemName);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

[Serializable]
public enum PickupType {
    Item,
    Weapon,
    Armor
}