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
    // The GameManager dictates all
    private GameManager gm;

    private Vector2 movement;
    private BoxCollider2D boxcollider;
    private Rigidbody2D rb;
    private RaycastHit2D hit;
    private Animator animator;

    public float moveSpeed;
    private bool isSprinting = false;

    public int healthPoints;
    [SerializeField] private Image[] hearts;

    public GameObject player;
    private Inventory inventory;

    public AudioSource hurt;
    public AudioSource dieSounds;
    public AudioSource walk;
    public AudioSource freeze;
    

    // Start is called before the first frame update
    void Start()
    {
       

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();;
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


        // Only collects input if game is unpaused
        if(!LevelManager.instance.isPaused){
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
    void FixedUpdate()
    {
        // Casts a box around character, and returns the collider that the target vector will contact
        // It will only work for objects in a specfic layer.
        // Y
        hit = Physics2D.BoxCast(rb.position, boxcollider.size, 0, new Vector2(0, movement.y), Math.Abs(movement.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        // If nothing was hit, then move
        if (hit.collider == null || hit.collider.tag == "Drop"){
            // Moves player in the y direction
            // The reason there is weird movement glitches is because Transform bypasses physics. Thats why we must use Rigidbody.
            rb.MovePosition(rb.position + (movement * moveSpeed * Time.deltaTime));
            walk.Play();
        }

        // X
        hit = Physics2D.BoxCast(rb.position, boxcollider.size, 0, new Vector2(movement.x, 0), Math.Abs(movement.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null || hit.collider.tag == "Drop"){
            // Moves player in the x direction
            rb.MovePosition(rb.position + (movement * moveSpeed * Time.deltaTime));
            walk.Play();
        }
    
    }

    [ContextMenu("Kill Player")]
    // Instant kills the Player just because...
    public void TakeDamage(){
        hurt.Play();
        healthPoints = 0;
        UpdateHealth();
        Die();
        gm.lastDeath = Time.time;
    }

    // Instant kills the Player, knows what did damage to it
    public void TakeDamage(GameObject sender){
        hurt.Play();
        healthPoints = 0;
        UpdateHealth();
        Die();
        gm.lastDeath = Time.time;
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
        if (healthPoints <= 0){
            hurt.Play();
            Die();
            gm.lastDeath = Time.time;
            //dieSounds.Play();
        }
        else{
            hurt.Play();
        }
    }

    // What happens upon death
    public void Die(){
        // Calls the GameOver method from LevelManager
        LevelManager.instance.GameOver();
        gameObject.SetActive(false);
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
            StopCoroutine(Damage());
            StartCoroutine(Freeze());
        }
        else if (affect == "damage"){
            StartCoroutine(Damage());
        }
    }

    IEnumerator Freeze(){
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

}
