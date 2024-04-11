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

public class Inventory : MonoBehaviour
{

    public Button inventoryButton;
    public List<Sprite> inventoryImages;
    // Indicates whether closed or open
    private int imgIdx = 0;

    [SerializeField] Player player; // Should player be the GameObject or the script?

    // Weapons & other Collectables
    public GameObject inventoryPanel;
    // Mission items
    public GameObject sideInventoryPanel;

    // From the Side Inventory Panel
    private List<Image> missionItemImages = new();
    private List<Text> amountTexts = new();
    private List<Image> colorBackgrounds = new();

    // Briefly shows to indicate pickup
    [SerializeField] private GameObject eBtn;

    public bool missionItemsCollected;
    public GameObject missionText;

    public int coins;
    // List of weapons for the current inventory
    public List<GameObject> weapons;

    // Mission Items for the level
    public List<string> missionItems;
    public List<Sprite> missionImages;
    public List<int> numberOfEachMissionItem;
    public List<int> numberOfEachMissionItemCollected;
    public List<GameObject> missionItemDrops;

    private float cooldown;
    private float time;
    private float pickupDistance = 0.2f;

    // Awake is called even before Start
    void Awake(){
        coins = 0;
        // Sets cooldown for picking up items
        cooldown = 0.2f;
        time = Time.time;

    }

    void Start(){
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
        for (int w = 0; w < weapons.Count; w++){
            Sprite image = weapons[w].GetComponent<SpriteRenderer>().sprite;
            // Gets the image slot at the exact position
            Image inventorySlot = inventoryPanel.transform.GetChild(w).transform.GetChild(0).gameObject.GetComponent<Image>();
            // Activates the image with visible properties
            inventorySlot.sprite = image;
            inventorySlot.color = new Color(255, 255, 255, 1);
            inventorySlot.preserveAspect = true;
        }

        // For the next level
        LevelManager.instance.levelComplete = false;
        // Now that the objects are found, the values can be set
        Reset();
        SetMissionItems();
    }

    // Update is called once per frame
    void Update(){
        if (!LevelManager.instance.isPaused && LevelManager.instance.isFunctional){
            // Inventory Key
            if (Input.GetKeyDown(KeyCode.Q)) {
                ToggleInventory();
            }
            // Checks for E or Right Mouse and if so, looks for items in the area to be picked up. 
            // Runs collectitem to collect item
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButton(1)) && Time.time - time >= cooldown){
                time = Time.time;
                // Finds everything that it collided within the pickup distance
                Collider2D[] coll = Physics2D.OverlapCircleAll(player.transform.position, pickupDistance);
                if (coll != null){
                    for (int i = 0; i< coll.Length; i++){
                        if (coll[i].tag == "Drop" && coll[i] != null){
                            // Collects and destroys the drop
                            CollectItem(coll[i].name);
                            coll[i].GetComponent<SpriteRenderer>().enabled = false;
                            Destroy(coll[i].GameObject());
                            // Only one item picked up at once
                            break;
                        }
                    }
                }
            }
            // Flashes the e button if nearby a collectable
            Collider2D[] nearby = Physics2D.OverlapCircleAll(player.transform.position, pickupDistance);
            if (nearby != null){
                for (int i = 0; i < nearby.Length; i++){
                    if (nearby[i].tag == "Drop" && nearby[i] != null){
                        eBtn.SetActive(true);
                        break;
                    }
                    else{
                        eBtn.SetActive(false);
                    }
                }
            }
        }

    }

    // Everything is reset in terms of what is displayed on the inventory on the screen and replaces them to their defaults 
    public void Reset(){
        for (int i = 0; i < amountTexts.Count; i++){
            amountTexts[i].text = "0/" + numberOfEachMissionItem[i] + " ";
            amountTexts[i].color = Color.black;
            // White but transparent
            colorBackgrounds[i].color = new Color(1, 1, 1, 0);
        }
    }

    // Sets the values for each mission item on the panel
    public void SetMissionItems(){
        for(int i = 0; i < missionItems.Count; i++){
            missionItemImages[i].sprite = missionImages[i];
            amountTexts[i].text = "0/" + numberOfEachMissionItem[i].ToString();
        }
    }

    // Adds one to the mission item counter with reference to the exact object through the parameter id
    public void AddOneToItemCount(int id){
        string text = amountTexts[id].text.ToString();
        string[] nums = text.Split("/");

        nums[0] = Regex.Replace(nums[0], @"\s+", "");
        nums[1] = Regex.Replace(nums[1], @"\s+", "");
        amountTexts[id].text = (int.Parse(nums[0]) + 1).ToString() + "/" + nums[1] + "  ";
        
    }

    // When all quantities of an item is collected, the inventory spaces turn green to show that it is done 
    public void SetAsDone(int id){
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

    // Collects item by making sure the item found around the player is one needed for the mission.
    public void CollectItem(string name){
        for(int i = 0; i < missionItems.Count; i++){
            // Checks if objects match by name
            if(missionItems[i] != null && name.Contains(missionItems[i])){
                numberOfEachMissionItemCollected[i] += 1;
                // Shows on screen
                AddOneToItemCount(i);
                GlobalManager.instance.itemsCollected++;
                // All numbers of that item is collected
                if(numberOfEachMissionItem[i] == numberOfEachMissionItemCollected[i]){
                    SetAsDone(i);
                }
            }
        }
        // Checks if all items are collected
        CheckIfDone();
    }

    // Checks if all mission items are collected by iterating through each mission item. 
    public void CheckIfDone(){
        for(int i = 0; i < missionItems.Count; i++){
            if(numberOfEachMissionItemCollected[i] < numberOfEachMissionItem[i]){
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
        Invoke("turnOffMissionHeader", 7);
    }

    // Allows level to be finished in Inspector
    [ContextMenu("Auto-Complete Level")]
    public void FinishLevel(){
        for (int i = 0; i < missionItems.Count; i++){
            SetAsDone(i);
        }
        SetDone();
    }

    // Adds weapon to list UNUSED
    public void CollectWeapon(GameObject weapon){
        weapons.Add(weapon);
    }

    // Adds coins UNUSED
    public void AddCoins(int coinsAmount){
        coins += coinsAmount;
    }

    // Finds weapon by name UNUSED
    private bool LookUpWeapon(string name){
        bool found = false;
        for(int i = 0; i < weapons.Count; i++){
            if(weapons[i].name == name){
                found = true;
                return found;
            }
        }
        return found;
    }
    
    // UNUSED
    private void UpdateMissionItemProgress(){
        
    }

    // UNUSED
    private void UpdateWeapons(string weapon){
        
    }

    // Gets weapon list 
    public List<GameObject> GetWeaponList(){
        return weapons;
    }

    // Turns off the mission header when needed. 
    public void TurnOffMissionHeader(){
        missionText.SetActive(false);
    }

    // Shows the border around the currently used weapon slot
    public void HighlightSlot(int index){
        string weapon = weapons[index].name;
        UnHighlightAllSlots();
        GameObject border = inventoryPanel.transform.GetChild(index).GetChild(1).gameObject;
        border.SetActive(true);

        // Shows the name of the weapon briefly
        GameObject toolText = inventoryPanel.transform.GetChild(index).GetChild(2).gameObject;
        toolText.SetActive(true);
        // Sets the text to the name of the weapon
        toolText.transform.GetChild(0).GetComponent<TMP_Text>().text = weapon;

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
        for (int i = 0; i < weapons.Count; i++){
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

}
