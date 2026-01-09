using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ConstructionCheck : MonoBehaviour
{
   
    private HashSet<Collider> objectsInTrigger = new HashSet<Collider>();
    private Outline outline;
    private void Start()
    {
        outline = GetComponent<Outline>();
    }
    private void OnEnable()
    {
        objectsInTrigger.Clear();
        ConstructionManager.Instance.canBuild = true;
    }
    public bool IsTriggerEmpty()
    {
        return objectsInTrigger.Count == 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GhostConstructed")) return;
        //if (other.CompareTag("Constructed")) return;

        if (other.CompareTag("Ground")) return ;
        
            objectsInTrigger.Add(other);

            //if (!ConstructionManager.Instance.canBuild) return;
            //ConstructionManager.Instance.canBuild = false;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GhostConstructed")) return;
        //if (other.CompareTag("Constructed")) return;

        if (other.CompareTag("Ground")) return;
        objectsInTrigger.Remove(other);


        
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (!other.CompareTag("Ground") )
    //    {
    //        if(!ConstructionManager.Instance.canBuild) return;
    //        ConstructionManager.Instance.canBuild = false;
    //        Debug.Log(other.name +"enter");
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.CompareTag("Ground"))
    //    {
    //        ConstructionManager.Instance.canBuild = true;
    //        Debug.Log(other.name + "exit");

    //    }
    //}

    // Set Outline
    public void SetInvalidColor()
    {
        if (outline != null)
        {
            outline.enabled = true;
            outline.OutlineColor = Color.red;
        }
    }
    public void SetValidColor()
    {
        if (outline != null)
        {
            outline.enabled = true;

            outline.OutlineColor = Color.green;
        }
    }
    public void SetDefaultColor()
    {
        if (outline != null)
        {
            Destroy(outline);
        }
      

    }
}
