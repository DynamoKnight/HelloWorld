using System.Collections.Generic;

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
    public static int credits {get; set;} = 0;
}