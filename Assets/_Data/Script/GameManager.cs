using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    
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
    public bool UnableOpenInventory()
    {
        if (
            InventorySystem.Instance.isOpeningChest ||
            InventorySystem.Instance.isOpeningShop ||
            DialogSystem.Instance.thePlayerTalking
            )
            return true;
        else return false;
    }


    public bool UnableLookAround()
    {
        if(InventorySystem.Instance.isOpenInventory ||
            InventorySystem.Instance.isOpeningChest||
            InventorySystem.Instance.isOpeningShop||
            DialogSystem.Instance.thePlayerTalking
            )
            return true;
        else return false;
    }
    private void Update()
    {
        if (UnableLookAround()&& Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible =true;
        }
        else if(!UnableLookAround() && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }
    public bool UnableMovement()
    {
        if (
            InventorySystem.Instance.isOpeningChest ||
            InventorySystem.Instance.isOpeningShop ||
            DialogSystem.Instance.thePlayerTalking
            )
            return true;
        else return false;
    }
}
