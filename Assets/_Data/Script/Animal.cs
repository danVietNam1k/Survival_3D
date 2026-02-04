using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;
[RequireComponent(typeof(AudioSource))]
public class Animal : MonoBehaviour
{
    ParticleSystem particle;
    GameObject bloodFX;
    public AudioAnimal[] audioAnimal;
    public float maxHealth, currentHealth;
    public Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        bloodFX = this.transform.Find("blood_puddle").gameObject;
        particle = this.transform.Find("BloodHitFX").GetComponent<ParticleSystem>();
    }
    public bool isDeath = false;
    private void OnEnable()
    {
        RefreshState();
    }
    public void TakeDamge(float damegeTakeIn)
    {
        
        currentHealth -= damegeTakeIn;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StateDeadOfAnimal(animator);
        }

    }
    public void StateDeadOfAnimal(Animator animator)
    {
        //obj = Instantiate(Resources.Load<GameObject>("Item_obj/Meat"));
        //obj.transform.position = this.transform.position;
        isDeath = true;
        PlaySound("Death");
        animator.SetTrigger("isDying");
        GetComponent<Rigidbody>().isKinematic = true;
        bloodFX.SetActive(true);
        GetComponent<ItemFallout>()?.FallOutItem();
        StartCoroutine(WaitForDestroyAnimal());

    }
    void RefreshState()
    {
        currentHealth = maxHealth;
        isDeath = false ;

    }
    IEnumerator WaitForDestroyAnimal()
    {
        yield return new WaitForSeconds(10f);
        transform.gameObject.SetActive(false);


    }
    public void PlaySoundHitAnimal()
    {
        particle.Play();
        SoundManager.Instance.PlaySFX(audioAnimal[0].audioclip);
    }
    public void PlaySound(string soundName)
    {
        foreach(AudioAnimal audioInfo in audioAnimal)
        {
            if(audioInfo.Name == soundName) 
                GetComponent<AudioSource>().PlayOneShot(audioInfo.audioclip);
        }
    }

}
[System.Serializable]
public class AudioAnimal
{
    public string Name;
    public AudioClip audioclip;
}
