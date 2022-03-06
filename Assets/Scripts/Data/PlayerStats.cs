using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats {

    public static readonly Dictionary<string, int> IntAttributes = new Dictionary<string, int>(10);
    public static readonly Dictionary<string, float> FloatAttributes = new Dictionary<string, float>(10);
    public static readonly Dictionary<string, string> StringAttributes = new Dictionary<string, string>(10);

    public static readonly string[] WeaponsOnHold = { "", "", "" };
    public static string ArmorOnHold = "";

    public static readonly Dictionary<string, int> Items = new Dictionary<string, int>(10);
    public static readonly Dictionary<string, int> ItemsMax = new Dictionary<string, int>(10);
    public static readonly List<string> weapons = new List<string>(10);
    public static readonly List<string> armors = new List<string>(10);

    static PlayerStats() {
        IntAttributes["max health"] = 100;
        IntAttributes["curr health"] = 100;
        IntAttributes["max craziness"] = 100;
        IntAttributes["curr craziness"] = 100;

        FloatAttributes["speed"] = 5.0f;

        StringAttributes["name"] = "Why don't you name your character?";
        
        ItemsMax["hand sanitizer"] = 99;
        ItemsMax["boba"] = 99;
        ItemsMax["weed"] = 99;
    }

    public static int GetIntAttribute(string attr, int defaultValue = 0) => !IntAttributes.ContainsKey(attr) ? defaultValue : IntAttributes[attr];
    
    public static float GetFloatAttribute(string attr, float defaultValue = .0f) => !FloatAttributes.ContainsKey(attr) ? defaultValue : FloatAttributes[attr];
    
    public static string GetStringAttribute(string attr, string defaultValue = "") => !StringAttributes.ContainsKey(attr) ? defaultValue : StringAttributes[attr];

    public static bool CheckItemExistence(string itemName) => Items.ContainsKey(itemName) && Items[itemName] > 0;
    
    public static int GetItemNum(string itemName) => Items.ContainsKey(itemName) ? Items[itemName] : 0;
    
    public static int GetItemMaxNum(string itemName) => ItemsMax.ContainsKey(itemName) ? ItemsMax[itemName] : 0;

    public static bool UseItem(string itemName, int amount = 1) {
        if (!Items.ContainsKey(itemName)) return false;
        if (Items[itemName] < amount) return false;
        Items[itemName] = 0;
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
}