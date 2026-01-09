using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;
    public GameObject constructScreenUI;
    public Transform menuButtons;

    //IN Menu Buttons
    Button toolsBTN;
    Button constructBTN;
    //Craft Buttons
    Button craftAxeBTN;
    Button craftPlankBTN;
    Button craftWoodenWallBTN;
    Button craftWoodenFloorBTN;
    Button craftWoodenChestBTN;
    //Requirement Text
    Text AxeReq1, AxeReq2;
    Text plankReq;
    Text woodenWallReq;
    Text woodenFloorReq;
    Text woodenChestReq1, woodenChestReq2;


    public bool isOpen;

    //All Blueprints
    private Blueprint AxeBLP = new Blueprint("Axe",1, 2, "Stone", 3, "Stick", 3);
    private Blueprint PlankBLP = new Blueprint("Plank",2, 1, "Log", 1, "", 0);
    private Blueprint WoodenWallBLP = new Blueprint("Wooden wall", 1, 1, "Plank", 2, "", 0);
    private Blueprint WoodenFloorBLP = new Blueprint("Wooden floor", 1, 1, "Plank", 2, "", 0);
    private Blueprint WoodenChestBLP = new Blueprint("Wooden chest", 1, 2, "Plank", 1, "Log", 1);



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
            craftingScreenUI = ReferenceManager.Instance.canvas.transform.Find("CraftingScreen").gameObject;
            toolsScreenUI = ReferenceManager.Instance.canvas.transform.Find("CategoryScreen").Find("ToolCategoryScreen").gameObject;
            constructScreenUI = ReferenceManager.Instance.canvas.transform.Find("CategoryScreen").Find("ContructCategoryScreen").gameObject;
        }
        menuButtons = craftingScreenUI.transform.Find("MenuButton").transform;

        // ++++++Menu+++++++++

        toolsBTN = menuButtons.Find("ButtonTools").Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        constructBTN = menuButtons.Find("ButtonConstruct").Find("ConstructButton").GetComponent<Button>();
        constructBTN.onClick.AddListener(delegate { OpenConstructCategory(); });

        // ++++++ToolCategoryScreen+++++++++
        // AXE
        AxeReq1 = toolsScreenUI.transform.Find("Axe").Find("AxeCraft").Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").Find("AxeCraft").Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").Find("AxeCraft").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        // Chest
        woodenChestReq1 = toolsScreenUI.transform.Find("Chest").Find("ChestCraft").Find("req1").GetComponent<Text>();
        woodenChestReq2 = toolsScreenUI.transform.Find("Chest").Find("ChestCraft").Find("req2").GetComponent<Text>();

        craftWoodenChestBTN = toolsScreenUI.transform.Find("Chest").Find("ChestCraft").transform.Find("CraftButton").GetComponent<Button>();
        craftWoodenChestBTN.onClick.AddListener(()=> { CraftAnyItem(WoodenChestBLP); });

        // +++++++ContructCategoryScreen++++
        //Plank
        plankReq = constructScreenUI.transform.Find("Plank").Find("PlankCraft").Find("req1").GetComponent<Text>();

        craftPlankBTN = constructScreenUI.transform.Find("Plank").Find("PlankCraft").transform.Find("CraftButton").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });

        //Wooden Wall
        woodenWallReq = constructScreenUI.transform.Find("WoodenWall").Find("WoodenWallCraft").Find("req1").GetComponent<Text>();

        craftWoodenWallBTN = constructScreenUI.transform.Find("WoodenWall").Find("WoodenWallCraft").transform.Find("CraftButton").GetComponent<Button>();
        craftWoodenWallBTN.onClick.AddListener(delegate { CraftAnyItem(WoodenWallBLP); });

        //Wooden Floor
        woodenFloorReq = constructScreenUI.transform.Find("WoodenFloor").Find("WoodenFloorCraft").Find("req1").GetComponent<Text>();

        craftWoodenFloorBTN = constructScreenUI.transform.Find("WoodenFloor").Find("WoodenFloorCraft").transform.Find("CraftButton").GetComponent<Button>();
        craftWoodenFloorBTN.onClick.AddListener(delegate { CraftAnyItem(WoodenFloorBLP); });
    }


    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }
    void OpenConstructCategory()
    {
        craftingScreenUI.SetActive(false);
        constructScreenUI.SetActive(true);
    }


    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        for(int i = 0; i < blueprintToCraft.amountCraftItem; i++)
        {
            InventorySystem.Instance.AddItemToInventoryAndPopup((blueprintToCraft.itemName),true);
        }
      

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

    }
  
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int logWood_count = 0;
        int Plank_count = 0;
        InventorySystem.Instance.ReCalculateList();
        foreach (string itemName in InventorySystem.Instance.itemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
                case "Plank":
                    Plank_count += 1;
                    break;
                case "Log":
                    logWood_count += 1;
                    break;
                    
            }
            //if (itemName == "Stone") { stone_count += 1; }
            //if (itemName == "Stick") { stick_count += 1; }
            //if (itemName == "Log") { logWood_count += 1; }
        }

        //   wooden wall and floor
         woodenWallReq.text = Plank_count + "/2 Plank";
        woodenFloorReq.text = woodenWallReq.text;
        if (Plank_count >= 2)
        {
            craftWoodenWallBTN.gameObject.SetActive(true);
             craftWoodenFloorBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWoodenWallBTN.gameObject.SetActive(false);
            craftWoodenFloorBTN.gameObject.SetActive(false);
        }
        //   Plank
        plankReq.text = logWood_count + "/1 Log";

        if (logWood_count >= 1)
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
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
        }//------Chest-----//

        woodenChestReq1.text = Plank_count + "/1  Plank";
        woodenChestReq2.text = logWood_count + "/1 Log";

        if (Plank_count >= 1 && logWood_count >= 1)
        {
            craftWoodenChestBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWoodenChestBTN.gameObject.SetActive(false);
        }
    }
}
