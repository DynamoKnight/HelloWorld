using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    // Represents the object the camera is focusing
    public Transform focus;
    // How far before the camera starts following
    public float boundX = 1f;
    public float boundY = 0.5f;

    // Gets called after Update
    private void LateUpdate(){
        // (0, 0, 0)
        Vector3 delta = Vector3.zero;
        if (focus){
            // Checks if the object it is focusing on left the bounds of the X axis
            float deltaX = focus.position.x - transform.position.x;
            if (deltaX > boundX || deltaX < -boundX){
                // The object is to the left of the focus
                if (transform.position.x < focus.position.x){
                    delta.x = deltaX - boundX;
                }
                else{
                    delta.x = deltaX + boundX;
                }
            }

            // Checks if the object it is focssing on left the bounds of the Y axis
            float deltaY = focus.position.y - transform.position.y;
            if (deltaY > boundY || deltaY < -boundY){
                // The object is to the left of the focus
                if (transform.position.y < focus.position.y){
                    delta.y = deltaY - boundY;
                }
                else{
                    delta.y = deltaY + boundY;
                }
            }

            // Moves character
            transform.position += new Vector3(delta.x, delta.y, 0);
        }
    }
}
