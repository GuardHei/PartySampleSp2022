using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class PlayerStats {

    public static GameObject player;

    public static readonly SerializedDictionary<string, int> IntAttributes = new SerializedDictionary<string, int>();
    public static readonly SerializedDictionary<string, float> FloatAttributes = new SerializedDictionary<string, float>();
    public static readonly SerializedDictionary<string, string> StringAttributes = new SerializedDictionary<string, string>();

    public static readonly string[] WeaponsOnHold = { "", "", "" };
    public static string ArmorOnHold = "";

    public static readonly SerializedDictionary<string, int> Items = new SerializedDictionary<string, int>();
    public static readonly SerializedDictionary<string, int> ItemsMax = new SerializedDictionary<string, int>();
    public static readonly List<string> weapons = new List<string>(10);
    public static readonly List<string> armors = new List<string>(10);

    static PlayerStats() {
        Init();
        SceneManager.activeSceneChanged += (s0, s1) => UpdatePlayerObject();
    }

    public static void Init() {
        UpdatePlayerObject();
        
        IntAttributes["max health"] = 100;
        IntAttributes["curr health"] = 100;
        IntAttributes["max madness"] = 100;
        IntAttributes["curr madness"] = 100;

        FloatAttributes["speed"] = 5.0f;

        StringAttributes["name"] = "Why don't you name your character?";

        Items["hand sanitizer"] = 1;
        Items["boba"] = 1;
        Items["weed"] = 1;
        
        ItemsMax["hand sanitizer"] = 99;
        ItemsMax["boba"] = 99;
        ItemsMax["weed"] = 99;
    }

    public static int GetIntAttribute(string attr, int defaultValue = 0) => !IntAttributes.ContainsKey(attr) ? defaultValue : IntAttributes[attr];
    
    public static float GetFloatAttribute(string attr, float defaultValue = .0f) => !FloatAttributes.ContainsKey(attr) ? defaultValue : FloatAttributes[attr];
    
    public static string GetStringAttribute(string attr, string defaultValue = "") => !StringAttributes.ContainsKey(attr) ? defaultValue : StringAttributes[attr];

    public static bool CheckItemExistence(string itemName) => Items.ContainsKey(itemName) && Items[itemName] > 0;
    
    public static int GetItemNum(string itemName) => Items.ContainsKey(itemName) ? Items[itemName] : 0;
    
    public static int GetItemMaxNum(string itemName) => ItemsMax.ContainsKey(itemName) ? ItemsMax[itemName] : -1;

    public static bool UseItem(string itemName, int amount = 1) {
        if (!Items.ContainsKey(itemName)) return false;
        if (Items[itemName] < amount) return false;
        Items[itemName] -= amount;
        return true;
    }
    
    public static void AddItem(string itemName, int amount = 1) {
        if (!Items.ContainsKey(itemName)) Items.Add(itemName, 0);
        var curr = Items[itemName];
        curr += amount;
        if (ItemsMax.ContainsKey(itemName)) curr = Mathf.Min(curr, ItemsMax[itemName]);
        Items[itemName] = curr;
    }

    public static void AddWeapon(string itemName) {
        if (weapons.Contains(itemName)) return;
        weapons.Add(itemName);
    }
    
    public static void AddArmor(string itemName) {
        if (armors.Contains(itemName)) return;
        armors.Add(itemName);
    }

    public static void UpdatePlayerObject() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}