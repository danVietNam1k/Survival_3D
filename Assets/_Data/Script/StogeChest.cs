using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageChest : MonoBehaviour
{
    public List<GameObject> slotsChestList = new List<GameObject>();
    public List<string> itemChestList = new List<string>();
    public GameObject chestStorage, inventory;

    void Start()
    {
        inventory = InventorySystem.Instance.inventoryScreenUI;
        AddSlotsList();
        RefreshItemInChest();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void OpenChest()
    {
        chestStorage.SetActive(true);
        inventory.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InventorySystem.Instance.isOpenInventory = true;

    }
    public void RefreshItemInChest()
    {
        itemChestList.Clear();
        foreach (GameObject slot in slotsChestList)
        {
            if (slot.transform.childCount > 0)
            {
               string name = slot.transform.GetChild(0).name;
                itemChestList.Add(name);
            }
        }
    }
    public void CloseChest() 
        {
            chestStorage.SetActive(false);
        inventory.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InventorySystem.Instance.isOpenInventory = false;


    }
    void AddSlotsList()
    {
        foreach (Transform slot in chestStorage.transform)
        {
            slotsChestList.Add(slot.gameObject);
        }

    }

}
