
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
// UNUSED
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{

    private GameObject gameManager;
    private Inventory inventory;
    
    public GameObject AntiFrostBoots;

    List<string> itemList = new List<string>();

    private string currentItem;
    private string pastItem;
    private int currentInd;


    // Start is called before the first frame update
    void Start(){
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        inventory = gameManager.GetComponent<Inventory>();

        currentItem = itemList[0];
        currentInd = 0;
    }

    // Update is called once per frame
    void Update(){
        //if on the last item in the list, resets back to the first index
        if(Input.GetKeyDown("E")){
            if(currentInd == itemList.Count-1){
                pastItem = currentItem;
                currentItem = itemList[0];
                currentInd = 0;
                switchItem();
            }
            //
            else {
                pastItem = currentItem;
                currentItem = itemList[currentInd + 1];
                currentInd++;
                switchItem();
            }
        }

        else if(Input.GetKeyDown("E")){
            if(currentInd == 0){
                pastItem = currentItem;
                currentItem = itemList[itemList.Count - 1];
                currentInd = itemList.Count - 1;
                switchItem();
            }
            else {
                pastItem = currentItem;
                currentItem = itemList[currentInd - 1];
                currentInd--;
                switchItem();
            }
        }
    }
    //switches item shown on screen
    private void switchItem(){
        nameToObject(pastItem).GetComponent<SpriteRenderer>().enabled = false;
        nameToObject(currentItem).GetComponent<SpriteRenderer>().enabled = true;
    }

    //UNUSED
    private GameObject nameToObject(string name){
        if(name == "AntiFrostBoots"){
            return AntiFrostBoots;
        }
        else return null;
    }
}
