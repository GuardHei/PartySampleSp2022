using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InGameStats : MonoBehaviour {
    
    public SerializedDictionary<string, int> intAttributes;
    public SerializedDictionary<string, float> floatAttributes;
    public SerializedDictionary<string, string> stringAttributes;

    public string[] weaponsOnHold;
    public string armorOnHold;
    
    public List<string> weapons;
    public List<string> armors;
    public SerializedDictionary<string, int> items;
    public SerializedDictionary<string, int> ItemsMax;

    private void Awake() {
        /*
        intAttributes.Clear();
        floatAttributes.Clear();
        stringAttributes.Clear();
        weapons.Clear();
        armors.Clear();
        items.Clear();
        ItemsMax.Clear();
        */
        
        intAttributes = PlayerStats.IntAttributes;
        floatAttributes = PlayerStats.FloatAttributes;
        stringAttributes = PlayerStats.StringAttributes;
        weaponsOnHold = PlayerStats.WeaponsOnHold;
        armorOnHold = PlayerStats.ArmorOnHold;
        
        weapons = PlayerStats.weapons;
        armors = PlayerStats.armors;
        items = PlayerStats.Items;
        ItemsMax = PlayerStats.ItemsMax;
    }

    private void Update() {
        // print(PlayerStats.player.name);
        armorOnHold = PlayerStats.ArmorOnHold;
    }
}