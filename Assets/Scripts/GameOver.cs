using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public KeyCode debugKey = KeyCode.L;
    public KeyCode restartKey = KeyCode.R;
    public KeyCode quitKey = KeyCode.Escape;

    public AudioSource srcDontMuteOnDeath;
    public GameObject defeatUI;
    public GameObject debugUI;
    public TextMeshProUGUI debugText;

    private void Awake() {
        // print(debugUI.name);
    }

    private void Update() {
        if (Input.GetKeyUp(restartKey)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (Input.GetKeyUp(quitKey)) Application.Quit();
        if (Input.GetKeyUp(debugKey)) debugUI.SetActive(!debugUI.activeSelf);
    }
    
    public void Gameover() {
        if (debugText) debugText.text = "0\n";
        
        foreach (var audio in FindObjectsOfType<AudioSource>()) audio.mute = true;
        foreach (var item in PlayerStats.player.GetComponents<Item>()) item.enabled = false;
        foreach (var sideMusic in FindObjectsOfType<SideMusicController>()) sideMusic.enabled = false;

        if (srcDontMuteOnDeath) srcDontMuteOnDeath.mute = false;

        if (debugText) debugText.text += "1\n";
        
        if (defeatUI) defeatUI.SetActive(true);
        
        if (debugText) debugText.text += "2\n";
        if (debugText) debugText.text += "Player null: " + (PlayerStats.player == null) + "\n";

        var pmc = FindObjectOfType<PlayerMovementController>();
        if (pmc) {
            pmc.enabled = false;
            if (debugText) debugText.text += "a\n";
        }
        
        var pfc = FindObjectOfType<PlayerFocusController>();
        if (pfc) {
            pfc.enabled = false;
            if (debugText) debugText.text += "b\n";
        }

        var ic = FindObjectOfType<InteractionCheck>();
        if (ic) {
            ic.enabled = false;
            if (debugText) debugText.text += "c\n";
        }

        var wc = FindObjectOfType<WeaponController>();
        if (wc) {
            wc.enabled = false;
            if (debugText) debugText.text += "d\n";
        }

        /*
        PlayerStats.player.GetComponent<PlayerMovementController>().enabled = false;
        PlayerStats.player.GetComponent<PlayerFocusController>().enabled = false;
        PlayerStats.player.GetComponent<InteractionCheck>().enabled = false;
        PlayerStats.player.GetComponent<WeaponController>().enabled = false;
        */

        if (debugText) debugText.text += "3\n";
        if (debugText) debugText.text += "MM null: " + (MusicManager.instance == null) + "\n";
        if (debugText) debugText.text += "MM src null: " + (MusicManager.instance.src == null) + "\n";

        MusicManager.instance.src.mute = false;
        MusicManager.instance.src.clip = null;
        
        if (debugText) debugText.text += "4\n";
        
        MusicManager.instance.PlayDefeatTheme();
        
        if (debugText) debugText.text += "5\n";
    }
}