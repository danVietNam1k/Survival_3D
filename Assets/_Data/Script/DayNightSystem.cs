using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public Light DayLight;
    public Light EveningLight;
    public float dayDurationInSeconds = 24f;
    public int currenthour;
    float currentTimeOfDay = 9/24f;
    public List<SkyboxTimeMapping> timeMappings;

    public float blenderValue = 0f;
    bool triggerNextDay = false;

    public TextMeshProUGUI textTime;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;
        currenthour = Mathf.FloorToInt(currentTimeOfDay * 24);

        DayLight.transform.rotation = Quaternion.Euler(new Vector3(currentTimeOfDay * 360 - 90, 170, 0));
        EveningLight.transform.rotation = Quaternion.Euler(new Vector3(currentTimeOfDay * 360 - 270, 170, 0));
        UpdateSkybox();

    }
    void UpdateSkybox()
    {

        textTime.text = "Time: "+ currenthour + ":00";

        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currenthour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;
                if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                {
                  
                    blenderValue += Time.deltaTime;
                    blenderValue = Mathf.Clamp01(blenderValue);
                    currentSkybox.SetFloat("_TransitionFactor", blenderValue);
                }
                else
                {
                    blenderValue = 0;
                }
                    break;
            }

        }
        if (currenthour == 0)
        {
            if(!triggerNextDay) return;
            TimeManager.Instance.TriggerNextDay();
            triggerNextDay =false;
        }else triggerNextDay = true;

            if (currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;

        }
        TurnOfflight();
    }
    void TurnOfflight()
    {
        if (currenthour >= 5f && currenthour <= 17f)
        {
            DayLight.enabled = true;
            EveningLight.enabled = false;

        }
        else
        {
            DayLight.enabled = false;
            EveningLight.enabled = true;
        }
    }
}
[System.Serializable]
    public class SkyboxTimeMapping
    {
        public string phaseName;
        public int hour; //hour of the day (0-23)
        public Material skyboxMaterial;
    }

