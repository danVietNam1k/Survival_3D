using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public eCanPickupItemType type;
    public bool thisCanCooking;
    public float countTimeCooking;

    public void PickUpItem()
    {
        string itemName = this.GetComponent<InteractableObject>().ItemName;
        InventorySystem.Instance.AddItemToInventoryAndPopup(itemName, true);
        if(!InventorySystem.Instance.CheckInventoryIsFull())
        Destroy(gameObject);
       
    }
  
   
}
