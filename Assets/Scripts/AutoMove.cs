using System;
using UnityEngine;

[RequireComponent(typeof(HitBox))]
public class AutoMove : MonoBehaviour {

    public float speed;
    public float duration;
    public Rigidbody rigidbody;

    private float _birthTime;
    private bool _destroyFlag;

    private void Awake() {
        _birthTime = Time.time;
        if (!rigidbody) rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (Time.time - _birthTime >= duration) {
            Destroy(gameObject);
            return;
        }
        
        if (_destroyFlag) Destroy(gameObject, .5f);
        rigidbody.MovePosition(transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter() {
        _destroyFlag = true;
    }
    
    private void OnCollisionEnter() {
        _destroyFlag = true;
    }
}