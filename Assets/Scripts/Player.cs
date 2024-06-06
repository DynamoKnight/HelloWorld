using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // The GlobalManager.instance dictates all

    protected Vector2 movement;
    protected BoxCollider2D boxcollider;
    protected Rigidbody2D rb;
    protected RaycastHit2D hit;
    protected Animator animator;

    [SerializeField] protected float moveSpeed;
    protected bool isSprinting = false;

    protected int healthPoints;
    [SerializeField] protected Image[] hearts;
    // The explosion particle system
    [SerializeField] protected GameObject explosion;
    // Default color
    protected Color spriteColor = Color.white;

    protected GameObject gm;
    protected InventoryManager inventoryManager;

    // Goes toward magnetic field
    protected bool isAttracted;
    protected Vector3 magnetPosition;
    protected float magnetStrength;

    // Audio sounds
    [SerializeField] protected AudioSource hurt;
    [SerializeField] protected AudioSource dieSounds;
    [SerializeField] protected AudioSource walk;
    [SerializeField] protected AudioSource freeze;
    

    // Start is called before the first frame update
    protected virtual void Start(){
        // The inventory is within the game manager
        gm = GameObject.Find("GameManager");
        inventoryManager = gm.GetComponent<InventoryManager>();

        healthPoints = hearts.Length;
        boxcollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHealth();
    }

    // Gets called every frame
    // All inputs should be initialized here
    protected virtual void Update(){
        // Sets volume
        hurt.volume = GameObject.FindGameObjectWithTag("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier;
        freeze.volume = GameObject.FindGameObjectWithTag("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier;

        // Only collects input if game is unpaused and player is allowed to move
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Signals the animations based on movement
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            // magnitude squared is so that value is positive
            animator.SetFloat("Speed", movement.sqrMagnitude);
            
            // Speed increases when SHIFT is held
            if (Input.GetKeyDown(KeyCode.LeftShift)){
                // Makes sure speed increases only when key is down after being up
                if (!isSprinting){
                    moveSpeed += 10;
                    isSprinting = true;
                }
                
            }
            // Goes back to default speed
            if (Input.GetKeyUp(KeyCode.LeftShift)) {
                moveSpeed -= 10;
                isSprinting = false;
            }
        }
    
    }

    // Gets called every frame at a interval of time
    protected virtual void FixedUpdate(){
        // Casts a box around character, and returns the collider that the target vector will contact
        // It will only work for objects in a specfic layer.
        // Y
        hit = Physics2D.BoxCast(rb.position, boxcollider.size, 0, new Vector2(0, movement.y), Math.Abs(movement.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        // If nothing was hit, then move
        // Skips collision with drop cause its not actual collider
        if (hit.collider == null || hit.collider.tag == "Drop"){
            // Moves player in the y direction
            Move();
        }
        // X
        hit = Physics2D.BoxCast(rb.position, boxcollider.size, 0, new Vector2(movement.x, 0), Math.Abs(movement.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null || hit.collider.tag == "Drop"){
            // Moves player in the x direction
            Move();
        }

        // Moves toward magnetic field
        if (isAttracted){
            Vector2 magnetDirection = (magnetPosition - transform.position).normalized;
            rb.velocity = magnetDirection * magnetStrength;
        }
    
    }

    // Moves the player based on the given input from Update
    protected virtual void Move(){
        // The reason there is weird movement glitches is because Transform bypasses physics. Thats why we must use Rigidbody.
        // If player is attracted to a magnetic field, they can't move
        if (!isAttracted){
            rb.MovePosition(rb.position + (moveSpeed * Time.deltaTime * movement));
        }
        walk.Play();
    }

    [ContextMenu("Kill Player")]
    // Instant kills the Player just because...
    public void TakeDamage(){
        hurt.Play();
        //oof.play();
        healthPoints = 0;
        UpdateHealth();
        Die();
    }
    // Instant kills the Player, knows what did damage to it
    public virtual void TakeDamage(GameObject sender){
        TakeDamage();
    }
    // Remove health points from the Player
    public virtual void TakeDamage(GameObject sender, int damage){
        ApplyAffect("damage");
        // Hurts player
        healthPoints -= damage;
        UpdateHealth();
        // Player dies
        if (healthPoints <= 0){
            TakeDamage();
        }
        // Player is just hurt
        else{
            hurt.Play();
        }
        
    }

    // What happens upon death
    public virtual void Die(){
        // Records time of death
        GlobalManager.instance.lastDeath = GlobalManager.instance.timePlayed;
        // Calls the GameOver method from LevelManager
        LevelManager.instance.GameOver();
        // Creates the particle effect, slightly higher
        Instantiate(explosion, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
        // Destroys player
        Destroy(gameObject);
    }

    // Shows the number of hearts
    public virtual void UpdateHealth(){
        for (int i = 0; i < hearts.Length; i++){
            // Displays regular or black heart based on health
            if (i < healthPoints){
                hearts[i].color = Color.white;
            }
            else{
                hearts[i].color = Color.black;
            }
        }
    }

    public virtual void ApplyAffect(string affect){
        if (affect == "freeze"){
            freeze.Play();
            StopAllCoroutines();
            StartCoroutine(Freeze());
        }
        else if (affect == "damage"){
            StartCoroutine(Damage());
        }
    }

    public virtual IEnumerator Freeze(){
        // Blue-ish
        float green = 150f/255f;
        float red = 0f;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(red, green, 1f);
        // Slows down character and halves animation speed
        moveSpeed -= 5;
        animator.speed = .5f;
        // Changes color overtime
        while (green < 1f || red < 1f){
            // Increase color values
            green += 0.05f;
            red += 0.05f;

            // Set the color
            gameObject.GetComponent<SpriteRenderer>().color = new Color(red, green, 1f);
            yield return new WaitForSeconds(0.1f);
        }
        // Ends the frost effect
        moveSpeed += 5;
        animator.speed = 1f;
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }

    public virtual IEnumerator Damage(){
        // Flashes damage twice
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }

    // Returns the arrow
    public virtual GameObject GetArrow(){
        return gameObject.transform.GetChild(2).gameObject;
    }

    // Sets the position for the player to move towards
    public virtual void SetMagnet(Vector3 magnetPosition, float magnetStrength){
        this.magnetPosition = magnetPosition;
        this.magnetStrength = magnetStrength;
        isAttracted = true;
    }

    // Stops the magnet effect
    public virtual void StopMagnet(){
        rb.velocity = Vector3.zero;
        isAttracted = false;
        magnetPosition = Vector3.zero;
        magnetStrength = 0;
        
    }

    // Returns the number of health points
    public virtual int GetHearts(){
        return healthPoints;
    }
    // Returns the max health
    public virtual int GetMaxHealth(){
        return hearts.Length;
    }
    // Adds hearts
    public virtual void AddHeart(int heart = 1){
        healthPoints += heart;
        UpdateHealth();
    }

}
