using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudioController : MonoBehaviour {

    public AudioSource src;

    public List<AudioClip> hitSfx;
    public List<AudioClip> deathSfx;
    public List<AudioClip> attackSfx;

    private void Awake() {
        src ??= GetComponent<AudioSource>();
    }

    public void PlayHitSfx() {
        if (hitSfx.Count == 0) return;
        src?.Stop();
        src?.PlayOneShot(hitSfx[Random.Range(0, hitSfx.Count)]);
    }
    
    public void PlayDeathSfx() {
        if (deathSfx.Count == 0) return;
        src?.Stop();
        src?.PlayOneShot(deathSfx[Random.Range(0, deathSfx.Count)]);
    }
    
    public void PlayAttackSfx() {
        if (attackSfx.Count == 0) return;
        src?.Stop();
        src?.PlayOneShot(attackSfx[Random.Range(0, attackSfx.Count)]);
    }
}