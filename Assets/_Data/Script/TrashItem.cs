using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] Transform posReturnItem;
    GameObject draggedItem
    {
        get
        {
            return DragDrop.itemBeingDragged;
        }
    }

    GameObject itemToBeDeleted;
    void Start()
    {
        EffectTrash(0.3f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            EffectTrash(1f);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EffectTrash(0.3f);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    void EffectTrash(float aphal)
    {
        this.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, aphal);

    }

    private void Update()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        //itemToBeDeleted = DragDrop.itemBeingDragged.gameObject;
        if (draggedItem.GetComponent<InventoryItem>().cannotTrash == false)
        {

            itemToBeDeleted = draggedItem;
            EquipSystem.Instance.SetUnEquippedModel();
            EquipSystem.Instance.selectedNumber = -1;
            DeleteItemInventory();
        }

    }
    private void DeleteItemInventory()
    {
        ReturItemToWorld(itemToBeDeleted.name);
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.ReCalculateList();
        //CraftingSystem.Instance.RefreshNeededItems();
    }
    private void ReturItemToWorld(string itemName)
    {
        GameObject item;
        item = Instantiate(Resources.Load<GameObject>("Item_obj/" + itemName));
        item.transform.position = posReturnItem.position;  

    }


}
