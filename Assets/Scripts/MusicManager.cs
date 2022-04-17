using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

    public static MusicManager instance {
        get;
        private set;
    }

    public AudioClip defeatTheme;
    public AudioClip introTheme;
    public AudioClip fieldTheme;
    public AudioClip quietTheme;

    public AudioSource src;

    private void Awake() {
        instance = this;
        if (src == null) src = GetComponent<AudioSource>();
        src.clip = fieldTheme;
        src.Play();
    }

    public void PlayDefeatTheme() {
        src.volume = 1f;
        src.PlayOneShot(defeatTheme);
    }
}