using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Slider staminaBar, healBar, caloriesBar, hydrationBar;
    public PlayerState playerState;
    void Reset()
    {
        staminaBar = this.transform.GetChild(0).GetComponent<Slider>();
        healBar = this.transform.GetChild(1).GetComponent<Slider>();
        caloriesBar = this.transform.GetChild(2).GetComponent<Slider>();
        hydrationBar = this.transform.GetChild(3).GetComponent<Slider>();
        playerState = ReferenceManager.Instance.playerState;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCaloriesBar();
        UpdateHealBar();
        UpdateStaminaBar();
        UpdateHydrationBar();
    }
    void UpdateStaminaBar()
    {
        float currentStamina = playerState.currentStamina;
        float maxStamina = playerState.maxStamina;
        float fillValue = currentStamina / maxStamina;
        staminaBar.value = fillValue;
        

        if (fillValue <1f)
        {
            staminaBar.transform.gameObject.SetActive(true);
        }
        else
        {
            staminaBar.transform.gameObject.SetActive(false);
        }
     }
    void UpdateHealBar()
    {
        float currentHeal = playerState.currentHeal;
        float maxHeal = playerState.maxHeal;
        float fillValue = currentHeal / maxHeal;
        healBar.value = fillValue;

    }
    void UpdateCaloriesBar()
    {
        float currentCalories = playerState.currentCalories;
        float maxCalories = playerState.maxCalories;
        float fillValue = currentCalories / maxCalories;
        caloriesBar.value = fillValue;
       
    }
    void UpdateHydrationBar()
    {
        float currentHydration = playerState.currentHydration;
        float maxHydration = playerState.maxHydration;
        float fillValue = currentHydration / maxHydration;
        hydrationBar.value = fillValue;
    }
}

