using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftCtl : MonoBehaviour
{
    public bool have2Require;
    public int amountCraft;
    
    public eInventoryItemType nameItemRq1;
    
    public int amountItemRq1;
    public eInventoryItemType nameItemRq2;
    public int amountItemRq2;
   
    Text rq1,rq2;
    Button craftBNT;
    // Start is called before the first frame update
    void Start()
    {
        rq1 = transform.Find("BG/req1").GetComponent<Text>();
        rq2 = transform.Find("BG/req2").GetComponent<Text>();     
        craftBNT = transform.Find("BG/CraftButton").GetComponent<Button>();
        craftBNT.onClick.AddListener(() =>
        {
            int amountRq = 1;
            if (have2Require)
            {
                amountRq = 2;
            }
            Blueprint blueprint = new Blueprint(this.name, amountCraft, amountRq, nameItemRq1.ToString(), amountItemRq1, nameItemRq2.ToString(), amountItemRq2);
            CraftingSystem.Instance.CraftAnyItem(blueprint);
        });

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        int amount = FilterItems(nameItemRq1);
        int amount2 = FilterItems(nameItemRq2);

       
        if (have2Require)
        {
           
            rq2.text = amount2 + "/" + amountItemRq1 + " " + nameItemRq2.ToString();
            rq1.text = amount + "/" + amountItemRq1 + " " + nameItemRq1.ToString();
            if (amount >= amountItemRq1 && amount2 >=amountItemRq2)
            {
                craftBNT.interactable = true;
            }
            else { craftBNT.interactable = false; }

        }
        else
        {
            rq1.text = amount + "/" + amountItemRq1 + " " + nameItemRq1.ToString();
            rq2.text = "";
            if (amount >= amountItemRq1)
            {
                craftBNT.interactable = true;

            }
            else { craftBNT.interactable = false; }

        }
    }
    int FilterItems(eInventoryItemType type)
    {
        switch (type)
        {
            case eInventoryItemType.Stick:
               return CraftingSystem.Instance.amountStick;
            case eInventoryItemType.Stone:
                return CraftingSystem.Instance.amountStone;
            case eInventoryItemType.Plank:
                return CraftingSystem.Instance.amountPlank;
            case eInventoryItemType.LogWooden:
                return CraftingSystem.Instance.amountLog;
        }
        return 0;
    }

}
