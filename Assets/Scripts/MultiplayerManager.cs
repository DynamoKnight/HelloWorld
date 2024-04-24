using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField] private GameObject waveText;
    private int currentWave;

    // Start is called before the first frame update
    void Start(){
        currentWave = 0;
    }

    // Starts the next wave
    public void StartWave(int wave){
        currentWave = wave;
        waveText.SetActive(true);
        waveText.GetComponent<TMP_Text>().text = "Wave " + currentWave;
    }

    public void CompleteWave(int wave){
        waveText.GetComponent<TMP_Text>().text = "Wave Completed";
    }
    
}
