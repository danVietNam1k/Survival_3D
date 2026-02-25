using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel, numbersHolder;
    public Transform handHoldItem;
    public Transform referenceItemHolding = null;
    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemListQuickSlot = new List<string>();
    public int selectedNumber = -1;
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
        for (int i = 1; i <= quickSlotsList.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SelectQuickSlot(i);
                break;
            }
        }
        //check DropItem
    

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
        return null;
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
    public void NotChoseItemInQuickSlot()
    {
        //if(referenceItemHolding == null) selectedNumber = -1;



        foreach (Transform child in numbersHolder.transform)
        {
            child.transform.Find("number").GetComponent<Text>().color = Color.gray;
        }
        return;
    }
    void SelectQuickSlot(int number)
    {
       
        if (checkIfSlotIsFull(number) == true)
        {
            if (ConstructionManager.Instance.isBuilding == true) // check progress construction 
            {
                ConstructionManager.Instance.isBuilding = false;
            }
            SetUnEquippedModel();
            if (selectedNumber != number)
            {
                selectedNumber = number;
                //drop item on quickslot
                // Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                }
                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;
                string itemName = selectedItem.GetComponent<InventoryItem>().thisName;
                SetEquippedModel(itemName, selectedItem);



                // Changing the color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("number").GetComponent<Text>().color = Color.gray;
                    if(child.transform == numbersHolder.transform.GetChild(number - 1))
                    {
                        child.transform.Find("number").GetComponent<Text>().color =Color.white;
                    }
                }
                //Text toBeChanged = numbersHolder.transform.GetChild(number - 1).GetChild(0).GetComponent<Text>();
                //toBeChanged.color = Color.white;
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
        else { }

        

    }
    void SetEquippedModel(string itemName,GameObject itemInQuickSlot)
    {
       
        GameObject item = Instantiate(Resources.Load<GameObject>("Item_on_hand/" + itemName), handHoldItem);
        item.transform.position = handHoldItem.transform.position;
        item.transform.Rotate(0f, -10f, -20f);
        item.name = itemName;
        if (item.GetComponent<TheItemEquipping>() != null)
        item.GetComponent<TheItemEquipping>().thisItemInQuickSlot = itemInQuickSlot;
        referenceItemHolding = item.transform;
        
    }
    public void SetUnEquippedModel()
    {
        if(handHoldItem.childCount==0 || referenceItemHolding == null) return;
        
        Destroy(referenceItemHolding.gameObject);
        referenceItemHolding = null;
        NotChoseItemInQuickSlot();
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

