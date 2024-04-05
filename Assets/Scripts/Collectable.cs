using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    protected bool collected;

    protected override void OnCollide(Collider2D coll)
    {
        // Can only be collected by the Player
        if (coll.name == "Player"){
            OnCollect();
            
        }
        if (coll.gameObject.CompareTag("Bullet")){
            GameObject bullet = coll.gameObject;
            Knockback knockback = gameObject.GetComponent<Knockback>();
            knockback.PlayFeedback(bullet);
        }
    }

    // So that it can only be collected once
    protected virtual void OnCollect(){
        collected = true;
    }

}
