using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Weapon
{

    protected Rigidbody2D rb;

    protected Vector2 mousePos;
    [SerializeField] protected GameObject bulletPrefab;
    // The place where the bullet comes from
    [SerializeField] protected Transform firingPoint;
    [Range(0.1f, 2f)]
    [SerializeField] protected float fireRate = 0.5f;
    protected float fireTimer;


    protected virtual void Start(){
        WeaponStart();
        attackSound = GameObject.Find("Blaster").GetComponent<AudioSource>();
        gameObject.name = "Laser Blaster";
    }

    // Update is called once per frame
    protected virtual void Update(){
        WeaponUpdate();
        // Only collects input if game is unpaused and functional
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Gets the angle of the character by using arctangent of the delta position
            float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            // Rotates in z direction
            // Flips along the y-axis
            transform.localRotation = Quaternion.Euler(0, 0, angle);
            if (angle > 90f || angle < -90f){
                spriteRenderer.flipY = true;
            }
            else{
                spriteRenderer.flipY = false;
            }
            // Only fires when in use
            // beingUsed is determine by WeaponSwitcher
            if(beingUsed){
                // Left Mouse Button
                if (Input.GetMouseButton(0) && fireTimer >= fireRate){
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
    // Spawns a bullet
    protected virtual void Shoot(){
        attackSound.Play();
        // Keeps track of the bullet object shot
        Bullet bulletFired = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation).GetComponent<Bullet>();
        // Tells the bullet that this is the sender
        bulletFired.SetSender(player, gameObject);
    }
}
