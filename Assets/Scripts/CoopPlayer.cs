using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopPlayer : Player
{
    // For Multiplayer, indicates player
    [SerializeField] private int playerID = 1;

    protected override void Start(){
        gm = GameObject.Find("GameManager");

        healthPoints = hearts.Length;
        boxcollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHealth();

        if (playerID == 1){
            // Slightly blue
            spriteColor = new Color(0.43f, 0.43f, 1);
        }
        else if (playerID == 2){
            // Slightly green
            spriteColor = new Color(0.43f, 1, 0.43f);
        }
        // Sets the color
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }

    protected override void Update(){
        // Sets volume
        hurt.volume = GameObject.FindGameObjectWithTag("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier;
        freeze.volume = GameObject.FindGameObjectWithTag("VolumeManager").GetComponent<VolumeManager>().SFXVolumeMultplier;

        // Only collects input if game is unpaused and player is allowed to move
        if(!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            int horizontalInput = 0;
            int verticalInput = 0;
            // Assigns movement input
            if (playerID == 1){
                // Manually assigned to WASD
                horizontalInput = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
                verticalInput = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0;
            }
            else if (playerID == 2){
                // Manually assigned to Arrow key
                horizontalInput = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
                verticalInput = Input.GetKey(KeyCode.DownArrow) ? -1 : Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
            }
            movement.x = horizontalInput;
            movement.y = verticalInput;

            // Signals the animations based on movement
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            // magnitude squared is so that value is positive
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    
    }

    // Returns the id of the player
    public int GetID(){
        return playerID;
    }
}
