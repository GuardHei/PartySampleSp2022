using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

    public AudioClip defeatTheme;
    public AudioClip introTheme;
    public AudioClip fieldTheme;

    public AudioSource src;

    private void Awake() {
        if (src == null) src = GetComponent<AudioSource>();
        src.clip = fieldTheme;
        src.Play();
    }

    public void PlayDefeatTheme() => src.PlayOneShot(defeatTheme);
}