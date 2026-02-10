using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TheItemEquipping : MonoBehaviour
{
    public KeyCode action = KeyCode.Mouse0;
    Animator animator;
    public float thisDamege;
    public GameObject thisItemInQuickSlot;
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(action) && !InventorySystem.Instance.isOpenInventory)
        {
            animator.SetTrigger("Action");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<InteractableObject>() != null && other.GetComponent<InteractableObject>().CanHit)
        {
            other.GetComponent<InteractableObject>().TakeDamege(thisDamege);
            SoundAxeHitTarget(other);
        }
    }

    //Run is animator
    public void Consumed()
    {
        if (!thisItemInQuickSlot.GetComponent<InventoryItem>().isConsumable) return;

        thisItemInQuickSlot.GetComponent<InventoryItem>().consumingFunction();
        EquipSystem.Instance.NotChoseItemInQuickSlot();
        Destroy(thisItemInQuickSlot);
        Destroy(gameObject);
    }
    public void PlaySoundSwingTools() //use for axe, sword, pickaxe...
    {
        AudioClip sfx = SoundManager.Instance.containerSound.axeSwing;
        SoundManager.Instance.PlaySFX(sfx);
    }
    public void SoundAxeHitTarget(Collider other)
    {
        foreach(var name in NameStatic.FamilyTree)
        {
            if(other.gameObject.name == name)
            {
                
                int ran = Random.Range(0, SoundManager.Instance.containerSound.axeChoppingTree.Length);
                AudioClip sfx = SoundManager.Instance.containerSound.axeChoppingTree[ran];
                SoundManager.Instance.PlaySFX(sfx);
            }
        }
    }
    
    
}
