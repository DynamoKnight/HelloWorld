using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    // Indicates if the object has been interacted with
    protected bool collected;

    protected override void OnCollide(Collider2D coll){
        base.OnCollide(coll);
        // Can only be collected by the Player
        if (coll.name == "Player"){
            OnCollect(); 
        }
        
    }

    // So that it can only be collected once
    protected virtual void OnCollect(){
        collected = true;
    }

}
