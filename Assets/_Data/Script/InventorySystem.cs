using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public GameObject quickSlotScreenUI;
    public Transform ItemInforUI;
    
    [Header("----------Category Screen------------")]
    public GameObject MenuScreen;
    public GameObject categoryScreen;
    
   [Header("------------------------------------")]
    public GameObject inforItemPopup;
    public GameObject ThrowItemAreaUI;
    public GameObject itemInforUI;
    private GameObject itemToAdd;
    public bool isFull;
    private GameObject whatSlotToAdd;
    [Header("------------------------------------")]
    public bool isOpeningChest;
    public bool isOpeningShop;
    public int currentMonney = 0;
 

    public bool isOpenInventory;
    [Header("------------------------------------")]
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    public List<GameObject> quickSlotList = new List<GameObject>();
    public List<string> itemInQuickSlotList = new List<string>();
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
    void Start()
    {
        isOpenInventory = inventoryScreenUI.activeSelf;
        PopulateSlotList();
    }
    void Update()
    {
       
        OpenInventory();
        ShowMonney();

    }
    void OpenInventory()
    {
        if (GameManager.Instance.UnableOpenInventory()) return;
        if (Input.GetKeyDown(KeyCode.I))
        {   
            if(DialogSystem.Instance.thePlayerTalking) return;
            //ReferenceManager.Instance.craftingSystem.RefreshNeededItems();
            isOpenInventory = !isOpenInventory;
            inventoryScreenUI.SetActive(isOpenInventory);
            ThrowItemAreaUI.SetActive(isOpenInventory);
            MenuScreen.SetActive(inventoryScreenUI.activeSelf);
            TurnOffInMenu();




            CraftingSystem.Instance.RefreshNeededItems();

        }
      
    }
    void TurnOffInMenu()
    {
        foreach(Transform inMenu in categoryScreen.transform)
        {
            if (inMenu.gameObject.activeSelf)
            {
                inMenu.gameObject.SetActive(false);
            }
        }
    }
    public void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
                if (child.transform.childCount > 0)
                {
                    itemList.Add(child.GetChild(0).gameObject.name);
                }
            }
        }
        foreach (Transform child in quickSlotScreenUI.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotList.Add(child.gameObject);
                if (child.transform.childCount > 0)
                {
                    itemInQuickSlotList.Add(child.GetChild(0).gameObject.name);
                }
            }
        }
    }
    public void AddItemToInventoryAndPopup(string itemName, bool popupTurnOn)
    {
        
        if (CheckInventoryIsFull())
        {
            Debug.Log("Inventory Is Full");
        }
        else
        {
           
            itemToAdd = Instantiate(Resources.Load<GameObject>("Item_Inventory/"+itemName));

            if(popupTurnOn) OpenPopupItem(itemName, itemToAdd);
            print("add item");
            if (ItemStackable(itemToAdd)) {
                AddItemStackable(itemToAdd,itemName);
                
            }
            else
            {
                AddItemNormal(itemToAdd, itemName);
            }
        }
    }
    void AddItemNormal(GameObject itemToAdd,string itemName)
    {
        whatSlotToAdd = FindNextEptySlot();
        itemToAdd.transform.SetParent(whatSlotToAdd.transform);
        itemToAdd.name = itemName;
        itemToAdd.transform.localPosition = Vector3.zero;
        whatSlotToAdd.GetComponent<ItemSlot>().amount = 1;
        CraftingSystem.Instance.RefreshNeededItems();
        QuestManager.Instance.RefreshTrackerAmountItem();
    }
    void AddItemStackable(GameObject itemToAdd, string itemName)
    {
        CraftingSystem.Instance.RefreshNeededItems();
        foreach (string item in itemList)
        {
            if (item == itemName)
            {
                foreach(GameObject slot in slotList)
                {
                    if(slot.transform.childCount >0 && itemName == slot.transform.GetChild(0).name)
                    {
                        slot.GetComponent<ItemSlot>().amount++;
                        Destroy(itemToAdd);
                        return;
                    }
                }
            }
        }
        AddItemNormal(itemToAdd,itemName);
    }
    bool ItemStackable(GameObject itemToAdd)
    {
        return itemToAdd.GetComponent<InventoryItem>().stackable;
    }
    public GameObject FindNextEptySlot()
    {
        
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();

    }
    public bool CheckInventoryIsFull()
    {
        int counter = 0;
        foreach (GameObject slot in slotList) {
            if (slot.transform.childCount > 0)
            {
                counter++;
            } }

        if (counter == slotList.Count) {
            
                isFull = true;
            }
            else isFull = false;
        return isFull;
    }
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int amount = 0;
        foreach (GameObject slot in slotList)
        {

            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == nameToRemove)
            {
                amount++;

                GameObject item = slot.transform.GetChild(0).gameObject;
                if (ItemStackable(item))
                {
                    
                    slot.GetComponent<ItemSlot>().amount--;
                    print("use item ");
                    if(slot.GetComponent<ItemSlot>().amount < 1)
                    {
                        slot.GetComponent<ItemSlot>().amount = 0;
                        DestroyImmediate(item);
                    }

                }
                else
                {
                    slot.GetComponent<ItemSlot>().amount = 0;
                    DestroyImmediate(item);
                }
                
                if (amount == amountToRemove)
                {
                    CraftingSystem.Instance.RefreshNeededItems();
                    QuestManager.Instance.RefreshTrackerAmountItem();

                    return;
                }

            }

        }
        CraftingSystem.Instance.RefreshNeededItems();

        QuestManager.Instance.RefreshTrackerAmountItem();


    }
    public void ReCalculateList()
    {
        itemList.Clear();
        itemInQuickSlotList.Clear();
        foreach (GameObject item in slotList)
        {
            if (item.transform.childCount > 0)
            {
                itemList.Add(item.transform.GetChild(0).name);
              
            }

        }
        foreach (GameObject item in quickSlotList)
        {
            if (item.transform.childCount > 0)
            {
                itemInQuickSlotList.Add(item.transform.GetChild(0).name);

            }

        }
    }
   
    
    void OpenPopupItem(string itemName, GameObject item)
    {
        inforItemPopup.SetActive(false);

        inforItemPopup.SetActive(true);
        Sprite itemImgae = item.GetComponent<Image>().sprite;
        Transform popup = inforItemPopup.transform.Find("Popup");
        popup.gameObject.SetActive(true);
        popup.Find("Image").GetComponent<Image>().sprite = itemImgae;
        popup.Find("Text").GetComponent<Text>().text = itemName;

    }
    public GameObject QuickSlotEmpty()
    {
        foreach (GameObject slot in quickSlotList)
        {
            if (slot.transform.childCount == 0)
            { return slot; }
        }
        return null;
    }
    public void AddItemtoQuickSlot(string itemName)
    {
        GameObject SlotToEquip = QuickSlotEmpty();
        GameObject itemToAddInQuickSlot = Instantiate(Resources.Load<GameObject>("Item_Inventory/" + itemName), SlotToEquip.transform);
        itemToAddInQuickSlot.transform.SetParent(SlotToEquip.transform);
        itemToAddInQuickSlot.name = itemName;
        CraftingSystem.Instance.RefreshNeededItems();
        QuestManager.Instance.RefreshTrackerAmountItem();
    }
    private void ShowMonney()
    {
        if (isOpenInventory || isOpeningShop)
        {
            TextMeshProUGUI txtMoney =ThrowItemAreaUI.transform.Find("Text Gold").GetComponent<TextMeshProUGUI>();
            txtMoney.text = currentMonney + " Bitcoin";
        }
    }
}
