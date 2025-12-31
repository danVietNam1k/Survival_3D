using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    Transform dotCrosshair, handCrosshair;

    private void Start()
    {
        dotCrosshair= this.transform.Find("DotCrosshair").transform;
        handCrosshair = this.transform.Find("HandCrosshair").transform;

    }
    public void SwitchCrosshair(Transform target)
    {

        if (target == null) {
            dotCrosshair.gameObject.SetActive(true);
            handCrosshair.gameObject.SetActive(false);
            return;
        }

        switch (target.tag)
        {
            case "CanPickUp":
                dotCrosshair.gameObject.SetActive(false);
                handCrosshair.gameObject.SetActive(true);
                break;
            case "NPC":
                //handCrosshair.gameObject.SetActive(false);
                //handCrosshair.gameObject.SetActive(false);

                break;
            default:
                dotCrosshair.gameObject.SetActive(true);
                handCrosshair.gameObject.SetActive(false);
                break;
        }
           
    }
}
