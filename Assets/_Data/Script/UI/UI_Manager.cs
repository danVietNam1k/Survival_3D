using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; set; }
    Canvas canvasUI;
    Transform deadUI;
   

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    private void Start()
    {
        canvasUI = ReferenceManager.Instance.canvas;
        deadUI = canvasUI.transform.Find("DeadUI");
    }
    private void Update()
    {
        OpenUI();
    }
    void OpenUI()
    {
        if (PlayerState.Instance.isPlayerdead && !deadUI.gameObject.activeSelf)
        {
            deadUI.gameObject.SetActive(true);
        }
    }

}
