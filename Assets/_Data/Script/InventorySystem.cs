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
    public GameObject craftingScreen, toolCategoryScreen, contructCategoryScreen;
    public GameObject inforItemPopup;
    public GameObject ThrowItemAreaUI;
    public GameObject itemInforUI;

    public bool isOpenInventory;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
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
            ReferenceManager.Instance.craftingSystem.RefreshNeededItems();
            isOpenInventory = !isOpenInventory;
            inventoryScreenUI.SetActive(isOpenInventory);
            ThrowItemAreaUI.SetActive(isOpenInventory);
            craftingScreen.SetActive(inventoryScreenUI.activeSelf);
            if (!inventoryScreenUI.activeSelf)
            {
                toolCategoryScreen.SetActive(false);
                contructCategoryScreen.SetActive(false);

            }
            if (inventoryScreenUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
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
    }
    public void AddToInventory(string itemName)
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
            Debug.Log(itemToAdd.name);
            OpenPopupItem(itemName, itemToAdd);
          
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

        Debug.Log("remove item");
        int amount = 0;
        foreach (GameObject slot in slotList)
        {

            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == nameToRemove)
            {
                amount++;
                DestroyImmediate(slot.transform.GetChild(0).gameObject);
                if (amount == amountToRemove)
                {

                    return;
                }
            }

        }

    }
    public void ReCalculateList()
    {
        itemList.Clear();
        foreach (GameObject item in slotList)
        {
            if (item.transform.childCount > 0)
            {
                itemList.Add(item.transform.GetChild(0).name);
              
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
}
