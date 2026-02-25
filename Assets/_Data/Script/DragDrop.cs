using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    public int amountCurrent;



    private void Awake()
    {

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = ReferenceManager.Instance.canvas;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {

        canvasGroup.alpha = .6f;
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(canvas.transform.root);
        itemBeingDragged = gameObject;
        TakeAmount(startParent);

    }

    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }



    public void OnEndDrag(PointerEventData eventData)
    {

        itemBeingDragged = null;

        if (transform.parent == startParent || transform.parent == transform.root)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
            SetAmount(startParent);

        }
        else
        {
           
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
    void TakeAmount(Transform startParent)
    {
        if (GetComponent<InventoryItem>().stackable)
        {
            amountCurrent = startParent.GetComponent<ItemSlot>().amount;
            startParent.GetComponent<ItemSlot>().amount = 0;
        }
    }
    public void SetAmount(Transform newParent)
    {
        if (GetComponent<InventoryItem>().stackable)
        {
            newParent.GetComponent<ItemSlot>().amount = amountCurrent;
        }
    }
    


}