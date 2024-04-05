using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager instance;
    //sets sliders in the inspector; keeps track of volume settings by user
    [Range(0.0f, 1.0f)]public float musicVolumeMultplier;
    [Range(0.0f, 1.0f)]public float SFXVolumeMultplier;


    void Awake()
    {
        // Makes sure there is 1 instance of this
        if (VolumeManager.instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
