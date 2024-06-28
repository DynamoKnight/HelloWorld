using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows class to be manipulated in the Inspector
[System.Serializable]
public class SaleItem
{
    // The game object contains the name
    public GameObject gameObject;
    // The value of the item to buy/sell
    public int cost;
    // The amount of the item for 1 cost
    public int quantity;
    // The amount of the item in stock
    public int stock = 100;
    // The quantitiy of the item either bought or sold with the player
    [HideInInspector] public int exchanged;
    // The position of the sale item in the trade list (trade panel)
    [HideInInspector] public int index;

    // Constructor
    public SaleItem(GameObject gameObject, int cost, int quantity){
        this.gameObject = gameObject;
        this.cost = cost;
        this.quantity = quantity;
    }

    // Sets the position of the item
    public int SetIndex(int index){
        this.index = index;
        return this.index;
    }
    

}

