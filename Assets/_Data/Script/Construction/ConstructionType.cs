using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionType : MonoBehaviour
{

    public eConstructItemType constructItemType;
    public void ConstructDone()
    {
        switch (constructItemType)
        {
            case eConstructItemType.CampFire:
                GetComponent<CampfireCtrl>().enabled = true;
                GetComponent<CampfireCtrl>().thisBeConstructed = true;
               
                GetComponent<BoxCollider>().isTrigger = false;
                Destroy(GetComponent<Outline>());
                Destroy(GetComponent<ConstructionCheck>());
                this.transform.GetChild(0).gameObject.SetActive(true);

            break;
            case eConstructItemType.SomeItemPlacement:
                this.tag = NameStatic.ChestStorage;
                this.GetComponent<StorageChest>().enabled = true;
                break; 
            default:
                this.tag = "Constructed";
                foreach (Transform child in this.transform)
                { child.gameObject.SetActive(true); }
                break;
        }
        Destroy(this);


    }
}
