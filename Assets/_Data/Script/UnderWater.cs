using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWater : MonoBehaviour
{

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckWaterPlayer"))
        {
            print(other.gameObject);
            other.GetComponentInParent<FirstPersonController>().isSwimming = true;
        }
        if (other.CompareTag("MainCamera")){
            other.GetComponentInParent<FirstPersonController>().isUnderWater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckWaterPlayer"))
        {
            other.GetComponentInParent<FirstPersonController>().isSwimming = false;
        }
        if (other.CompareTag("MainCamera")){
            other.GetComponentInParent<FirstPersonController>().isUnderWater = false;
        }
    }
}
