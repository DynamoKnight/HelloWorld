using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extends the Enemy object
public class RangedEnemy : Enemy
{
    public float distanceToShoot = 20f;

    public float fireRate;
    public float timeToFire;
    public Transform firingPoint;
    [SerializeField] private GameObject bulletPrefab;

    protected override void Start(){
        timeToFire = 0f;
        base.Start();
        SetTarget();
    }

    protected override void Update(){
        // Only if target exists
        // Only shoots if unpaused
        if(!LevelManager.instance.isPaused){
            if (target){
                // Rotate and shoot if within range of player
                RotateTowardsTarget();
                if (Vector2.Distance(targetPosition, rb.position) <= distanceToShoot){
                    Shoot();
                }
            }
            base.Update();
        }
    }

    protected override void FixedUpdate(){
        if (target){
             base.FixedUpdate();
        }
    }

    private void Shoot(){
        if (timeToFire >= fireRate){
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            timeToFire = 0f;
        }
        else{
            timeToFire += Time.deltaTime;
        }
    }

    // Turns direction of firingPoint so it can shoot
    private void RotateTowardsTarget(){
        Vector2 targetDirection = targetPosition - firingPoint.position; 
        // Gets angle to player
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        // Rotates at a smooth speed
        firingPoint.transform.localRotation = Quaternion.Slerp(firingPoint.localRotation, q, rotateSpeed);

    }

}
