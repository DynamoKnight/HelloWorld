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

    private Vector2 movement;
    private BoxCollider2D boxcollider;
    private Rigidbody2D rb;
    private RaycastHit2D hit;
    private Animator animator;

    [SerializeField] private float moveSpeed;
    private bool isSprinting = false;

    public int healthPoints;
    [SerializeField] private Image[] hearts;
    // The explosion particle system
    [SerializeField] private GameObject explosion; 

    private GameObject gm;
    private Inventory inventory;

    // Goes toward magnetic field
    private bool isAttracted;
    private Vector3 magnetPosition;
    private float magnetStrength;

    // Audio sounds
    [SerializeField] private AudioSource hurt;
    [SerializeField] private AudioSource dieSounds;
    [SerializeField] private AudioSource walk;
    [SerializeField] private AudioSource freeze;
    

    // Start is called before the first frame update
    void Start(){
        // The inventory is within the game manager
        gm = GameObject.Find("GameManager");
        inventory = gm.GetComponent<Inventory>();

        boxcollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHealth();
    }

    // Gets called every frame
    // All inputs should be initialized here
    void Update(){

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
            if (Input.GetKeyUp(KeyCode.Y)) {
                SceneManager.LoadScene("NeptuneDialog");
            }
        }
    
    }

    // Gets called every frame at a interval of time
    void FixedUpdate(){
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
    private void Move(){
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
    public void TakeDamage(GameObject sender){
        TakeDamage();
    }
    // Remove health points from the Player
    public void TakeDamage(GameObject sender, int damage){
        ApplyAffect("damage");
        // Does knockback to player
        Knockback knockback = GetComponent<Knockback>();
        knockback.PlayFeedback(sender);
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
    public void Die(){
        // Records time of death
        GlobalManager.instance.lastDeath = Time.time;
        // Calls the GameOver method from LevelManager
        LevelManager.instance.GameOver();
        // Creates the particle effect, slightly higher
        Instantiate(explosion, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
        // Destroys player
        Destroy(gameObject);
    }

    public void UpdateHealth(){
        for (int i = 0; i < hearts.Length; i++){
            // Displays regular or black heart
            if (i < healthPoints){
                hearts[i].color = Color.white;
            }
            else{
                hearts[i].color = Color.black;
            }
        }
    }

    public void ApplyAffect(string affect){
        if (affect == "freeze"){
            freeze.Play();
            StopAllCoroutines();
            StartCoroutine(Freeze());
        }
        else if (affect == "damage"){
            StartCoroutine(Damage());
        }
    }

    public IEnumerator Freeze(){
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
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator Damage(){
        // Flashing damage
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Returns the arrow
    public GameObject GetArrow(){
        return gameObject.transform.GetChild(2).gameObject;
    }

    // Sets the position for the player to move towards
    public void SetMagnet(Vector3 magnetPosition, float magnetStrength){
        this.magnetPosition = magnetPosition;
        this.magnetStrength = magnetStrength;
        isAttracted = true;
    }

    // Stops the magnet effect
    public void StopMagnet(){
        rb.velocity = Vector3.zero;
        isAttracted = false;
        magnetPosition = Vector3.zero;
        magnetStrength = 0;
        
    }

}
