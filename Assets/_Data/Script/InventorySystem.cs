using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public GameObject quickSlotScreenUI;

    [Header("----------Category Screen------------")]
    public GameObject MenuScreen;
    public GameObject categoryScreen;
    
   [Header("------------------------------------")]
    public GameObject inforItemPopup;
    public GameObject ThrowItemAreaUI;
    public GameObject itemInforUI;

    public bool isOpenInventory;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    public List<GameObject> quickSlotList = new List<GameObject>();
    public List<string> itemInQuickSlotList = new List<string>();

    private GameObject itemToAdd;
    public bool isFull;
    private GameObject whatSlotToEquip;
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

    }
    void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {   
            if(DialogSystem.Instance.thePlayerTalking) return;
            //ReferenceManager.Instance.craftingSystem.RefreshNeededItems();
            isOpenInventory = !isOpenInventory;
            inventoryScreenUI.SetActive(isOpenInventory);
            ThrowItemAreaUI.SetActive(isOpenInventory);
            MenuScreen.SetActive(inventoryScreenUI.activeSelf);

            SetActiveFollowInventory();

            if (inventoryScreenUI.activeSelf == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }
            CraftingSystem.Instance.RefreshNeededItems();

        }
      
    }
    void SetActiveFollowInventory()
    {
            if (!inventoryScreenUI.activeSelf)
            {foreach (Transform child in categoryScreen.transform)
                child.gameObject.SetActive(false);
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
        CheckIsFull();
        if (isFull)
        {
            Debug.Log("Inventory Is Full");
        }
        else
        {
            whatSlotToEquip = FindNextEptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>("Item_Inventory/"+itemName), whatSlotToEquip.transform);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemToAdd.name = itemName;
            if(popupTurnOn) OpenPopupItem(itemName, itemToAdd);
            CraftingSystem.Instance.RefreshNeededItems();
            QuestManager.Instance.RefreshTrackerAmountItem();
        }
    }

    GameObject FindNextEptySlot()
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
    void CheckIsFull()
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
    }
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        //var itemsToRemove = slotList
        //    .Where(s => s.transform.childCount > 0 && s.transform.GetChild(0).name == nameToRemove)
        //    .Take(amountToRemove);

        //foreach (var item in itemsToRemove)
        //{

        //    Destroy(item.transform.GetChild(0).gameObject);

        //}
        //ReCalculateList();

        int amount = 0;
        foreach (GameObject slot in slotList)
        {

            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == nameToRemove)
            {
                amount++;
                DestroyImmediate(slot.transform.GetChild(0).gameObject);
                if (amount == amountToRemove)
                {
                    CraftingSystem.Instance.RefreshNeededItems();

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

}
