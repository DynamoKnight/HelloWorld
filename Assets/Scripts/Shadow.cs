using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{

    public Vector3 offset = new Vector3(-0.1f,-0.1f);
    public Material material;

    GameObject shadow;


    // Start is called before the first frame update
    void Start(){
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;

        shadow.transform.localPosition = offset;
        shadow.transform.rotation = Quaternion.identity;

        SpriteRenderer renderer  = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = shadow.AddComponent<SpriteRenderer>();
        sr.sprite = renderer.sprite;
        sr.material = material;

        sr.sortingLayerName = renderer.sortingLayerName;
        sr.sortingOrder = renderer.sortingOrder - 1;
    }

    // Update is called once per frame
    void LateUpdate(){
        shadow.transform.localPosition = offset;
    }



}
