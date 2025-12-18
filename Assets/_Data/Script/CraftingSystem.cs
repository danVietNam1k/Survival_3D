using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsBTN;

    //Craft Buttons
    Button craftAxeBTN;

    //Requirement Text
    Text AxeReq1, AxeReq2;

    public bool isOpen;

    //All Blueprints
    private Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);


    public static CraftingSystem Instance { get; set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if(craftingScreenUI == null || toolsScreenUI == null)
        {
            craftingScreenUI = ReferenceManager.Instance.canvas.Find("CraftingScreen").gameObject;
            toolsScreenUI = ReferenceManager.Instance.canvas.Find("ToolCategoryScreen ").gameObject;
        }
        toolsBTN = craftingScreenUI.transform.Find("Button").Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        // AXE
        AxeReq1 = toolsScreenUI.transform.Find("Button").Find("AxeCraft").Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Button").Find("AxeCraft").Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Button").Find("AxeCraft").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

    }


    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }


    void CraftAnyItem(Blueprint blueprintToCraft)
    {

        InventorySystem.Instance.AddToInventory((blueprintToCraft.itemName));

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }
        InventorySystem.Instance.ReCalculateList();

        RefreshNeededItems();
    }
  
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }
        //------Axe-----//
        AxeReq1.text = stone_count+ "/3 Stones";
        AxeReq2.text = stick_count+ "/3 Sticks";

        if (stone_count >= 3 && stick_count >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
    }
}
