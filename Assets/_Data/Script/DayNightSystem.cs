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
    public float lightIntensity;
    public AnimationCurve animationCurve;
    float currentTimeOfDay = 9/24f;
    public List<SkyboxTimeMapping> timeMappings;

    public float blenderValue = 0f;
    bool triggerNextDay = false;
    private float valueBright = 200f;
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
    private void FixedUpdate()
    {
        EnvironmentLight();

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
        SwitchlightMoonSun();
    }
    // void EnviromentLight()
    // {
    //     if (currenthour >= 6f && currenthour <= 19f)
    //     {
    //         if (valueBright < 200f)
    //         {
    //             valueBright += Time.deltaTime / (dayDurationInSeconds/60);
    //             float i = valueBright / 255f;
    //             RenderSettings.ambientLight =new Color(i, i, i);
                
    //         }
    //     }
    //     else
    //     {
    //         if (valueBright > 0f)
    //         {
    //             valueBright -= Time.deltaTime * dayDurationInSeconds;
    //             float i = valueBright / 255f;
    //             RenderSettings.ambientLight = new Color(i, i, i);
    //         }
    //     }
    // }
    void EnvironmentLight()
{
    float targetBrightness = GetTargetBrightness(currenthour);

    valueBright = Mathf.Lerp(valueBright, targetBrightness * 200f, Time.deltaTime * 2f);

    float i = valueBright / 255f;
    RenderSettings.ambientLight = new Color(i, i, i);
}

float GetTargetBrightness(float hour)
{
    if (hour < 5f || hour >= 20f)
        return 0f;

    if (hour >= 5f && hour < 7f)
        return Mathf.InverseLerp(5f, 7f, hour);

    if (hour >= 7f && hour < 18f)
        return 1f;

    if (hour >= 18f && hour < 20f)
        return Mathf.InverseLerp(20f, 18f, hour);

    return 0f;
}
    void SwitchlightMoonSun()
    {
        if (currenthour >= 6f && currenthour <= 17f)
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

