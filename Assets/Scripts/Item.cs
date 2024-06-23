using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows class to be manipulated in the Inspector
[System.Serializable]
public class Item
{
    // Represents an inventory object
    public GameObject prefab;
    // Comes from the prefab, but here for easier access
    public string name;
    public Sprite image;

    // CONSTRUCTOR
    public Item(GameObject prefab){
        this.prefab = prefab;
        this.name = prefab.name;
        this.image = prefab.GetComponent<SpriteRenderer>().sprite;
    }

}
