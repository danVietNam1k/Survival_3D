using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeadUI : MonoBehaviour
{
    // Start is called before the first frame update
    Image image;
    TextMeshProUGUI text;
    GameObject textClick;
    void Awake()
    {
        image = transform.GetComponent<Image>();
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        textClick = transform.Find("TextClick").gameObject;
    }
    private void OnEnable()
    {
        ResetValue();
        InvokeRepeating(nameof(FlickerText),2f,1f);
    }
    private void ResetValue()
    {
        image.color = new Color(0, 0, 0, 0);
        text.gameObject.SetActive(false);
        textClick.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        float newValue = image.color.a +Time.deltaTime;
        image.color = new Color(0, 0, 0, newValue);
       
        if(newValue > 1f)
        {
            if(!text.gameObject.activeSelf) 
            text.gameObject.SetActive(true);

            if (Input.anyKeyDown)
            {
                CancelInvoke(nameof(FlickerText));
                
                GameManager.Instance.LoadScene("MainMenu");
            }
        }
    }
    void FlickerText()
    {  
            textClick.SetActive(!textClick.activeSelf);   
    }
     
}
