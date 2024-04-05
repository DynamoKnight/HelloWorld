using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinsAmount = 5;

    // Overrides the Parents OnCollide method
    protected override void OnCollect(){
        if (!collected){
            base.OnCollect();
            // Changes the image when collided
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            inventory.AddCoins(coinsAmount);
            Debug.Log("You gained " + coinsAmount + " coins");
        }
        
    }
}
