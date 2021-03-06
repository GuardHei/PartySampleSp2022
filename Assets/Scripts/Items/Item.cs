using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

    public Image image;
    public string id;
    public KeyCode key;

    void Update() {
        if (Input.GetKeyDown(key)) Use();
    }

    public virtual void Use(int amount = 1) {
        if (string.IsNullOrEmpty(id)) return;
        if (PlayerStats.UseItem(id, amount)) OnUse(amount);
    }

    public virtual void OnUse(int amount = 1) {
        Debug.Log("Use " + amount + " " + id);
    }
}