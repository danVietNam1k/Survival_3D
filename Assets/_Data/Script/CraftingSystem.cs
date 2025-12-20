using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

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
    //Requirement Text
    Text AxeReq1, AxeReq2;
    Text plankReq;

    public bool isOpen;

    //All Blueprints
    private Blueprint AxeBLP = new Blueprint("Axe",1, 2, "Stone", 3, "Stick", 3);
    private Blueprint PlankBLP = new Blueprint("Plank",2, 1, "Log", 1, "", 0);

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
            toolsScreenUI = ReferenceManager.Instance.canvas.Find("CategoryScreen").Find("ToolCategoryScreen").gameObject;
            constructScreenUI = ReferenceManager.Instance.canvas.Find("CategoryScreen").Find("ContructCategoryScreen").gameObject;
        }
        menuButtons = craftingScreenUI.transform.Find("MenuButton").transform;


        toolsBTN = menuButtons.Find("ButtonTools").Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        constructBTN = menuButtons.Find("ButtonConstruct").Find("ConstructButton").GetComponent<Button>();
        constructBTN.onClick.AddListener(delegate { OpenConstructCategory(); });
        // AXE
        AxeReq1 = toolsScreenUI.transform.Find("Button").Find("AxeCraft").Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Button").Find("AxeCraft").Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Button").Find("AxeCraft").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        //Plank
        plankReq = constructScreenUI.transform.Find("Button").Find("PlankCraft").Find("req1").GetComponent<Text>();

        craftPlankBTN = constructScreenUI.transform.Find("Button").Find("PlankCraft").transform.Find("CraftButton").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });
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
            InventorySystem.Instance.AddToInventory((blueprintToCraft.itemName));
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

      RefreshNeededItems();
    }
  
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int logWood_count = 0;
        InventorySystem.Instance.ReCalculateList();
        foreach (string itemName in InventorySystem.Instance.itemList)
        {
            //switch (itemName)
            //{
            //    case "Stone":
            //        stone_count += 1;
            //        break;
            //    case "Stick":
            //        stick_count += 1;
            //        break;
            //}
            if(itemName == "Stone") { stone_count += 1; }
            if (itemName == "Stick") { stick_count += 1; }
            if (itemName == "Log") { logWood_count += 1; }
        }
        //   Plank
        plankReq.text = logWood_count + "/1 Log Wood";

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
        }
    }
}
