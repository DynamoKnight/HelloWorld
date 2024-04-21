using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : MonoBehaviour
{
    // The rate at which it flips
    private float animationRate = 0.2f;
    private float timer = 0f;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    // For knockback
    private float strength = 60f;
    private float duration = 0.3f;

    void Start(){
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update(){
        if (!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // Flips around x-axis for animation effect
            if (timer >= animationRate){
                // Flips to opposite axis
                spriteRenderer.flipX = !spriteRenderer.flipX;
                timer = 0f;
            }
            else{
                timer += Time.deltaTime;
            }
        }

    }

    void OnTriggerStay2D(Collider2D coll){
        // Pushes the player back
        if (coll.name == "Player"){
            Push(coll);
        }
    }

    // Applies a big knockback effect
    public void Push(Collider2D coll){
        // Does knockback to player
        Knockback knockback = coll.GetComponent<Knockback>();
        knockback.RandomKnockback(strength, duration);
    }
}
