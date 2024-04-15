using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    // An array of every object it collided with in a frame
    private Collider2D[] hits = new Collider2D[10];

    private GameObject gm;

    [HideInInspector]
    public Inventory inventory;

    protected virtual void Start(){
        boxCollider = GetComponent<BoxCollider2D>();
        gm = GameObject.Find("GameManager");
        inventory = gm.GetComponent<Inventory>();
    }

    protected virtual void Update(){
        // Looks for collisions and adds it into the array
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++){
            // Nothing is hit
            if (hits[i] == null){
                continue;
            }
            OnCollide(hits[i]);
            // Cleans up the array
            hits[i] = null;
        }
    }
    // This is automatically called by Unity during collision
    void OnCollisionEnter2D(Collision2D collision){
        OnCollide(collision.collider);
    }

    // protected means that it is private to everything but its children
    // virtual means that the method can be overriden
    // This OnCollide method is a custom method written by me
    protected virtual void OnCollide(Collider2D coll){
        Debug.Log(coll.name);
    }
    

}
