using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrucItemOnHand : MonoBehaviour
{
    private void Start()
    {
        OnSelect();
    }
    private void OnSelect()
    {
        ConstructionManager.Instance.ReferenceItemOnHand(gameObject);
        ConstructionManager.Instance.StartConstruction(gameObject.name);

    }

    //run in animator
    public void StartConstruct()
    {
        
        if (!ConstructionManager.Instance.canBuild
            || (ConstructionManager.Instance.ghost.activeSelf ==false 
            && ConstructionManager.Instance.ghost !=null)) return;
        ConstructionManager.Instance.isBuilding = true;
    }

    public void Constructed()
    { if ( !ConstructionManager.Instance.isBuilding) return;
        ConstructionManager.Instance.Construction();
        Destroy(transform.GetComponent<TheItemEquipping>().thisItemInQuickSlot);
        EquipSystem.Instance.SetUnEquippedModel();
        Destroy(gameObject);
    }
}
