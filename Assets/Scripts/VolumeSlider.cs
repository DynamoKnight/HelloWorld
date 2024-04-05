using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{

    public GameObject MusicSlider;
    public GameObject SFXSlider;
    public VolumeManager volumeManager;

    public AudioMixer audioMixer;


    // Update is called once per frame
    void Update()
    {
        volumeManager.musicVolumeMultplier = MusicSlider.GetComponent<Slider>().value;
        volumeManager.SFXVolumeMultplier = SFXSlider.GetComponent<Slider>().value;

        //audioMixer.SetFloat("Music", volume);
    }
}
