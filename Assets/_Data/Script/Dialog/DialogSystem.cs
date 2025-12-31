using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance {  get; private set; }

    public bool thePlayerTalking = false;
    public GameObject canvasDialog, canvasUI;
    public TextMeshProUGUI dialogText;
    public Button answearBTN1, answearBTN2, answearBTN3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenDialogUI()
    {
        canvasDialog.SetActive(true);
        canvasUI.SetActive(false);
        thePlayerTalking = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    public void CloseDialogUI()
    {
        canvasDialog.SetActive(false);
        canvasUI.SetActive(true);

        thePlayerTalking = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
