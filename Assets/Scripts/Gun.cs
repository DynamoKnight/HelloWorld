using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // The player
    [SerializeField] private GameObject player;

    protected Vector2 mousePos;
    [SerializeField] protected GameObject bulletPrefab;
    // The place where the bullet comes from
    [SerializeField] protected Transform firingPoint;
    [Range(0.1f, 2f)]
    [SerializeField] protected float fireRate = 0.5f;
    protected float fireTimer;
    protected float yscale;

    public bool beingUsed;

    public AudioSource blast;

    protected virtual void Start(){
        // 2.45
        yscale = transform.localScale.y; 
        gameObject.name = "Laser Blaster";
    }

    // Update is called once per frame
    protected virtual void Update(){
        blast.volume = GameObject.FindGameObjectWithTag("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier;
        // Only collects input if game is unpaused and functional
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            var scale = transform.localScale;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Gets the angle of the character by using arctangent of the delta position
            float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            // Rotates in z direction
            // yscale needs to match with the y scale
            transform.localRotation = Quaternion.Euler(0, 0, angle);
            if (angle > 90f || angle < -90f){
                scale.y = -yscale;
            }
            else{
                scale.y = yscale;
            }
            transform.localScale = scale;
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
        // Creates a bullet object
        blast.Play();
        // Keeps track of the bullet shot
        Bullet bulletFired = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation).GetComponent<Bullet>();
        // Tells the bullet that this is the sender
        bulletFired.SetSender(player);
    }
}
