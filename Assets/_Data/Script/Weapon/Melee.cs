using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private float meleeDamge = 20f;
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        
        other.GetComponent<Animal>()?.TakeDamge(meleeDamge);
        
        other.transform.GetComponent<BossCtl>()?.TakeDamge(meleeDamge);

    }
}
