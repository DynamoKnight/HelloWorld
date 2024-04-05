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

    // Store a reference to the coroutine
    private Coroutine knockbackCoroutine;

    public UnityEvent OnBegin;
    public UnityEvent OnDone;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // The sender is the object that hits the object
    // so that it can calculate the direction to knockback the object
    public void PlayFeedback(GameObject sender){
        // A Coroutine is a enumeratable method

        // Stop the previous knockback coroutine if it's running
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }
        // Invokes the function attached to the OnBegin event
        // In this case, it disables the movement script
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        // Applies force at an instant
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        // Start the new knockback coroutine
        knockbackCoroutine = StartCoroutine(Reset());
    }
    
    // Stops the Knockback working on the rigidbody
    private IEnumerator Reset(){
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.zero);
        // Invokes the function attached to the OnDone event
        OnDone?.Invoke();
    }
}
