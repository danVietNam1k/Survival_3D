using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;
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

        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(currentTimeOfDay * 360 - 90, 170, 0));
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
                print(currentSkybox);
                if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                {
                    print("into shader name     ");

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
    }
}
[System.Serializable]
    public class SkyboxTimeMapping
    {
        public string phaseName;
        public int hour; //hour of the day (0-23)
        public Material skyboxMaterial;
    }

