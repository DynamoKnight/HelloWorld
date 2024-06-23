using System.Collections.Generic;
using UnityEngine;

// Keeps track of all saved variables
public static class PlayerStats
{
    // Best times of each planet levels
    // 10 levels for each planet + moon
    public static float[] BestTimes {get; set;} = new float[10];
    

    // The totals
    public static int EnemiesDefeated {get; set;} = 0;
    public static int TotalViolations {get; set;} = 0;

    // Global data
    public static string CurrentPlanet {get; set;} = "Pluto";
    public static int PlanetsDiscovered {get; set;} = 0;
    public static int Credits {get; set;} = 0;
    // Keeps track of all the names of the collectables that have already been collected
    // Because it is by name, every collectable of the same object has to have a different name.
    public static List<string> Collected {get; set;} = new();
}