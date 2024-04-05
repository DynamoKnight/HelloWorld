using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 0.005f;
    protected Rigidbody2D rb;
    protected RaycastHit2D hit;
    protected BoxCollider2D boxcollider;
    protected GameManager gameManager;

    // The target object variables
    protected GameObject target;
    protected Vector3 targetPosition;
    List<int> availableIndexes;
    

    // Health and damage
    public int healthPoints;
    protected int touchDamage = 1;

    //Drops
    protected GameObject GameManager;
    protected Inventory inventory;
    public List<GameObject> Drops = new List<GameObject>();

    public AudioSource hurt;

    protected virtual void Start(){
        rb = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();

        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = GameManager.GetComponent<GameManager>();
        inventory = GameManager.GetComponent<Inventory>();
        availableIndexes = new List<int>();
        Drops = inventory.missionItemDrops;
        List<AudioSource> a = FindObjectsOfType<AudioSource>().ToList();
        foreach (AudioSource i in a){
            if(i.tag == "AlienAudio"){
                hurt = i;
            }
        }
    }

    protected virtual void Update(){
        // Only sets target if unpaused
        if(!LevelManager.instance.isPaused){
            if (!target){
                SetTarget();
            }
        }
        hurt.volume = GameObject.FindGameObjectWithTag("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier/10;
    }

    protected virtual void FixedUpdate(){
        var step = moveSpeed * Time.deltaTime;
        if (target){
            targetPosition = target.GetComponent<Rigidbody2D>().transform.position;
            var movement = Vector2.MoveTowards(rb.position, targetPosition, step);
            // Move towards the target
            hit = Physics2D.BoxCast(rb.position, boxcollider.size, 0, new Vector2(0, movement.y), Math.Abs(movement.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
            // If something was hit, do not move
            if (hit.collider != null && hit.collider.tag != "Drop"){
                return;
            }
            // Moves enemy in the y direction
            rb.position = movement;

            hit = Physics2D.BoxCast(rb.position, boxcollider.size, 0, new Vector2(movement.x, 0), Math.Abs(movement.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
            // If something was hit, do not move
            if (hit.collider != null && hit.collider.tag != "Drop"){
                return;
            }
            // Moves enemy in the x direction
            rb.position = movement;
        }
    }

    // Sets the target to the Player object. Default value is Player
    protected void SetTarget(string tag="Player"){
        GameObject tagObject = GameObject.FindGameObjectWithTag(tag);
        // Checks if the object is not null before setting it as the target
        if (tagObject != null){
            target = tagObject;
            targetPosition = target.GetComponent<Rigidbody2D>().transform.position;
        }
    }

    // This is automatically called by Unity during collision
    void OnCollisionEnter2D(Collision2D collision){
        OnCollide(collision.collider);
    }

    // Collision events
    protected virtual void OnCollide(Collider2D coll){
        // Gets the Player
        Player player = coll.gameObject.GetComponent<Player>();
        // If player is null, then coll was not a player object
        if (player != null){
            // Kills player
            player.TakeDamage(gameObject, touchDamage);
            target = null;
        }
    }

    // Instant kills the Enemy
    public void TakeDamage(){
        Instantiate(GetDrop(), transform.position, transform.rotation);
        Destroy(gameObject);
        gameManager.enemiesDefeated++;
    }

    // Remove health points from the Enemy
    public void TakeDamage(GameObject sender, int damage){
        // Does knockback to enemy
        Knockback knockback = GetComponent<Knockback>();
        knockback.PlayFeedback(sender);
        healthPoints -= damage;
        if (healthPoints <= 0){
            GameObject drop = GetDrop();
            if(drop != null){
                Instantiate(drop, transform.position, transform.rotation);
            }
            hurt.Play();
            Destroy(gameObject);
            gameManager.enemiesDefeated++;
        }
    }

    public GameObject GetDrop(){
        UpdateDrops();
        GameObject final = null;
        System.Random rnd = new System.Random();
    
        availableIndexes.Clear();

        for(int i = 0; i < inventory.numberOfEachMissionItem.Count; i++){
            if(inventory.numberOfEachMissionItem[i] > inventory.numberOfEachMissionItemCollected[i]){
                availableIndexes.Add(i);
            }
        }
        if(availableIndexes.Count == 0) return final;
        if(rnd.Next(0, 11) >= 4 ){
            final = Drops[availableIndexes[rnd.Next(0,availableIndexes.Count)]];
        }
        return final;
    }

    public void UpdateDrops(){
        Drops = inventory.missionItemDrops;
    }

}
