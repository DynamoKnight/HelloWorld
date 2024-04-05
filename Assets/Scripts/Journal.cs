using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class Journal : MonoBehaviour
{
    [SerializeField] private Sprite[] images;
    Dictionary<string, string> infos;

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
        // Array with key-value pairs
        infos = new Dictionary<string, string>
        {
            { "Pluto", "Pluto is a dwarf planet that used to be considered part of the Solar System." },
            { "Neptune", "Neptune has 14 known moons, the largest of which is Triton, which is believed to be a captured Kuiper Belt object." },
            { "Uranus", "Uranus is the only planet in the solar system that rotates on its side" },
            { "Saturn", "Saturn's density is so low that if there were a large enough body of water, it would float in it" },
            { "Jupiter", "Jupiter's Great Red Spot is a massive storm that has been raging for at least 400 years and is large enough to engulf Earth two or three times over." },
            { "Mars", "Mars has the largest volcano and canyon in the solar system, Olympus Mons and Valles Marineris respectively" },
            { "The Moon", "One small step for man. One giant leap for mankind." },
            { "Earth", "The home of the advanced species classified as Humans." }
            
        };
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
        int currentPlanetIdx = infos.Keys.ToList().IndexOf(LevelManager.instance.currentPlanet);
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
            info.text = infos[planet.text];
        }
        
    }

}
