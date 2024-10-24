using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{

    // Calculates the difficulty to obtain a resource from the enemy
    public float CalculateItemDistanceModifier(List<int> vendorCoordinates, GameObject enemy){
        // Current planet enemy spawn and drop rarity hashmap
        // The List represents the coordinates, GameObject is the enemy with it's stage (difficulty)
        Dictionary<List<int>, Dictionary<GameObject, int>> currentPlanet = PlayerStats.CurrentPlanetHash;
        // Initializes variables
        float totalDistance = 0f;
        float totalProbability = 0f;
        // The number of enemy positions
        int count = 0;

        foreach (var coordinateData in currentPlanet){
            List<int> coordinates = coordinateData.Key;
            Dictionary<GameObject, int> enemyData = coordinateData.Value;

            if (enemyData.ContainsKey(enemy)){
                // Calculates distance from vendor to enemy
                // The farther away from the vendor, the rarer
                float distance = Vector2.Distance(new Vector2(vendorCoordinates[0], vendorCoordinates[1]), new Vector2(coordinates[0], coordinates[1]));
                // Gets the spawn probability of the enemy
                float probability = enemyData[enemy];
                // Sums distances and probabilities
                totalDistance += distance * probability;
                totalProbability += probability;
                count++;
            }
        }

        // Calculate the item distance modifier
        // Vector average, needs to be normalized
        float itemDistanceModifier = (totalDistance / count) / 357f;
        return itemDistanceModifier;
    }
}