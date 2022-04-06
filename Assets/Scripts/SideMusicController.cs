using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SideMusicController : MonoBehaviour {

    public AudioClip clip;
    public AudioSource src;
    public float minDistance = .5f;
    public float maxDistance = 10f;
    public float affectDistance = 16.5f;
    public AnimationCurve falloff;

    private void Awake() {
        if (!src) src = GetComponent<AudioSource>();
        src.clip = clip;
        src.Play();
        src.volume = .0f;
    }

    private void Update() {
        if (!MusicManager.instance) return;
        var dist = (MusicManager.instance.transform.position - transform.position).magnitude;
        if (dist > affectDistance) return;
        if (dist > maxDistance) {
            src.volume = 0f;
            MusicManager.instance.src.volume = 1f;
            // print(1);
        } else if (dist < minDistance) {
            src.volume = 1.0f;
            MusicManager.instance.src.volume = 0f;
            // print(2);
        } else {
            var t = falloff.Evaluate(Mathf.Clamp01((dist - minDistance) / (maxDistance - minDistance)));
            src.volume = t;
            MusicManager.instance.src.volume = Mathf.Clamp01(1.0f - t * 2f);
            // print(3);
        }
    }
}