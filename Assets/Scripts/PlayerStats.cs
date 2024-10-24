using System.Collections.Generic;
using UnityEngine;

// Keeps track of all saved variables
public static class PlayerStats
{
    // Best times of each planet levels
    // 10 levels for each planet + moon
    //public static float[] BestTimes {get; set;} = new float[10];
    

    // The totals
    public static int EnemiesDefeated {get; set;} = 0;
    public static int TotalViolations {get; set;} = 0;
    public static double Debt {get; set;} = 5000000.00;

    public static Dictionary<List<int>, Dictionary<GameObject, int>> CurrentPlanetHash {get; set; }= new();

    // Global data
    public static string CurrentPlanet {get; set;} = "Pluto";
    public static string BestPlanet {get; set;} = "Pluto";
    public static int PlanetsDiscovered {get; set;} = 0;
    // The total time it took for the player to finish the game
    public static float TimePlayed {get; set;} = 0;
    // Keeps track of all the names of the collectables that have already been collected
    // Because it is by name, every collectable of the same object has to have a different name.
    public static List<string> Collected {get; set;} = new();

    // Constant data
    // Time in seconds before the game is over
    public static float TimeLimit {get; set;} = 3600;

    // Advanced Rules
    public static bool IsLimit {get; set;} = true;

    // Resets totals and game data back to start
    public static void ResetStats(){
        EnemiesDefeated = 0;
        TotalViolations = 0;
        PlanetsDiscovered = 0;
        ResetGame();
    }

    // Resets just the game data
    public static void ResetGame(){
        BestPlanet = "Pluto";
        Collected = new List<string>();
        TimePlayed = 0;
        Inventory.ResetInventory();
    }

    // Every planet with information about it
    // Array with key-value pairs
    // Planet, [Difficulty, Nickname, Info, Enemies, Temperature]
    public static Dictionary<string, string[]> PlanetInfos = new()
    {
        { "Pluto", new string[] {
            "Easy",
            "The Dwarf Planet",
            "The icy terrain and rocky landscape make it hard to navigate. While it's cold environment provides a challenge, the aliens settlers have made the planet a bit more bearable... ",
            "Touching the evil robots, their attacks, or other hazardous materials deal damage. They have adapted to the cold climate and are not kind to intruders...",
            "-375"
            } },
        { "Neptune", new string[] {
            "Easy",
            "The Blue Giant",
            "Neptune has 14 known moons, the largest of which is Triton, which is believed to be a captured Kuiper Belt object. Using your jetpack, you are able to float above the gas giant...",
            "Due to the harsh climate, they unleash frost bite attacks which will freeze anyone it touches...",
            "-330"
            } },
        { "Uranus", new string[] {
            "Medium",
            "The Bull's Eye Planet",
            "Uranus is the only planet in the solar system that rotates on its side. It has an extremely powerful atmospheric pressure that makes movement laborious...",
            "They have harnessed the gases abundant in the atmosphere to release ionized projectiles....",
            "-320"
            } },
        { "Saturn", new string[] {
            "Medium",
            "The Ringed Planet",
            "Saturn's density is so low that if there were a large enough body of water, it would float in it. Swirling clouds and powerful winds make it a challenging planet to traverse...",
            "The planets strong magnetic fields have been exploited by the enemies...",
            "-220"
            } },
        { "Jupiter", new string[] {
            "Hard",
            "The Gas Giant",
            "Jupiter's Great Red Spot is a massive storm that has been raging for at least 400 years and is large enough to engulf Earth two or three times over.",
            "Big boys",
            "-166"
            } },
        { "Mars", new string[] {
            "Hard",
            "The Red Planet",
            "Mars has the largest volcano and canyon in the solar system, Olympus Mons and Valles Marineris respectively",
            "Sand attacks",
            "-85"
            } },
        { "The Moon", new string[] {
            "Extreme",
            "Luna Selene",
            "One small step for man. One giant leap for mankind.",
            "Everybody",
            "0"
            } },
        { "Earth", new string[] {
            "Boss",
            "The Blue Planet",
            "The home of the advanced species classified as Humans.",
            "Boss is here",
            "59"
            } },
        { "Venus", new string[] {
            "Extreme",
            "The Morning Star",
            "Hot.",
            "Robots",
            "867"
            } },
        { "Mercury", new string[] {
            "Extreme",
            "The Swift Planet",
            "Close to Sun.",
            "Firey",
            "333"
            } }
            
    };

}