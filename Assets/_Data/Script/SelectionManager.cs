using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public GameObject interaction_InfoName_UI;
    public GameObject interaction_Info_Hp;
    public Crosshair crosshair;
    Text interaction_text;
    Slider interaction_Hp;
    [SerializeField] LayerMask layerMask;

    private void Start()
    {
        
        interaction_text = interaction_InfoName_UI.GetComponent<Text>();
        interaction_Hp = interaction_Info_Hp.GetComponent<Slider>();
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
                interaction_InfoName_UI.SetActive(true);
                //Debug.DrawRay(ray.origin, hit.transform.position, Color.green);
                ShowHp(selectionTransform);
            }
            else
            {

                interaction_InfoName_UI.SetActive(false);
                interaction_Info_Hp.SetActive(false);

            }

        }
        else
        {

            interaction_InfoName_UI.SetActive(false);
            interaction_Info_Hp.SetActive(false);

        }
    }
    void ActionPickUp(Transform pickup)
    {
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            pickup.GetComponent<PickUp>()?.PickUpItem();
        }
    }
    void ShowHp(Transform hit)
    {
        if(hit.GetComponent<InteractableObject>().CanHit ==false)return;
        interaction_Hp.value = hit.GetComponent<InteractableObject>().GetHpInfor();
        interaction_Info_Hp.SetActive(true);
    }
}