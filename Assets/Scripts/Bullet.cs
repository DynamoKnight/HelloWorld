using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Bullet : MonoBehaviour
{
    // Restricts the value of speed (Makes into a Slider)
    [Range(1, 50)]
    // Serialzed means that the value is written and stored in Unity's scene file
    // Non-serialized variables have dependant values
    [SerializeField] public float speed = 50f;
    [Range(1, 10)]
    [SerializeField] private float lifeTime = 2f;
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private int bulletDamage = 1;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>(); // Throws MissingComponentException??
        // Ignores collision with the object that is shooting
        if(gameObject.tag == "Bullet"){
            GameObject player = GameObject.Find("Player");     
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), cc);
        }
        else if (gameObject.tag == "EnemyBullet"){
            // Since there are multiple enemies, each one has to be ignored
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies){
                Physics2D.IgnoreCollision(enemy.GetComponent<BoxCollider2D>(), cc);
            } 
            
        }
        
        // Kills itself after lifeTime
        Destroy(gameObject, lifeTime);
    }
    
    private void FixedUpdate(){
        // Moves forever right
        rb.velocity = transform.right * speed;
    }

    // This is automatically called by Unity during collision
    void OnCollisionEnter2D(Collision2D collision){
        OnCollide(collision.collider);
    }

    // Collision with object
    public void OnCollide(Collider2D coll){
        // Hits player
        if(gameObject.CompareTag("EnemyBullet")){
            Player player = coll.gameObject.GetComponent<Player>();
            if (player){
                player.TakeDamage(gameObject, bulletDamage);
            }
        }
        // Hits enemy
        if(gameObject.CompareTag("Bullet")){
            Enemy enemy = coll.gameObject.GetComponent<Enemy>();
            if (enemy){
                enemy.TakeDamage(gameObject, bulletDamage);
            }
        }
        Destroy(gameObject);
    }
    
        

}
