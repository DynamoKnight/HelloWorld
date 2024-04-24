using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class Journal : MonoBehaviour
{
    private GameObject gm;
    private Dictionary<string, string[]> infos;
    private Sprite[] images;

    private Transform journalInfo;
    private Button nextBtn;
    private Button backBtn;

    private Image image;
    private TMP_Text planet;
    private TMP_Text day;
    private TMP_Text info;
    private int idx;
    // Start is called before the first frame update
    void Start(){
        gm = GameObject.Find("GameManager");

        infos = GlobalManager.instance.GetPlanets();
        images = GlobalManager.instance.GetImages();
        // MAKE SURE THE ORDER IS RIGHT IN THE HIERARCHY
        // Gets 1st child of the journal panel
        journalInfo = gameObject.transform.GetChild(0);
        nextBtn = gameObject.transform.GetChild(1).GetComponent<Button>();
        backBtn = gameObject.transform.GetChild(2).GetComponent<Button>();
        // Gets the children of the journal info
        image = journalInfo.transform.GetChild(0).GetComponent<Image>();
        planet = journalInfo.transform.GetChild(1).GetComponent<TMP_Text>();
        day = journalInfo.transform.GetChild(2).GetComponent<TMP_Text>();
        info = journalInfo.transform.GetChild(3).GetComponent<TMP_Text>();

        // Starts on Pluto
        idx = 0;
        SetPage(idx);

        // Adding a delegate with no parameters
        nextBtn.onClick.AddListener(NextPage);
        backBtn.onClick.AddListener(PreviousPage);

    }
    // Goes to next journal page
    public void NextPage(){
        if (idx < infos.Count - 1){
            idx += 1;
        }
        SetPage(idx);

    }
    // Goes to previous journal page
    public void PreviousPage(){
        if (idx > 0){
            idx -= 1;
        }
        SetPage(idx);
    }

    // Styles the journal page based on planet
    public void SetPage(int index){
        // Darkens planet if level isn't reached
        int currentPlanetIdx = GlobalManager.instance.GetIdxOfCurrentPlanet();
        if (index > currentPlanetIdx){
            image.sprite = images[index];
            image.color = new Color(0.15f,0.15f,0.15f);
            planet.text = "???";
            day.text = "---";
            info.text = "";
        }
        // Planet has been unlocked
        else{
            image.sprite = images[index];
            image.color = Color.white;
            planet.text = infos.Keys.ToList()[index]; // Gets ith key
            day.text = "Day " + index;
            info.text = infos[planet.text][2];
        }
        
    }

}
