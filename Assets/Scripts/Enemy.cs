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
    // The explosion particle system
    [SerializeField] protected GameObject explosion;

    // The target object variables (player)
    protected GameObject target;
    protected Vector3 targetPosition;

    // Health and damage
    [SerializeField] protected int healthPoints = 1;
    // The damage it deals
    protected int touchDamage = 1;

    // Drops
    protected GameObject GameManager;
    protected Inventory inventory;
    public List<GameObject> drops = new();
    // The index of what drops it can drop
    private List<int> availableIndexes;
    // The chance out of 10 to drop an item when killed
    public int dropChance = 7;

    // Audio
    public AudioSource hurt;

    protected virtual void Start(){
        rb = GetComponent<Rigidbody2D>();
        // For the magnetic enemy
        if (boxcollider == null){
            boxcollider = GetComponent<BoxCollider2D>();
        }

        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        inventory = GameManager.GetComponent<Inventory>();
        availableIndexes = new List<int>();
        drops = inventory.missionItemDrops;
        //
        List<AudioSource> a = FindObjectsOfType<AudioSource>().ToList();
        foreach (AudioSource i in a){
            if(i.tag == "AlienAudio"){
                hurt = i;
            }
        }
    }

    protected virtual void Update(){
        // Only sets target if unpaused and functional
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            if (!target){
                SetTarget();
            }
        }
        hurt.volume = GameObject.FindGameObjectWithTag("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier/10;
    }

    protected virtual void FixedUpdate(){
        var step = moveSpeed * Time.deltaTime;
        // Game must be unpaused and functional
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            if (target){
                targetPosition = target.GetComponent<Rigidbody2D>().transform.position;
                Vector2 movement = Vector2.MoveTowards(rb.position, targetPosition, step);
                // Move towards the target in y direction
                hit = Physics2D.BoxCast(rb.position, boxcollider.size, 0, new Vector2(0, movement.y), Math.Abs(movement.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
                // If it hit nothing, then move
                if (hit.collider == null || hit.collider.tag == "Drop"){
                    Move(movement);
                }
                
                // Move towards the target in X direction
                hit = Physics2D.BoxCast(rb.position, boxcollider.size, 0, new Vector2(movement.x, 0), Math.Abs(movement.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
                if (hit.collider == null || hit.collider.tag == "Drop"){
                    Move(movement);
                }
            }
        }

    }

    // Moves the enemy to the position
    protected void Move(Vector2 movement){
        // Moves enemy in a direction
        rb.position = movement;
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
        // Drops the drop
        GameObject drop = GetDrop();
        if(drop != null){
            Instantiate(drop, transform.position, transform.rotation);
        }
        // Creates the particle effect, slightly higher
        GameObject particles = Instantiate(explosion, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
        // Death
        Destroy(gameObject);
        // Destroys particles after 5 seconds
        Destroy(particles, 5f);
        GlobalManager.instance.enemiesDefeated++;
    }

    // Remove health points from the Enemy
    public virtual void TakeDamage(GameObject sender, int damage){
        healthPoints -= damage;
        if (healthPoints <= 0){
            TakeDamage();
        }
        hurt.Play();
    }

    // Returns a needed drop
    public GameObject GetDrop(){
        UpdateDrops();
        // The drop gameobject
        GameObject final = null;
        System.Random rnd = new System.Random();
        // Clears the list
        availableIndexes.Clear();
        // If the player hasn't collected all the mission items, then the enemy can still drop them
        for (int i = 0; i < inventory.numberOfEachMissionItem.Count; i++){
            if(inventory.numberOfEachMissionItemCollected[i] < inventory.numberOfEachMissionItem[i]){
                availableIndexes.Add(i);
            }
        }
        // No drops available because all have been collected
        if (availableIndexes.Count == 0){
            return final;
        }
        // Has a specific chance out of 10 to drop something
        if (rnd.Next(0, 11) <= dropChance){
            // Gets a random drop from the available drops
            final = drops[availableIndexes[rnd.Next(0,availableIndexes.Count)]];
        }
        return final;
    }
    // Sets the possible drops to those from the level's mission items
    public void UpdateDrops(){
        drops = inventory.missionItemDrops;
    }

}
