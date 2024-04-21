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
        // Sets velocity to zero if its not knocked or is an enemy
        // FIXES THE BUG AFTER FOREVER!!!!
        if (!isKnocked && gameObject.tag == "Enemy"){
            rb.velocity = Vector2.zero;
        }
    }

    // The sender is the object that hits the object
    // so that it can calculate the direction to knockback the object
    public void PlayFeedback(GameObject sender){
        // Stop the previous knockback coroutine if it's running
        if (knockbackCoroutine != null){
            StopCoroutine(knockbackCoroutine);
        }
        // Invokes the function attached to the OnBegin event
        // In this case, it disables the movement script
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        // Applies force that will not be stopped until the Reset function
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        isKnocked = true;
        // Start the new knockback coroutine
        knockbackCoroutine = StartCoroutine(Reset());
    }

    // Knocksback with overriden values
    public void PlayFeedback(GameObject sender, float strength, float delay){
        // Stores original values
        var tempDelay = this.delay;
        this.delay = delay;
        var tempStrength = this.strength;
        this.strength = strength;

        PlayFeedback(sender);
        // Sets back to original
        this.delay = tempDelay;
        this.strength = tempStrength;
    }

    // Applies knockback in a random direction to the object
    public void RandomKnockback(float strength, float delay){
        // So that original delay won't be lost
        var tempDelay = this.delay;
        this.delay = delay;

        // Stop the previous knockback coroutine if it's running
        if (knockbackCoroutine != null){
            StopCoroutine(knockbackCoroutine);
        }
        // Invokes the function attached to the OnBegin event
        OnBegin?.Invoke();
        // Sends to a random direction
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(-1, 1);
        Vector2 direction = new Vector2(randomX, randomY);
        // Applies force that will not be stopped until the Reset function
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        isKnocked = true;
        // Start the new knockback coroutine
        knockbackCoroutine = StartCoroutine(Reset());

        this.delay = tempDelay;
    }

    // A Coroutine is a enumeratable method
    // Stops the Knockback working on the rigidbody
    private IEnumerator Reset(){
        // Waits delay seconds before stopping
        yield return new WaitForSeconds(delay);
        // Stops the knockback
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        isKnocked = false;
        // Invokes the function attached to the OnDone event
        // In this case, it enables the movement script
        OnDone?.Invoke();
    }
}
