using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemFallout : MonoBehaviour
{
    public List<FalloutItemPossibility> possibilityItem;
    public List<FalloutItemActual> actualItems;
    public bool wasLootCalculated;

    public void FallOutItem()
    {
        foreach (FalloutItemPossibility possibility in possibilityItem)
        {
            var randomAmount = Random.Range(possibility.amountMin, possibility.amountMax+1);
            print("ran "+randomAmount);
            if (randomAmount > 0)
            {
                FalloutItemActual falloutItemActual = new FalloutItemActual();
                falloutItemActual.amount = randomAmount;
                falloutItemActual.item = possibility.item;
                actualItems.Add(falloutItemActual);
            }
        }
        foreach(FalloutItemActual falloutItemActual1 in actualItems)
        {
            for(int i = 0; i <falloutItemActual1.amount; i++)
            {
                print("i"+i);
                GameObject item = Instantiate(falloutItemActual1.item);
                Vector3 bornPos = this.transform.position;
                bornPos.y +=0.1f;
                bornPos.z += Random.Range(-0.2f, 0.2f);
                bornPos.x += Random.Range(-0.2f, 0.2f);
                item.transform.position = bornPos;


            }
        }
        
    }
}

[System.Serializable]
public class FalloutItemPossibility
{
    public GameObject item;
    public int amountMin;
    public int amountMax;
}
[System.Serializable]

public class FalloutItemActual
{
    public GameObject item;
    public int amount;
}
