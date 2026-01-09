using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Animal : MonoBehaviour
{
    public GameObject bloodFX;
    public AudioClip AudioClip;
    public ParticleSystem particle;
    private void Start()
    {
        bloodFX = this.transform.Find("blood_puddle").gameObject;
    }
    public void StateDeadOfAnimal(Animator animator)
    {
        //obj = Instantiate(Resources.Load<GameObject>("Item_obj/Meat"));
        //obj.transform.position = this.transform.position;

        animator.SetTrigger("isDying");
        bloodFX.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<AI_Movement>().enabled = false;

        GetComponent<ItemFallout>()?.FallOutItem();
        StartCoroutine(WaitForDestroyAnimal());
    }
    IEnumerator WaitForDestroyAnimal()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);


    }
    public void PlaySoundHitRabbit()
    {
        particle.Play();
        SoundManager.Instance.PlaySFX(AudioClip);
    }
}
