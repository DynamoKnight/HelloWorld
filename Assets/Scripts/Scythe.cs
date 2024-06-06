using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : Gun
{
    // Knockback
    private float strength = 10;
    private float duration = 0.2f;

    protected override void Start(){
        WeaponStart();
        gameObject.name = "Scythe";
    }

    public float GetStrength(){
        return strength;
    }

    public float GetDuration(){
        return duration;
    }

    

}
