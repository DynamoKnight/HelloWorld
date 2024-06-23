using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // The player
    [SerializeField] protected GameObject player;
    
    protected SpriteRenderer spriteRenderer;
    // Indicates if being used
    public bool beingUsed;
    // The sound when the weapon is used
    public AudioSource attackSound;

    // Rather than children having to call base.Start(), they just call this
    protected void WeaponStart(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
    }

    protected void WeaponUpdate(){
        // Sets the volume based on what was set
        attackSound.volume = GameObject.Find("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier;
    }


    

}
