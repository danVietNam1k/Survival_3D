using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamge : MonoBehaviour
{
    [SerializeField] public float damge = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState.Instance.TakeDamge(damge);
        }
    }
   
}
