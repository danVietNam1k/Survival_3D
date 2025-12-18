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
    private void Awake()
    {
        if(Instance != null && Instance!=this)  Destroy(gameObject);
        else Instance = this;

        selectionManager = this.transform.Find("SelectionManager").GetComponent<SelectionManager>();
        inventorySystem = this.transform.Find("InventorySystem").GetComponent<InventorySystem>();
        playerState = this.transform.Find("PlayerState").GetComponent<PlayerState>();
    }
}
