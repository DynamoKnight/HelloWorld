using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoopGun : Gun
{
    // How fast the gun rotates
    private float rotationSpeed = 200f;

    protected override void Start(){
        // Doesnt use WeaponStart
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        gameObject.name = "Laser Blaster";
        attackSound = GameObject.Find("Blaster").GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update(){
        WeaponUpdate();
        // Only collects input if game is unpaused and functional
        if (!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // Rotates the gun based on key press
            if (player.GetComponent<CoopPlayer>().GetID() == 1){
                RotateGun(KeyCode.Q, KeyCode.E);
                ShootGun(KeyCode.R);
            }
            else if (player.GetComponent<CoopPlayer>().GetID() == 2){
                RotateGun(KeyCode.PageUp, KeyCode.PageDown);
                ShootGun(KeyCode.Slash);
            }
            
        }
    }

    // Rotates the gun
    private void RotateGun(KeyCode counterClockwiseKey, KeyCode clockwiseKey){
        // Check if counter clockwise key is pressed
        if (Input.GetKey(counterClockwiseKey)){
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        // Check if clockwise key is pressed
        else if (Input.GetKey(clockwiseKey)){
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
        if (rb.rotation > 90 || rb.rotation < -90){
            spriteRenderer.flipY = true;
        }
        else{
            spriteRenderer.flipY = false;
        }
    }

    // Checks if shoot key is pressed
    private void ShootGun(KeyCode shootKey){
        // beingUsed is determine by WeaponSwitcher
        if(beingUsed){
            // Left Mouse Button
            if (Input.GetKeyDown(shootKey) && fireTimer >= fireRate){
                Shoot();
                // Resets timer
                fireTimer = 0f;
            }
            else{
                // Increases time until it reaches fireRate
                fireTimer += Time.deltaTime;
            }
        }
    }

}
