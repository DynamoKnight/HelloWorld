using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : Gun
{
    protected override void Start(){
        base.Start();
        gameObject.name = "Scythe";
    }
    
    protected override void Update(){
        // Only collects input if game is unpaused and functional
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            var scale = transform.localScale;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Gets the angle of the character by using arctangent of the delta position
            float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            // Rotates in z direction
            transform.localRotation = Quaternion.Euler(0, 0, angle);
            if (angle > 90f || angle < -90f){
                scale.y = -yscale;
            }
            else{
                scale.y = yscale;
            }
            transform.localScale = scale;
        
            // Left Mouse Button
            if (Input.GetMouseButton(0) && fireTimer >= fireRate && beingUsed){
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
