using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoItemShop : MonoBehaviour
{

    public string nameItem;
    public int priceItem;
    public Button buyBTN;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceTxt;
    // Start is called before the first frame update
    void Start()
    {
       
        nameText = transform.Find("Text Name").GetComponent<TextMeshProUGUI>();
        priceTxt = transform.Find("Text Price").GetComponent<TextMeshProUGUI>();
        buyBTN = transform.Find("Button").GetComponent<Button>();
        nameText.text = nameItem;
        priceTxt.text = priceItem + " BitCoin";
        buyBTN.onClick.AddListener(() =>
        {
            BuytItem();
        });
        LoadSprite();
    }
    private void OnValidate()
    {
        this.name = nameItem;
    }
    private void BuytItem()
    {
        if (InventorySystem.Instance.CheckInventoryIsFull()) return;
        InventorySystem.Instance.currentMonney -= priceItem;
        InventorySystem.Instance.AddItemToInventoryAndPopup(nameItem, true);
    }

    private void FixedUpdate()
    {
        checkBuyable();
    }

    private void checkBuyable()
    {
        float currentMonney = InventorySystem.Instance.currentMonney;
        if(currentMonney >= priceItem)
        {
            buyBTN.interactable = true;
        }
        else
        {
            buyBTN.interactable = false;

        }
    }
    void LoadSprite()
    {
        Sprite item = Resources.Load<GameObject>("Item_Inventory/"+ nameItem).GetComponent<Image>().sprite;
        transform.Find("Avatar").GetComponent<Image>().sprite = item;
    }
}
