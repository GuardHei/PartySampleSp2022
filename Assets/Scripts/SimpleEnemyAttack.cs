using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEnemyAttack : MonoBehaviour {

    public GameObject hitBox;
    public Vector3 offset = new Vector3(.0f, .0f, .6f);
    public float attackDuration = 1f;
    public float attackSpeed = 1f;
    public bool flag = true;
    public UnityEvent onAttack;

    private void Awake() {
        // hitBox.SetActive(false);
    }

    public void Attack() {
        if (!flag) return;
        var atk = Instantiate(hitBox, transform.TransformPoint(offset), transform.rotation);
        var autoMove = atk.GetComponent<AutoMove>();
        autoMove.velocity = attackSpeed * (PlayerStats.player.transform.position - transform.position).normalized;
        autoMove.duration = attackDuration;
        Physics.IgnoreCollision(autoMove.GetComponent<Collider>(), GetComponent<Collider>(), true);
        onAttack?.Invoke();
    }
}