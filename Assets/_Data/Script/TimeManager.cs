using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    public int dayNumberInGame = 1;

    public TextMeshProUGUI textDay;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        textDay.text = "Day: " + dayNumberInGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TriggerNextDay()
    {
        dayNumberInGame++;
        textDay.text = "Day: " + dayNumberInGame;

    }
}
