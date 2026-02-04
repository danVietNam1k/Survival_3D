using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    static SelectionManager instance;
    public static SelectionManager Instance => instance;
    public GameObject interaction_InfoName_UI;
    public GameObject interaction_Info_Hp;
    public Crosshair crosshair;
    Text interaction_text;
    Slider interaction_Hp;
    [SerializeField] LayerMask layerMask;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

        interaction_text = interaction_InfoName_UI.GetComponent<Text>();
        interaction_Hp = interaction_Info_Hp.GetComponent<Slider>();
    }

    void Update()
    {
    }
    private void LateUpdate()
    {
        RayCastCheck();

    }
    void RayCastCheck()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHitOntrigger(ray);
        RaycastHit(ray);
    }
    private void RaycastHit(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f, layerMask))
        {
            var selectionTransform = hit.transform;
           
            InteractiveActions(selectionTransform);

            crosshair.SwitchCrosshair(selectionTransform);
            GetInforItem(selectionTransform);
        }
        else
        {
            interaction_InfoName_UI.SetActive(false);
            interaction_Info_Hp.SetActive(false);
            crosshair.SwitchCrosshair(null);

        }
    }
    private Transform RaycastHitOntrigger(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f, layerMask, QueryTriggerInteraction.Collide))
        {
            return hit.transform;

        }
        else
        {
            return null;
        }
    }
    void GetInforItem(Transform hit) {
        if (hit.GetComponent<InteractableObject>())
        {
            interaction_text.text = hit.GetComponent<InteractableObject>().GetItemName();
            interaction_InfoName_UI.SetActive(true);
            //Debug.DrawRay(ray.origin, hit.transform.position, Color.green);
            ShowHp(hit);
        }
        else
        {

            interaction_InfoName_UI.SetActive(false);
            interaction_Info_Hp.SetActive(false);

        }

    }
    void InteractiveActions(Transform hit)
    {
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            print(hit.tag);
            switch (hit.tag){ 
                case NameStatic.TagNPC:

                    hit.GetComponent<NPC>()?.StartConversation();

                    break;
                case NameStatic.TagCanPickUp:
                    hit.GetComponent<PickUp>()?.PickUpItem();
                    break;
                case NameStatic.ChestStorage:
                    hit.GetComponent<StorageChest>()?.OpenChest();
                    break;
                case "Horse":
                    Transform player = ReferenceManager.Instance.player;
                    hit.GetComponent<HorseController>()?.Mount(player);
                    break;

            }
        }
    }
    void ShowHp(Transform hit)
    {
        if(hit.GetComponent<InteractableObject>().CanHit ==false)return;
        interaction_Hp.value = hit.GetComponent<InteractableObject>().GetHpInfor();
        interaction_Info_Hp.SetActive(true);
    }
}