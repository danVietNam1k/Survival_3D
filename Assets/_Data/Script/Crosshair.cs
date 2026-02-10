using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    Transform dotCrosshair, handCrosshair, dialogCrosshair;

    private void Start()
    {
        dotCrosshair= this.transform.Find("DotCrosshair").transform;
        handCrosshair = this.transform.Find("HandCrosshair").transform;
        dialogCrosshair = this.transform.Find("DialogCrosshair");

    }
    public void SwitchCrosshair(Transform target)
    {

        if (target == null) {
            dotCrosshair.gameObject.SetActive(true);
            handCrosshair.gameObject.SetActive(false);
            dialogCrosshair.gameObject.SetActive(false);
            return;
        }

        switch (target.tag)
        {
            case "CanPickUp":
                dotCrosshair.gameObject.SetActive(false);
                handCrosshair.gameObject.SetActive(true);
                dialogCrosshair.gameObject.SetActive(false);
                break;
            case "NPC":
                handCrosshair.gameObject.SetActive(false);
                dotCrosshair.gameObject.SetActive(false);
                dialogCrosshair.gameObject.SetActive(!false);

                break;
            default:
                dotCrosshair.gameObject.SetActive(true);
                handCrosshair.gameObject.SetActive(false);
                dialogCrosshair.gameObject.SetActive(false);

                break;
        }
           
    }
}
