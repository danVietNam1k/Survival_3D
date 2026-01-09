using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI buttonText;
    public int slotNumber;
    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            SaveManager.Instance.LoadAllGameData(slotNumber);
        });
    }
    private void FixedUpdate()
    {
        if (IsSlotEmpty())
        {
            buttonText.text = "Empty";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");

        }
    }
   
    private bool IsSlotEmpty()
    {
        if (SaveManager.Instance.DoesFileExists(slotNumber))
        {
            return false;
        }
        else return true;
    }
}
