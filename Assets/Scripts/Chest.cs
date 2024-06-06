using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    // We should make a new class called TreasureChest
    [SerializeField] private int coinsAmount;

    // The item to be collected
    [SerializeField] private GameObject drop;
    private GameObject chestDrop;

    protected override void Start(){
        chestDrop = gameObject.transform.GetChild(0).gameObject;
        // Sets the drop in the chest to the desired drop
        chestDrop.GetComponent<SpriteRenderer>().sprite = drop.GetComponent<SpriteRenderer>().sprite;
        base.Start();
    }

    // Overrides the Parents OnCollide method
    protected override void OnCollect(){
        // Can only be collected if not collected
        if (!collected){
            base.OnCollect();
            // Changes the image when collided
            gameObject.GetComponent<SpriteRenderer>().sprite = emptyChest;
            // Rewards Player
            Inventory.CollectCredits(coinsAmount);
            Debug.Log("You gained " + coinsAmount + " coins");

            // Gives the player the drop
            inventoryManager.CollectItem(drop);
            chestDrop.SetActive(false);
        }
        
    }
}
