using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI buttonText;
    public int slotNumber;
    public Transform overrideWarning;
   public Button YesBTN, NoBTN;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.GetComponentInChildren<TextMeshProUGUI>();
        
        YesBTN = overrideWarning.Find("ButtonYes").GetComponent<Button>();
        NoBTN = overrideWarning.Find("ButtonNo").GetComponent<Button>();
    }
    void Start()
    {
     

        button.onClick.AddListener(() =>
        {
            if (IsSlotEmpty())
            {
                SaveGameConfirmed();
            }
            else { SaveGameOverrideWarning(); }
            
        });
      
    }
    void SaveGameOverrideWarning()
    {
        overrideWarning.gameObject.SetActive(true);
        YesBTN.onClick.AddListener(() =>
        {
            print("click yes btn");
            SaveGameConfirmed();
            overrideWarning.gameObject.SetActive(false);

        });
        NoBTN.onClick.AddListener(() =>
        {
            overrideWarning.gameObject.SetActive(false);
        });
    }
    void SaveGameConfirmed()
    {

        SaveManager.Instance.SaveGame(slotNumber);
        DateTime dt = DateTime.Now;
        string time = dt.ToString("dd-MM-yyyy HH:mm");
        string description = "Saved Game" + slotNumber + "|" + time;
        buttonText.text = description;
        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);
        DeselectButton();
    }
    private void FixedUpdate()
    {
        if (IsSlotEmpty()) {
            buttonText.text = "Empty";        
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    private void DeselectButton()
    {
        GameObject myEventSystem = ReferenceManager.Instance.eventSystem;
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
    private bool IsSlotEmpty()
    {
        if (SaveManager.Instance.DoesFileExists(slotNumber))
        {
            return false;
        }
        else return true;   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
