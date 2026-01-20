using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PriceItemContainer", menuName = "ScriptableObjects/PriceItemContainer", order = 1)]
public class PriceItemContainer:ScriptableObject
{
   public List<ItemPrice> priceItemList;
  
}
[System.Serializable]
public class ItemPrice
{
    public string Name;
    public eInventoryItemType type;
    public int price;
    

}
