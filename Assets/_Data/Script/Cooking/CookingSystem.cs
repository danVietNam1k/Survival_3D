using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CookingSystem : MonoBehaviour
{
    public static CookingSystem Instance { get; private set; }
    public CookingContainer cookingContainer;
    private void Awake()
    {
        Instance = this;

    }

    public void StartCoking(eCanPickupItemType type, Transform pos)
    {
        switch (type)
        {
            case eCanPickupItemType.RawMeat:
                pos.GetComponent<PickUp>().countTimeCooking -= Time.deltaTime;
                print("cooking");
                print(pos.GetComponent<PickUp>().countTimeCooking);

                if (pos.GetComponent<PickUp>().countTimeCooking < 0)
                {
                    var newFood = Instantiate(cookingContainer.CookedMeat);
                    newFood.name = NameStatic.CookedMeat;
                    newFood.transform.position = pos.position;
                    pos.GetComponent<PickUp>().countTimeCooking = 2;
                    Destroy(pos.gameObject);
                }

             break;
        }
    
        
    }
}
