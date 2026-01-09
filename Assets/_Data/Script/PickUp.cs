using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PickUpItem()
    {
        string itemName = this.GetComponent<InteractableObject>().ItemName;
        InventorySystem.Instance.AddItemToInventoryAndPopup(itemName, true);
        if(!InventorySystem.Instance.isFull)
        Destroy(gameObject);
       
        
    }
   
}
