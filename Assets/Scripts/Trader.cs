using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Trader : MonoBehaviour
{
    private GameObject gm;
    private InventoryManager inventoryManager;
    private UIManager uiManager;
    [SerializeField] private Player player;

    // Distance until the dialogue appears
    [SerializeField] private float activationDistance = 20f;
    // The trade panel for trading
    [SerializeField] private GameObject tradePanel;
    private Button backBtn;
    // Buy/Sell refers to the player not the trader, so the player is buying from the buy panel
    private GameObject sellPanel;
    private GameObject buyPanel;
    private TMP_Text dialogueText;
    private GameObject optionButtons;

    // Briefly shows to indicate pickup
    [SerializeField] private GameObject eBtn;
    // Items to Sell will always be the mission items of the planet
    [HideInInspector] public List<SaleItem> sellItems;
    // Represents the cost
    public List<int> sellCosts;
    // Items to Buy
    public List<SaleItem> buyItems;

    // Start is called before the first frame update
    void Start(){
        gm = GameObject.Find("GameManager");
        inventoryManager = gm.GetComponent<InventoryManager>();
        uiManager = gm.GetComponent<UIManager>();

        // Initializes everything in the trade panel (Make sure all the order is correct!)
        backBtn = tradePanel.transform.GetChild(1).GetComponent<Button>();
        sellPanel = tradePanel.transform.GetChild(3).gameObject;
        buyPanel = tradePanel.transform.GetChild(4).gameObject;
        dialogueText = tradePanel.transform.GetChild(5).GetChild(0).GetComponent<TMP_Text>();
        optionButtons = tradePanel.transform.GetChild(6).gameObject;

        backBtn.onClick.AddListener(ToggleTradePanel);
        // Items to Sell
        // The last child is the label
        for (int c = 0; c < sellPanel.transform.childCount - 1; c++){
            GameObject item = sellPanel.transform.GetChild(c).GetChild(0).gameObject;
            TMP_Text itemQuantity = item.transform.GetChild(0).GetComponent<TMP_Text>();
            Button sellButton = sellPanel.transform.GetChild(c).GetChild(1).GetComponent<Button>();
            TMP_Text costText = sellPanel.transform.GetChild(c).GetChild(2).GetChild(0).GetComponent<TMP_Text>();

            
            // Sets the sell item to the planets mission item
            item.GetComponent<Image>().sprite = inventoryManager.planetMissionItems[c].gameObject.GetComponent<SpriteRenderer>().sprite;
            itemQuantity.text = inventoryManager.planetMissionItems[c].toCollect.ToString();
            sellButton.onClick.AddListener(() => SellItem());
            costText.text = sellCosts[c].ToString();
        }
        // Items to Buy
        for (int c = 0; c < buyPanel.transform.childCount - 1; c++){
            GameObject item = buyPanel.transform.GetChild(c).GetChild(0).gameObject;
            TMP_Text itemQuantity = item.transform.GetChild(0).GetComponent<TMP_Text>();
            Button buyButton = buyPanel.transform.GetChild(c).GetChild(1).GetComponent<Button>();
            TMP_Text costText = buyPanel.transform.GetChild(c).GetChild(2).GetChild(0).GetComponent<TMP_Text>();

            item.GetComponent<Image>().sprite = buyItems[c].gameObject.GetComponent<SpriteRenderer>().sprite;
            itemQuantity.text = buyItems[c].quantity.ToString();
            buyButton.onClick.AddListener(() => BuyItem());
            costText.text = buyItems[c].cost.ToString();
        }
    }

    // Update is called once per frame
    void Update(){
        // Player should be alive first
        if (player){
            // Calculate the distance between player and target object
            float distance = Vector2.Distance(player.GetComponent<Rigidbody2D>().position, gameObject.GetComponent<Rigidbody2D>().position);

            // Check if the player is within the activation distance
            if (distance <= activationDistance){
                // Shows the Dialogue
                transform.GetChild(1).gameObject.SetActive(true);
                eBtn.SetActive(true); 
                if (Input.GetKeyDown(KeyCode.E)){
                    ToggleTradePanel();
                }
            }
            else{
                // Hides the Dialogue
                transform.GetChild(1).gameObject.SetActive(false);
                eBtn.SetActive(false);
            }
        }
    }

    // Shows/Hides the Trade panel
    public void ToggleTradePanel(){
        // Toggles the trade panel
        tradePanel.SetActive(!tradePanel.activeSelf);
        // Stops all time operations
        LevelManager.instance.TogglePause();
    }

    public void BuyItem(){

    }

    public void SellItem(){

    }

}
