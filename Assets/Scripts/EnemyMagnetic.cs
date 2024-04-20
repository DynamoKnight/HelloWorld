using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagnetic : Enemy
{
    // Different variations of the sprite when hit
    [SerializeField] private Sprite[] damage_sprites;

    // This is for when the enemy does it's magnetic pull
    private GameObject magneticField;
    // The time till it can do it's magnetic attack again
    [SerializeField] private float magneticCooldown;
    private float magneticTimer = 0f;
    [SerializeField] private float magnetStrength;

    protected override void Start(){
        //boxcollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        base.Start();

        magneticField = gameObject.transform.GetChild(0).gameObject;
        // Hides the visual
        magneticField.GetComponent<SpriteRenderer>().enabled = false;
    }

    protected override void Update(){
        base.Update();
        if (magneticTimer >= magneticCooldown){
            // Signifies that the time is reset, but is waiting for target to enter
            magneticTimer = -1f;
        }
        // Makes sure the timer won't keep going
        else if (magneticTimer != -1f){
            // Increases time until it reaches magneticCooldown
            magneticTimer += Time.deltaTime;
        }
    }

    public override void TakeDamage(GameObject sender, int damage){
        base.TakeDamage(sender, damage);
        // Gets hit, stops magnetic field
        if (sender.tag == "Bullet"){
            magneticField.GetComponent<SpriteRenderer>().enabled = false;
            target.GetComponent<Player>().StopMagnet();
            // Allows timer to start again
            magneticTimer = 0f;
        }
        // Appears damaged
        if (healthPoints == 3){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[0];
        }
        if (healthPoints == 2){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[1];
        }
        if (healthPoints == 1){
            gameObject.GetComponent<SpriteRenderer>().sprite = damage_sprites[2];
        }
    }

    
    private void OnTriggerStay2D(Collider2D coll){
        // Checks if the player is within the radius
        if (coll != null && target != null){
            if (coll.name == target.name){
                MagneticPull(coll);
            }
        }
    }

    // Makes the Player move toward if they are inside the magnetic field
    private void MagneticPull(Collider2D coll){
        // Only activates after cooldown
        if (magneticTimer == -1f){
            magneticField.GetComponent<SpriteRenderer>().enabled = true;
            coll.GetComponent<Player>().SetMagnet(gameObject.transform.position, magnetStrength);
        }
    }
}
