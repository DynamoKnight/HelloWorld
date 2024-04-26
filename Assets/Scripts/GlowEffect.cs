using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    private int rotationSpeed = 100;

    // Update is called once per frame
    void FixedUpdate(){
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
