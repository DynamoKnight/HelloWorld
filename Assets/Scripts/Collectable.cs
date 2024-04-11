using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    // Indicates if the object has been interacted with
    protected bool collected;

    protected override void OnCollide(Collider2D coll){
        // Can only be collected by the Player
        if (coll.name == "Player"){
            OnCollect(); 
        }
        // Allows it to be knocked back by bullet
        if (coll.gameObject.CompareTag("Bullet")){
            GameObject bullet = coll.gameObject;
            Destroy(bullet);
            /*Knockback knockback = gameObject.GetComponent<Knockback>();
            knockback.PlayFeedback(bullet);*/
        }
    }

    // So that it can only be collected once
    protected virtual void OnCollect(){
        collected = true;
    }

}
