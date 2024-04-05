using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostEnemy : Enemy
{

    protected override void Start(){
        base.Start();
    }

    protected override void Update(){
        // Only sets target if unpaused
        if(!LevelManager.instance.isPaused){
            base.Update();
        }
    }

    protected override void FixedUpdate(){
        base.FixedUpdate();
    }

    // This is automatically called by Unity during collision
    void OnCollisionEnter2D(Collision2D collision){
        OnCollide(collision.collider);
    }

    // Collision events
    protected override void OnCollide(Collider2D coll){
        // Gets the Player
        Player player = coll.gameObject.GetComponent<Player>();
        // If player is null, then coll was not a player object
        if (player != null){
            // Kills player
            player.ApplyAffect("freeze");
            player.TakeDamage(gameObject, touchDamage);
            target = null;
        }
    }

}
