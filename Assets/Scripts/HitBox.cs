using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitBox : MonoBehaviour {

    public DamageType damageType;
    public int attack;
}