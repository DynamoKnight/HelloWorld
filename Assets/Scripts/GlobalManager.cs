using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    // Global
    public static GlobalManager instance;

    public float timePlayed;
    public int enemiesDefeated;
    public int planetsDiscovered;
    public int itemsCollected;

    public float lastDeath;
    // Images of each planet
    [SerializeField] private Sprite[] images;
    // Every planet with information about it
    // Array with key-value pairs
    public Dictionary<string, string[]> infos = new Dictionary<string, string[]>
        {
            { "Pluto", new string[] {
                "Easy",
                "The Dwarf Planet",
                "The icy terrain and rocky landscape make it hard to navigate. While it's cold environment provides a challenge, the aliens settlers have made the planet a bit more bearable... ",
                "They have adapted to the cold climate and are not kind to intruders..."
                } },
            { "Neptune", new string[] {
                "Easy",
                "The Blue Giant",
                "Neptune has 14 known moons, the largest of which is Triton, which is believed to be a captured Kuiper Belt object.",
                "Frost bites "
                } },
            { "Uranus", new string[] {
                "Medium",
                "The Bull's Eye Planet",
                "Uranus is the only planet in the solar system that rotates on its side",
                "Shoots balls"
                } },
            { "Saturn", new string[] {
                "Medium",
                "The Ringed Planet",
                "Saturn's density is so low that if there were a large enough body of water, it would float in it",
                "Magnetic fields"
                } },
            { "Jupiter", new string[] {
                "Hard",
                "The Gas Giant",
                "Jupiter's Great Red Spot is a massive storm that has been raging for at least 400 years and is large enough to engulf Earth two or three times over.",
                "Big boys"
                } },
            { "Mars", new string[] {
                "Hard",
                "The Red Planet",
                "Mars has the largest volcano and canyon in the solar system, Olympus Mons and Valles Marineris respectively",
                "Sand attacks"
                } },
            { "The Moon", new string[] {
                "Extreme",
                "Luna Selene",
                "One small step for man. One giant leap for mankind.",
                "Everybody"
                } },
            { "Earth", new string[] {
                "Boss",
                "The Blue Planet",
                "The home of the advanced species classified as Humans.",
                "Boss is here"} }
            
        };

    void Awake(){
        // Makes sure there is 1 instance of this
        if (GlobalManager.instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        // Statistics
        timePlayed = 0;
        enemiesDefeated = 0;
        planetsDiscovered = 0;
        itemsCollected = 0;

        lastDeath = 0;
    }

    void Update(){
        timePlayed = Time.time - lastDeath;
    }
    
    // Returns the dictionary of planet information
    public Dictionary<string, string[]> GetPlanets(){
        return infos;
    }
    // Returns the Planet Images
    public Sprite[] GetImages(){
        return images;
    }
}
