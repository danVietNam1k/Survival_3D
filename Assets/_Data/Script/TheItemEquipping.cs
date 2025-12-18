using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheItemEquipping : MonoBehaviour
{
    public KeyCode action = KeyCode.Mouse0;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(action))
        {
            animator.SetTrigger("Action");
        }
    }
}
