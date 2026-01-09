using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance {  get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }else Instance = this;
    }
    public Transform playerBody;

    //Hp
    public float currentHeal, maxHeal;

    //stamina
    public float currentStamina, maxStamina;

    //calories
    public float currentCalories, maxCalories;

    //hydration
    public float currentHydration, maxHydration;
    // 
    KeyCode sprintKey;
    FirstPersonController firstPersonController;
    // Start is called before the first frame update
    void Start()
    {
        currentHeal = maxHeal;
        currentStamina = maxStamina;
        currentCalories = maxCalories;
        currentHydration = maxHydration;
        firstPersonController = ReferenceManager.Instance.player.GetComponent<FirstPersonController>();
        StartCoroutine(MinusHydration());
        StartCoroutine(MinusCalories());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStamina();

    }
    private void UpdateHeal()
    {
        if (currentHeal <= 0f) return;


    }
    private void UpdateStamina()
    {
        bool isSprinting = firstPersonController.isSprinting;
        if (isSprinting) 
        {
            if (currentStamina <= 0f)
            {
                firstPersonController.canSprint = false;
                return;
            }
            currentStamina -= 10f*Time.deltaTime;
        }
        else
        {
            if (currentStamina >= maxStamina)
            {
                return;
            }
            else if (currentStamina > maxStamina / 2)
            {
                firstPersonController.canSprint = true;
            }
                currentStamina += 10f * Time.deltaTime;
        }

           
    }
    
    void UpdateHydration()
    {

        currentHydration -= 1f;


    }
    void UpdateCalories()
    {
        currentCalories -= 1f;

    }
    IEnumerator MinusCalories()
    {
       
        while (currentCalories > 0f)
        {
           
            yield return new WaitForSeconds(1f);
            UpdateCalories();
        }
    }
    IEnumerator MinusHydration()
    {
        while (currentHydration > 0f)
        {
          
            yield return new WaitForSeconds(10f);
            UpdateHydration();
        }
    }
    public void setHydration(float newHydration)
    {
        currentHydration = newHydration;
    }
    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }
    public void setHealth(float newHealth)
    {
        currentHeal = newHealth;
    }

}
