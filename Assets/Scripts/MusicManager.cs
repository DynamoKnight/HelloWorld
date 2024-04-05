using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public List<AudioClip> audioclips;

    public GameObject volumeManager;
    public VolumeManager volumeManagerScript;

    private int index;
    private float time;
    private float runtime;

    public int loopTimes = 5;

    public AudioSource music;

    private float originalVolume;


    // Start is called before the first frame update
    void Start()
    {
        originalVolume = music.volume;
        //looks for volumemanager
        List<GameObject> m = FindObjectsOfType<GameObject>().ToList();
        foreach(GameObject c in m){
            if (c.tag == "VolumeManager"){
                volumeManager = c;
            }
        }
        volumeManagerScript = volumeManager.GetComponent<VolumeManager>();
        music.volume = originalVolume * volumeManagerScript.musicVolumeMultplier;
        index = 0;
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //changes the music volume with respect to what the player choses on the slider
        music.volume = originalVolume * volumeManagerScript.musicVolumeMultplier;
        //plays next song after runtime is over
        if(Time.time - time >= runtime){
                playNext();
        }
    }

    //plays the next song
    private void playNext()
    {
        //updates index
        if(index == audioclips.Count - 1){
            index = 0;
        }
        else{
            index++;
        }
        //sets runtime to 5* audioclip length
        runtime = audioclips[index].length * 5;
        GetComponent<AudioSource>().clip = audioclips[index];
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;
        time = Time.time;
    }
    //fades music volume out UNUSED
    private IEnumerator fadeDown(){
        while(GetComponent<AudioSource>().volume >= 0f){
            GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume - 0.05f;
            yield return new WaitForSeconds(0.3f);
        }


    }
    //fades music volume in UNUSED
    private IEnumerator fadeUp(){
        while(GetComponent<AudioSource>().volume <= 0.2f){
            GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume + 0.05f;
            yield return new WaitForSeconds(0.3f);
        }

    }

}
