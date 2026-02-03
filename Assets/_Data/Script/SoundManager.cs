using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public ContainerSound containerSound;
    public AudioMixer audioMixer;
    public AudioSource musicBackGround;
    public AudioSource SFX;
    public AudioSource soundNPC_Talking;
    public AudioSource soundPlayerAction;
    Slider musicSlider, sfxSlider, matterSlider;
    public List<AudioClip> musicsBackground;
    float secondWaitMusic;

    [Header("ON/Off")]
    public bool playMusic;

    //
    const string Mix_Music = "Music";
    const string Mix_Master = "Master";
    const string Mix_Sfx = "SFX";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReplayMusicBackground());
        musicSlider = MenuManager.Instance.musicSlider;
        sfxSlider = MenuManager.Instance.sfxSlider;
        matterSlider = MenuManager.Instance.matterSlider;
        musicSlider.onValueChanged.AddListener(MusicVolume);
        sfxSlider.onValueChanged.AddListener(SFXVolume);
        matterSlider.onValueChanged.AddListener(MasterVolume);
    }

    private void MusicVolume(float volume)
    {
        audioMixer.SetFloat(Mix_Music, Mathf.Log10(volume)*20f);
    }
    private void SFXVolume(float volume)
    {
        audioMixer.SetFloat(Mix_Sfx, Mathf.Log10(volume)*20f);
    }
    private void MasterVolume(float volume)
    {
        audioMixer.SetFloat(Mix_Master, Mathf.Log10(volume)*20f);
    }
    void PlayMusicBackground()
    {

        
            int index = Random.Range(0,containerSound.musicsBackground.Count);
        AudioClip audioClip = containerSound.musicsBackground[index];
            musicBackGround.clip = audioClip;
            musicBackGround.Play();

            secondWaitMusic = containerSound.musicsBackground[index].length;
        

    }
    IEnumerator ReplayMusicBackground()
    {
        while (playMusic)
        {
            PlayMusicBackground();
            yield return new WaitForSeconds(secondWaitMusic);
        }
        
    }
    public void PlaySoundPlayerAction(AudioClip audioClip)
    {
        
            soundPlayerAction.PlayOneShot(audioClip);
       
    }
    public void PlaySFX(AudioClip audioClip)
    {
       
            
            SFX.PlayOneShot(audioClip);
       
    }
    public void PlaySoundNPC(AudioClip audioClip, Transform transform)
    {
        soundNPC_Talking.transform.position = transform.position;
        if (soundNPC_Talking.isPlaying)
        {
            soundNPC_Talking.Stop();
            soundNPC_Talking.PlayOneShot(audioClip);
        }
        else
        {
            soundNPC_Talking.PlayOneShot(audioClip);
        }
    }

    }
