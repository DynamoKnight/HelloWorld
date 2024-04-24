using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    // The rigidbody of the object
    private Rigidbody2D rb;
    [SerializeField] private float strength = 6;
    // How long until the object can move again
    [SerializeField] private float delay = 0.15f;
    private bool isKnocked = false;

    // Store a reference to the coroutine
    private Coroutine knockbackCoroutine;

    public UnityEvent OnBegin;
    public UnityEvent OnDone;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update(){
        // Sets velocity to zero if its not knocked and is an enemy
        // FIXES THE BUG AFTER FOREVER!!!!
        if (!isKnocked && gameObject.tag == "Enemy"){
            rb.velocity = Vector2.zero;
        }
    }

    // The sender is the object that hits the object
    // so that it can calculate the direction to knockback the object
    public void PlayFeedback(GameObject sender, bool isRandom){
        // Stop the previous knockback coroutine if it's running
        if (knockbackCoroutine != null){
            StopCoroutine(knockbackCoroutine);
        }
        // Invokes the function attached to the OnBegin event
        // In this case, it disables the movement script
        OnBegin?.Invoke();
        Vector2 direction;
        // Sends to a random direction
        if (isRandom){
            // From -1 to 1
            float randomX = Random.Range(-1, 1);
            float randomY = Random.Range(-1, 1);
            direction = new Vector2(randomX, randomY);
        }
        // Sends in the direction the sender was moving
        else{
            direction = (transform.position - sender.transform.position).normalized;
        }
        // Applies force that will not be stopped until the Reset function
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        isKnocked = true;
        // Start the new knockback coroutine, allows parameter
        IEnumerator coroutine = StopKnockback(delay);
        knockbackCoroutine = StartCoroutine(coroutine);
    }

    // Knockback with overriden values
    public void PlayFeedback(GameObject sender, float strength, float delay, bool isRandom){
        // Stores original values
        var tempDelay = this.delay;
        this.delay = delay;
        var tempStrength = this.strength;
        this.strength = strength;

        PlayFeedback(sender, isRandom);
        // Sets back to original
        this.delay = tempDelay;
        this.strength = tempStrength;
    }

    // A Coroutine is a enumeratable method
    // Stops the Knockback working on the rigidbody
    private IEnumerator StopKnockback(float duration){
        // Waits delay seconds before stopping
        yield return new WaitForSeconds(duration);
        // Stops the knockback
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        isKnocked = false;
        // Invokes the function attached to the OnDone event
        // In this case, it enables the movement script
        OnDone?.Invoke();
    }

}
