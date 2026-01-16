using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
[RequireComponent(typeof(AudioSource))]
public class NPC_Shopkeeper : MonoBehaviour
{
    TextMeshProUGUI npcDialogText, answearBTN1Text, answearBTN2Text, answearBTN3Text;
    public AudioClip shopkeeperVoice;
    public string shopkeeperDialog;
    Button answearBTN1, answearBTN2, answearBTN3;
    GameObject ShopUI;
    public GameObject slotSellUI;
    List<GameObject> slotSells = new();
    public TextMeshProUGUI txtTotalMonneySell;
    int totalMonneySellItem;
    public List<ItemSell> itemSells = new();

    private void Update()
    {
        ShowMonneyTotalSellItem();
    }
    // Start is called before the first frame update
    void Start()
    {
        npcDialogText = DialogSystem.Instance.dialogText;
        answearBTN1 = DialogSystem.Instance.answearBTN1;
        answearBTN1Text = answearBTN1.transform.GetComponentInChildren<TextMeshProUGUI>();
        answearBTN2 = DialogSystem.Instance.answearBTN2;
        answearBTN2Text = answearBTN2.transform.GetComponentInChildren<TextMeshProUGUI>();
        answearBTN3 = DialogSystem.Instance.answearBTN3;
        answearBTN3Text = answearBTN3.transform.GetComponentInChildren<TextMeshProUGUI>();
        ShopUI = transform.Find("ShopUI").gameObject;
        LoadSlotSell();
    }
    public void StartShopkeeperConvesation()
    {
        DialogSystem.Instance.OpenDialogUI();
        npcDialogText.text = shopkeeperDialog;
        GetComponent<AudioSource>().PlayOneShot(shopkeeperVoice);
        answearBTN1.gameObject.SetActive(true);
        answearBTN2.gameObject.SetActive(true);
        answearBTN3.gameObject.SetActive(true);
        answearBTN1.onClick.RemoveAllListeners();
        answearBTN2.onClick.RemoveAllListeners();
        answearBTN3.onClick.RemoveAllListeners();
        answearBTN1Text.text = "Talk";
        answearBTN2Text.text = "Buy";
        answearBTN3Text.text = "Leave";
        answearBTN1.onClick.AddListener(() =>
        {

            GetComponent<NPC>().StartQuestConvesation();
            
        });
        answearBTN2.onClick.AddListener(() =>
        {

            OpenShopUI();
            DialogSystem.Instance.CloseDialogUI();
        });
        answearBTN3.onClick.AddListener(() =>
        {

            DialogSystem.Instance.CloseDialogUI();
        });


    }
    void LoadSlotSell()
    {
        foreach (Transform child in slotSellUI.transform)
        {
            slotSells.Add(child.gameObject);
        }
    }
    public void CloseShopUI()
    {
        ShopUI.SetActive(false);
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);
        ReturnItemToInventory();
        InventorySystem.Instance.isOpeningShop = false;

    }
     
    public void OpenShopUI()
    {
        InventorySystem.Instance.inventoryScreenUI.SetActive(true);
        ShopUI.SetActive(true);
        InventorySystem.Instance.isOpeningShop = true;
    }
    void ReturnItemToInventory()
    {
        
        foreach (GameObject child in slotSells)
        {
            if (child.transform.childCount > 0)
            {
                GameObject newSlot = InventorySystem.Instance.FindNextEptySlot();
                Transform item = child.transform.GetChild(0);
                item.SetParent(newSlot.transform);
                item.localPosition = new Vector2(0, 0);
                
                
                
            }
        }
    }
    private void ShowMonneyTotalSellItem()
    {
        if (InventorySystem.Instance.isOpeningShop)
        {
            totalMonneySellItem = 0;
            List<Transform> item = new();
            foreach(GameObject child in slotSells)
            {
                if(child.transform.childCount > 0)
                {
                    item.Add(child.transform.GetChild(0));
                }
            }
            foreach(Transform child in item)
            {
                foreach(ItemSell itemSell in itemSells)
                {
                   if(child.GetComponent<InventoryItem>().type == itemSell.type)
                    {
                        totalMonneySellItem =+ itemSell.itemPrice;
                    }
                }
                
            }

            txtTotalMonneySell.text = totalMonneySellItem.ToString() + " Bitcoin";
        }

    }
    
}
[System.Serializable]
public class ItemSell
{
    public eInventoryItemType type;
    public int itemPrice;
}
