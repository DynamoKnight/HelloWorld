using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public GameObject Target;
    public float hideDistance;

    // Orients the arrow to the target
    void Update(){
        var direction = Target.transform.position - transform.position;

        // Hides arrow if too close
        if (direction.magnitude < hideDistance){
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else{
            transform.GetChild(0).gameObject.SetActive(true);
        }
        // Finds angle with math
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
}
