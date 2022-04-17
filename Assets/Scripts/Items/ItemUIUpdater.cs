using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUIUpdater : MonoBehaviour {

    public TextMeshProUGUI handSanitizerUI;
    public TextMeshProUGUI bobaUI;
    public TextMeshProUGUI nullUI;
    public TextMeshProUGUI weedUI;

    public int handSanitizerPrevCount;
    public int bobaPrevCount;
    public int weedPrevCount;

    public const string STRING_INF = "\u25A1";

    public void Update() {
        int count;
        int max;
        if (handSanitizerUI) {
            count = PlayerStats.GetItemNum("hand sanitizer");
            if (handSanitizerPrevCount != count) {
                handSanitizerPrevCount = count;
                max = PlayerStats.GetItemMaxNum("hand sanitizer");
                handSanitizerUI.text = count + " / " + (max == -1 ? STRING_INF : max);
            }
        }
        
        if (bobaUI) {
            count = PlayerStats.GetItemNum("boba");
            if (bobaPrevCount != count) {
                bobaPrevCount = count;
                max = PlayerStats.GetItemMaxNum("boba");
                bobaUI.text = count + " / " + (max == -1 ? STRING_INF : max);
            }
        }
        
        if (weedUI) {
            count = PlayerStats.GetItemNum("weed");
            if (weedPrevCount != count) {
                weedPrevCount = count;
                max = PlayerStats.GetItemMaxNum("weed");
                weedUI.text = count + " / " + (max == -1 ? STRING_INF : max);
            }
        }
    }
}