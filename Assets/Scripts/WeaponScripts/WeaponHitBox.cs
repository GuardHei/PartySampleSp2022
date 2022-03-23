using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    private void Start()
    {
       // WeaponRB = GetComponent<Rigidbody>(); //Getting a weird compile error here
    }

    private void Update()
    {
        //Destroy Hitbox here
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // check for collision with correct type of actor
        //Call Damage here
    }
}
