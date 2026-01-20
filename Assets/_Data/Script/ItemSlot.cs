using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemSlot : MonoBehaviour, IDropHandler
{
    public int amount = 0;
    public bool isNormalSlot = true;
    public int numberSlot;
    TextMeshProUGUI textAmount;
    private void OnEnable()
    {
        if (!isNormalSlot) return;
        textAmount = transform.parent.Find("AmountItem").GetChild(numberSlot).GetComponent<TextMeshProUGUI>();


    }
    private void FixedUpdate()
    {
        if (!isNormalSlot) return;
        if (amount > 1)
        {
            textAmount.text = amount.ToString();
        }
        else if(amount <=1) 
        {
            textAmount.text = "";
        }
    }

    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {

        //if there is not item already then set our item.
        if (!Item && DragDrop.itemBeingDragged!= null)
        { 
                DragDrop.itemBeingDragged.transform.SetParent(transform);
                DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);
                print(Item.name);
        }
    }
    




}