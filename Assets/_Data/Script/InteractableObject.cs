using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    //state
    public bool showHp = true;
    public bool CanHit;
    public float maxHealth, currentHealth;
    public Animator animator;
    
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        this.name = GetItemName();
    }
    public string GetItemName()
    {
        return ItemName;
    }
    public float GetHpInfor()
    {
        return currentHealth/maxHealth;
    }
    public void TakeDamege(float damegeTakeIn)
    {
        if (!CanHit) return;
        currentHealth -= damegeTakeIn;
        if(currentHealth<= 0)
        {
            currentHealth = 0;
            DaedState();
        }
        
    }
    void DaedState()
    {
        GameObject obj = null;
        foreach (string nameTree in NameStatic.FamilyTree)
        {
            if (ItemName == nameTree)
            {
                StateDeadOfTree(obj);
                return;
            }
        }

        foreach (string nameAnimal in NameStatic.FamilyAnimal)
        {
            if (ItemName == nameAnimal)
            {
              GetComponent<Animal>().StateDeadOfAnimal(animator);
                return;

            }
        }
        if (obj == null)
        {
            print("obj equal null");
            Destroy(gameObject);
            return;
        }

    }
    void StateDeadOfTree(GameObject obj)
    {
        obj = Instantiate(Resources.Load<GameObject>("Item_obj/DeadTree"));

        obj.transform.position = this.transform.position;
        Destroy(gameObject);
    }
   
 

}

