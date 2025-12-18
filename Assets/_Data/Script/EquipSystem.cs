using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel, numbersHolder;
    public Transform handHoldItem;
    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemListQuickSlot = new List<string>();
    int selectedNumber = -1;
    public GameObject selectedItem;
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


    private void Start()
    {
        PopulateSlotList();
    }
    private void Update()
    {
        ChoseSlot();

    }
    void ChoseSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        //check DropItem
        if (selectedNumber != -1&& handHoldItem.childCount != quickSlotsList[selectedNumber -1].transform.childCount)
        {
            SetUnEquippedModel();
            foreach (Transform child in numbersHolder.transform)
            {
                child.transform.Find("number").GetComponent<Text>().color = Color.gray;
                selectedNumber = -1;
            }
        }

    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        //string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemListQuickSlot.Add(itemToEquip.name);

        ReferenceManager.Instance.inventorySystem.ReCalculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == quickSlotsList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number) == true)
        {
            SetUnEquippedModel();
            if (selectedNumber != number)
            {
                selectedNumber = number;
                // Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                }
                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;
                SetEquippedModel(selectedItem.name);
                // Changing the color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("number").GetComponent<Text>().color = Color.gray;
                }
                Text toBeChanged = numbersHolder.transform.Find("ImageSlot" + number).transform.Find("number").GetComponent<Text>();
                toBeChanged.color = Color.white;
            }
            else
            {
                selectedNumber = -1;
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("number").GetComponent<Text>().color = Color.gray;
                }
            } // We are trying to select the same slot
           
        }

        

    }
    void SetEquippedModel(string itemName)
    {
       
        GameObject item = Instantiate(Resources.Load<GameObject>("Item_on_hand/" + itemName), handHoldItem);
        item.transform.position = handHoldItem.transform.position;
        item.transform.Rotate(0f, -10f, -20f);
        item.name = itemName;

    }
    void SetUnEquippedModel()
    {
        if(handHoldItem.childCount==0) return;
        GameObject item = handHoldItem.GetChild(0).gameObject;
        Destroy(item);
    }
    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber-1].transform.GetChild(0).gameObject;
    }
    bool checkIfSlotIsFull(int slotNumber)
    {
        GameObject slot = quickSlotsList[slotNumber - 1];
        if(slot.transform.childCount >0)
            return true;
        else
            return false;
    } 
}

