using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Arow : MonoBehaviour
{
    Rigidbody rb;
   
    public float damge;
    public bool isStuck = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 20f); 
    }
    void FixedUpdate()
    {
        if (isStuck) return;

        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
    // Update is called once per frame
    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag != "Player" &&!isStuck)
        {
            print(other.transform.name);
            isStuck = true;
            GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
            if (other.collider.GetComponent<Animal>())
            {
                other.collider.GetComponent<Animal>().TakeDamge(damge);
            }
            other.transform.GetComponent<BossCtl>()?.TakeDamge(damge);

            this.GetComponent<TrailRenderer>().enabled = false ;
            this.GetComponent<Collider>().enabled = false;
            this.transform.SetParent(other.transform);
        }
       

    }
    
}
