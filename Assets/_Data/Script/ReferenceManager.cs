using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
   public static ReferenceManager Instance { get; set; }
   public SelectionManager selectionManager;
    public InventorySystem inventorySystem;
    public PlayerState playerState;
    public Transform canvas;
    public Transform player;
    public CraftingSystem craftingSystem;
    private void Awake()
    {
        if(Instance != null && Instance!=this)  Destroy(gameObject);
        else Instance = this;
        CheckNullReference();

    }
    void CheckNullReference()
    {
        if(selectionManager ==null)
            selectionManager = GetComponentInChildren<SelectionManager>();
        if (inventorySystem == null)

            inventorySystem = GetComponentInChildren<InventorySystem>();
        if (playerState == null)

            playerState = GetComponentInChildren<PlayerState>();
        if (craftingSystem == null)

            craftingSystem = GetComponentInChildren<CraftingSystem>();
    }
}
