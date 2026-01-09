using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
   public static ReferenceManager Instance { get; set; }
   public SelectionManager selectionManager;
    public InventorySystem inventorySystem;
    public PlayerState playerState;
    public Canvas canvas;
    public Transform player, enviroment;
    public CraftingSystem craftingSystem;
    public GameObject eventSystem;
    private void Awake()
    {
        if(Instance != null && Instance!=this)  Destroy(gameObject);
        else Instance = this;
        CheckNullReference();
        eventSystem = this.transform.Find("EventSystem").gameObject;
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
