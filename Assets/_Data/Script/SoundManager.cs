using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public ContainerSound containerSound;

    public AudioSource musicBackGround;
    public AudioSource SFX;
    public AudioSource soundNPC_Talking;
    public AudioSource soundPlayerAction;

    public List<AudioClip> musicsBackground;
    float secondWaitMusic;

    [Header("ON/Off")]
    public bool playMusic;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReplayMusicBackground());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayMusicBackground()
    {

        
            int index = Random.Range(0,containerSound.musicsBackground.Count);
        
            musicBackGround.PlayOneShot(containerSound.musicsBackground[index]);
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
        if (soundPlayerAction.isPlaying)
        {
            soundPlayerAction.Stop();
            soundPlayerAction.PlayOneShot(audioClip);
        }
        else
        {
            soundPlayerAction.PlayOneShot(audioClip);
        }
    }
    public void PlaySFX(AudioClip audioClip)
    {
        if (SFX.isPlaying)
        {
            SFX.Stop();
            SFX.PlayOneShot(audioClip);
        }
        else
        {
            SFX.PlayOneShot(audioClip);
        }
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
