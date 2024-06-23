using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using UnityEditor;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    // To access the inventory (or Q)
    public Button inventoryButton;
    // Indicates whether the inventory is open or closed
    public List<Sprite> inventoryImages;
    private int imgIdx = 0;

    [SerializeField] Player player; // Should player be the GameObject or the script?

    // Displays Weapons & other Collectables
    public GameObject inventoryPanel;
    // Mission items
    public GameObject sideInventoryPanel;

    // From the Side Inventory Panel
    private List<Image> missionItemImages = new();
    private List<Text> amountTexts = new();
    private List<Image> colorBackgrounds = new();

    // Briefly shows to indicate pickup
    [SerializeField] private GameObject eBtn;

    // Indicates that all required items are collected
    public bool missionItemsCollected;
    // Displays text that says mission items are all collected
    public GameObject missionText;

    // List of weapons and resources to start with in the level
    // This will usually always be just the Gun and Scythe
    public List<GameObject> items;

    // The mission item objects
    public List<MissionItem> missionItems;

    private float cooldown;
    private float time;
    [SerializeField] private float pickupDistance = 0.1f;

    // The credit box
    [SerializeField] private GameObject currencyText;

    // Awake is called even before Start
    void Awake(){
        // Sets cooldown for picking up items
        cooldown = 0.2f;
        time = Time.time;

        // Finds all prefabs
        List<GameObject> prefabs = new();
        // Gets objects of type prefab
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        // A GUID is an identifier for assets
        foreach (string guid in guids){
            // Converts all GUIDS to paths to the assets
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null){
                prefabs.Add(prefab);
            }
        }
        // Stores in the Inventory class
        Inventory.Prefabs = prefabs;

    }

    void Start(){
        // From the Inventory, previous data
        foreach (GameObject item in Inventory.Items.Keys){
            AddToPlayer(item);
        }
        // Adds the initial level-specific items to the main inventory and player
        foreach (GameObject item in items){
            Inventory.CollectItem(item);
            AddToPlayer(item);
        }
        // Multiplayer inventory is fixed
        if (SceneManager.GetActiveScene().name == "Multiplayer"){
            return;
        }

        // Adds a listener to the inventory button
        inventoryButton.onClick.AddListener(ToggleInventory);
        // Stores the objects from the side inventory panel
        for (int i = 0; i < sideInventoryPanel.transform.childCount; i++){
            // There are children inside the mission item image
            Transform missionItemImage = sideInventoryPanel.transform.GetChild(i);
            missionItemImages.Add(missionItemImage.GetComponent<Image>());
            amountTexts.Add(missionItemImage.GetChild(0).GetComponent<Text>());
            colorBackgrounds.Add(missionItemImage.GetChild(1).GetComponent<Image>());
        }

        // Sets the images for the inventory panel
        SetInventory();
        // For the next level
        LevelManager.instance.levelComplete = false;
        // Now that the objects are found, the values can be set
        Reset();
        SetMissionItems();
        // Checks if all items are collected
        CheckIfDone();
    }

    // Update is called once per frame
    void Update(){
        // Multiplayer inventory is fixed
        if (SceneManager.GetActiveScene().name == "Multiplayer"){
            return;
        }

        // Only when game is unpaused and functional
        if (!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // Inventory Key
            if (Input.GetKeyDown(KeyCode.Q)) {
                ToggleInventory();
            }
            // Checks for E or Right Mouse and if so, looks for items in the area to be picked up. 
            // Runs Collectitem to collect item
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButton(1)) && Time.time - time >= cooldown){
                time = Time.time;
                // Finds everything that it collided within the pickup distance
                Collider2D[] coll = Physics2D.OverlapCircleAll(player.transform.position, pickupDistance);
                if (coll != null){
                    for (int i = 0; i< coll.Length; i++){
                        // Ensures it is a drop
                        if (coll[i].CompareTag("Drop") && coll[i] != null){
                            // Collects and destroys the drop
                            CollectItem(coll[i].gameObject);
                            coll[i].GetComponent<SpriteRenderer>().enabled = false;
                            Destroy(coll[i].GameObject());
                            // Only one item can be picked up at once
                            break;
                        }
                    }
                }
            }
            // Flashes the E button if nearby a collectable
            Collider2D[] nearby = Physics2D.OverlapCircleAll(player.transform.position, pickupDistance);
            if (nearby != null){
                for (int i = 0; i < nearby.Length; i++){
                    if (nearby[i].CompareTag("Drop") && nearby[i] != null){
                        eBtn.SetActive(true);
                        break;
                    }
                    else{
                        eBtn.SetActive(false);
                    }
                }
            }
        }
        // Updates the number of credits seen
        TMP_Text creditsText = currencyText.transform.GetChild(0).GetComponent<TMP_Text>();
        creditsText.text = Inventory.Credits.ToString();

    }

    // Updates the Inventory panel with the items in the inventory
    public void SetInventory(){
        for (int i = 0; i < Inventory.Items.Keys.Count; i++){
            // Represents a key-value pair of the gameobject and the count
            var kvItem = Inventory.Items.ElementAt(i);
            int count = kvItem.Value;
            // Gets the quantity (3) of the item at the exact position
            GameObject slotText = inventoryPanel.transform.GetChild(i).transform.GetChild(3).gameObject;
            // Only shows the count if there is more than 1 quantity
            if (count > 1){
                slotText.SetActive(true);
                slotText.GetComponent<TMP_Text>().text = count.ToString();
            }
            else{
                slotText.SetActive(false);
            }
            Sprite image = kvItem.Key.GetComponent<SpriteRenderer>().sprite;
            // Gets the image slot (0) at the exact position
            Image inventorySlot = inventoryPanel.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>();
            // Activates the image with visible properties
            inventorySlot.sprite = image;
            inventorySlot.color = new Color(255, 255, 255, 1);
            inventorySlot.preserveAspect = true;
        }
    }
    
    // Everything is reset in terms of what is displayed on the inventory on the screen and replaces them to their defaults 
    public void Reset(){
        for (int i = 0; i < missionItems.Count; i++){
            // Remembers the index UNUSED
            missionItems[i].SetIndex(i);
            // Sets the text to the name of the mission item
            amountTexts[i].text = "0/" + missionItems[i].gameObject.name + " ";
            amountTexts[i].color = Color.black;
            // White but transparent
            colorBackgrounds[i].color = new Color(1, 1, 1, 0);
        }
    }

    // Sets the image and text for each mission item on the panel
    public void SetMissionItems(){
        for(int i = 0; i < missionItems.Count; i++){
            missionItemImages[i].sprite = missionItems[i].gameObject.GetComponent<SpriteRenderer>().sprite;
            // Sets the quantity of the mission item collected if already in the inventory
            try{
                GameObject item = Inventory.GetPrefabFromObject(missionItems[i].gameObject);
                // May not exist in the Inventory
                // Sets done if already collected all required quantities
                if (Inventory.Items[item] >= missionItems[i].toCollect){
                        SetAsDone(i);
                }
                amountTexts[i].text = Inventory.Items[item].ToString() + "/" + missionItems[i].toCollect.ToString();
            }
            catch (KeyNotFoundException){
                // No quantity of the mission item has been collected
                amountTexts[i].text = "0/" + missionItems[i].toCollect.ToString();
                //Debug.Log("The mission item " + missionItems[i].gameObject.name + " was not found in the Inventory!");
            }
            
        }
    }

    // Increments the mission item counter with the position of the exact object through the parameter id
    public void AddOneToItemCount(int id){
        string text = amountTexts[id].text.ToString();
        string[] nums = text.Split("/");

        nums[0] = Regex.Replace(nums[0], @"\s+", "");
        nums[1] = Regex.Replace(nums[1], @"\s+", "");
        amountTexts[id].text = (int.Parse(nums[0]) + 1).ToString() + "/" + nums[1] + "  ";        
    }

    // When all quantities of an item is collected, the inventory spaces turn green to show that it is done 
    public void SetAsDone(int id){
        missionItems[id].isCollected = true;
        amountTexts[id].color = Color.green;
        // Slightly transparent green
        colorBackgrounds[id].color = new Color(0, 1, 0, 0.025f);
    }

    // Opens and closes the inventory panel
    private void ToggleInventory(){
        // PLAY INVENTORY SOUND
        // Pan transition
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        // Toggles open and closed inventory image
        Image inventoryImage = inventoryButton.GetComponent<Image>();
        // If 0 set to 1, else if 1 set to 0
        imgIdx = imgIdx == 0 ? 1 : 0;
        inventoryImage.sprite = inventoryImages[imgIdx];
    }

    // Collects the item to the inventory
    public void CollectItem(GameObject drop){
        // Adds item to the inventory
        bool collected = Inventory.CollectItem(drop);
        // Checks if the item is a mission item
        for (int i = 0; i < missionItems.Count; i++){
            // Checks if objects match by name, the name contains "(Clone)"
            if (drop.name.Contains(missionItems[i].gameObject.name)){
                if (collected){
                    // Updates the screen
                    AddOneToItemCount(i);
                    GameObject item = Inventory.GetPrefabFromObject(drop);
                    SetInventory();
                    AddToPlayer(item);
                    // Checks if all quantities of that mission item is collected
                    if (Inventory.Items[item] >= missionItems[i].toCollect){
                        SetAsDone(i);
                    }
                }
                // If it could not collect then the inventory is full
            }
            // If the object is not a mission item, it is still collected
        }
        // Checks if all items are collected
        CheckIfDone();
    }

    // Checks if all mission items are collected by iterating through each mission item. 
    public void CheckIfDone(){
        foreach(MissionItem missionItem in missionItems){
            if(!missionItem.isCollected){
                // If one item isn't enough, then not all items are collected
                return;
            }
        }
        // Allows Player to Leave
        SetDone();
    }

    // Indicates that the level is completed
    private void SetDone(){
        LevelManager.instance.levelComplete = true;
        missionItemsCollected = true;
        missionText.SetActive(true);
        // Sets arrow active to find ship
        player.GetArrow().SetActive(true);
        // Hides mission text after 7 seconds
        Invoke("TurnOffMissionHeader", 7);
    }

    // Allows level to be finished in Inspector
    [ContextMenu("Auto-Complete Level")]
    public void FinishLevel(){
        for (int i = 0; i < missionItems.Count; i++){
            SetAsDone(i);
        }
        SetDone();
    }

    // Finds weapon by name UNUSED
    private bool LookUpWeapon(string name){
        bool found = false;
        for(int i = 0; i < items.Count; i++){
            if(items[i].name == name){
                found = true;
                return found;
            }
        }
        return found;
    }

    // Gets item list 
    public List<GameObject> GetItemList(){
        return items;
    }


    // Turns off the mission complete text when needed. 
    public void TurnOffMissionHeader(){
        missionText.SetActive(false);
    }

    // Shows the border around the currently used item slot, and breifly shows the name
    public void HighlightSlot(int index){
        // Uses ElementAt in order for index to work, since Dictionary is unordered
        string item = Inventory.Items.ElementAt(index).Key.name;
        UnHighlightAllSlots();
        GameObject border = inventoryPanel.transform.GetChild(index).GetChild(1).gameObject;
        // Green border is shown
        border.SetActive(true);

        // Shows the name of the item briefly
        GameObject toolText = inventoryPanel.transform.GetChild(index).GetChild(2).gameObject;
        toolText.SetActive(true);
        // Sets the text to the name of the item
        toolText.transform.GetChild(0).GetComponent<TMP_Text>().text = item;

        // Resets the color to original opacity
        // Since it cant modify the color value directly, a temp variable is made
        var tempColor = toolText.GetComponent<Image>().color;
        tempColor.a = .8f;
        toolText.GetComponent<Image>().color = tempColor;

        tempColor = toolText.transform.GetChild(0).GetComponent<TMP_Text>().color;
        tempColor.a = 1f;
        toolText.transform.GetChild(0).GetComponent<TMP_Text>().color = tempColor;

        StartCoroutine(FadeText(toolText));

    }
    // Hides the border around all inventory slots
    private void UnHighlightAllSlots(){
        StopAllCoroutines();
        for (int i = 0; i < Inventory.Items.Keys.Count; i++){
            // Hides the border
            inventoryPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            // Hides the tool name
            inventoryPanel.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
        }
    }

    // Makes the tool text fade away
    public IEnumerator FadeText(GameObject toolText){
        // Waits 1 second before fading
        yield return new WaitForSeconds(1f);
        // Fade needs to be applied to the shade and text seperately
        for (float a = 0f; a <= 1f;  a += 0.05f){
            var tempColor = toolText.GetComponent<Image>().color;
            tempColor.a -= a;
            toolText.GetComponent<Image>().color = tempColor;

            tempColor = toolText.transform.GetChild(0).GetComponent<TMP_Text>().color;
            tempColor.a -= a;
            toolText.transform.GetChild(0).GetComponent<TMP_Text>().color = tempColor;

            yield return new WaitForSeconds(.1f);
        }
        
    }

    // Adds the object to the player for them to access
    public bool AddToPlayer(GameObject gameObject){
        // Removes the "(Clone)" from the name
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        foreach (Transform child in player.transform.GetChild(0)){
            // If the object is already with the playerInventory, it shouldn't be added
            if(child.name == gameObject.name){
                return false;
            }
        }
        // The 0th child of player is the game object where all the inventory items are stored
        GameObject tool = Instantiate(gameObject, player.transform.GetChild(0));
        tool.name = tool.name.Replace("(Clone)", "");
        // Hides object
        tool.GetComponent<SpriteRenderer>().enabled = false;
        if (tool.CompareTag("Weapon")){
            tool.GetComponent<Weapon>().beingUsed = false;
        }
        // Ensures that it doesn't act like a drop anymore
        if (tool.CompareTag("Drop")){
            tool.tag = "Untagged";
            // Changes sorting layer so it appears above the actors
            tool.GetComponent<Renderer>().sortingLayerName = "Tool";
            // The Glow and Shadow goes away
            foreach (Transform child in tool.transform){
                Destroy(child.gameObject);
            }

        }
        return true;
    }

}
