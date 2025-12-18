using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public GameObject interaction_Info_UI;
    public Crosshair crosshair;
    Text interaction_text;
    [SerializeField] LayerMask layerMask;

    private void Start()
    {
        
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    void Update()
    {
        RayCastCheck();
    }
    void RayCastCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f, layerMask))
        {
            var selectionTransform = hit.transform;
            //Debug.DrawLine(ray.origin, hit.point, Color.green);
            ActionPickUp(selectionTransform);

            crosshair.SwitchCrosshair(selectionTransform);

            if (selectionTransform.GetComponent<InteractableObject>())
            {
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
                //Debug.DrawRay(ray.origin, hit.transform.position, Color.green);
            }
            else
            {

                interaction_Info_UI.SetActive(false);
            }

        }
        else
        {

            interaction_Info_UI.SetActive(false);
        }
    }
    void ActionPickUp(Transform pickup)
    {
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            pickup.GetComponent<PickUp>()?.PickUpItem();
        }
    }
}