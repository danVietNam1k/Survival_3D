using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance {  get; private set; }
    public GameObject settingsMenu;
    public Slider musicSlider, sfxSlider, matterSlider;
    public Button backSettingsBTNl;
     void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        backSettingsBTNl.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, sfxSlider.value, matterSlider.value);
            print("back Button be press");
        });

        StartCoroutine(LoadAndApplySettings());
    }
    private IEnumerator LoadAndApplySettings()
    {

        yield return new WaitForSeconds(0.1f);
        LoadAndSetVolume();

    }
    void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        if (volumeSettings != null)
        {
            matterSlider.value = volumeSettings.master;
            sfxSlider.value = volumeSettings.effects;
            musicSlider.value = volumeSettings.music;
            print("Load and set volume");

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
