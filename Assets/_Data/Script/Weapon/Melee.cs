using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private float meleeDamge = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Animal>() != null)
        {
            other.GetComponent<Animal>().TakeDamge(meleeDamge);
        }
        other.transform.GetComponent<BossCtl>()?.TakeDamge(meleeDamge);

    }
}
