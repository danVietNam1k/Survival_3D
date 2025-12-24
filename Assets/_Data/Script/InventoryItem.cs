using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // --- Is this item trashable --- //
    public bool cannotTrash;
    public bool isSelected;
    // --- Item Info UI --- //
    public GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;
    
    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;
    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.itemInforUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionatily").GetComponent<Text>();
   
    }
    void Update()
    {
        itemInfoUI.transform.position = Input.mousePosition;
    }
    // Triggered when the mouse enters into the area of the item that has this script.
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    // Triggered when the mouse exits the area of the item that has this script.
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        //Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isConsumable)
            {
                // Setting this specific gameobject to be the item we want to destroy later

                StartCoroutine(IfDragItem());
                itemPendingConsumption = gameObject;

                IEnumerator IfDragItem()
                {
                    yield return new WaitForSeconds(0.1f);
                    itemPendingConsumption = null;
                }
               
            }
            
        }
    }
   
    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (isConsumable && itemPendingConsumption == gameObject)
            {
                consumingFunction();
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                //CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    public void consumingFunction()
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);

        caloriesEffectCalculation(caloriesEffect);

        hydrationEffectCalculation(hydrationEffect);


    }
   


    private static void healthEffectCalculation(float healthEffect)
    {
        // --- Health --- //

        float healthBeforeConsumption = ReferenceManager.Instance.playerState.currentHeal;

        float maxHealth = ReferenceManager.Instance.playerState.maxHeal;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                ReferenceManager.Instance.playerState.setHealth(maxHealth);
            }
            else
            {
                ReferenceManager.Instance.playerState.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }


    private static void caloriesEffectCalculation(float caloriesEffect)
    {
        // --- Calories --- //

        float caloriesBeforeConsumption = ReferenceManager.Instance.playerState.currentCalories;
        float maxCalories = ReferenceManager.Instance.playerState.maxCalories;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                ReferenceManager.Instance.playerState.setCalories(maxCalories);
            }
            else
            {
                ReferenceManager.Instance.playerState.setCalories(caloriesBeforeConsumption + caloriesEffect);
            }
        }
    }


    private static void hydrationEffectCalculation(float hydrationEffect)
    {
        // --- Hydration --- //

        float hydrationBeforeConsumption = ReferenceManager.Instance.playerState.currentHydration;
        float maxHydration = ReferenceManager.Instance.playerState.maxHydration;

        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                ReferenceManager.Instance.playerState.setHydration(maxHydration);
            }
            else
            {
                ReferenceManager.Instance.playerState.setHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }


}