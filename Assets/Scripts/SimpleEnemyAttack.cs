using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyAttack : MonoBehaviour {

    public GameObject hitBox;
    public Vector3 offset = new Vector3(.0f, .0f, .6f);
    public float attackDuration = 1.0f;
    public float attackSpeed = 1.0f;

    private void Awake() {
        // hitBox.SetActive(false);
    }

    public void Attack() {
        var atk = Instantiate(hitBox, transform.TransformPoint(offset), transform.rotation);
        var autoMove = atk.GetComponent<AutoMove>();
        autoMove.velocity = attackSpeed * (PlayerStats.player.transform.position - transform.position).normalized;
        autoMove.duration = attackDuration;
        Physics.IgnoreCollision(autoMove.GetComponent<Collider>(), GetComponent<Collider>(), true);
    }
}