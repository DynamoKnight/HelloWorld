using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionItemsManager : MonoBehaviour
{

    public List<string> missionItems = new();
    public List<int> numberOfMissionItems = new();

    // Start is called before the first frame update
    void Start(){
        // Adds the required items
        missionItems.Add("Engine");
        numberOfMissionItems.Add(1);

        missionItems.Add("Oil Tanks");
        numberOfMissionItems.Add(2);

        missionItems.Add("Scrap Metal");
        numberOfMissionItems.Add(10);

    }

    // Update is called once per frame
    void Update(){
        
    }
}
