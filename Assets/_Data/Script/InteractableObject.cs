using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    //state
    public bool CanHit;
    public float maxHealth, currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
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
        foreach (string nameTree in GroupFamily.FamilyTree)
        {
            if (ItemName == nameTree)
            {
                obj = Instantiate(Resources.Load<GameObject>("Item_obj/DeadTree"));
                goto determined;
            }
        }

        foreach (string nameTree in GroupFamily.FamilyAnimal)
        {
            if (ItemName == nameTree)
            {
                obj = Instantiate(Resources.Load<GameObject>("Item_obj/Meat"));
                goto determined;
            }
        }
        if (obj == null)
        {
            Destroy(gameObject);
            return;
        }

    determined:
        obj.transform.position = this.transform.position;
        Destroy(gameObject);
    }

}
    public class GroupFamily
{
    
    public static List<string> FamilyTree = new List<string>{ "Pine tree", "Willow tree", "Oak tree", "Deciduous tree", "Birch tree" };
    public static List<string> FamilyAnimal = new List<string> { "Rabbit" };


}
