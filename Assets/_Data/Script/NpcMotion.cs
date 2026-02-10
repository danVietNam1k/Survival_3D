using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMotion : MonoBehaviour
{
    public Transform destination;
    public List<Vector3> posDestination;
    NavMeshAgent navMeshAgent;
    public int currentPoit = 0;
    Animator animator;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        LoadDestination();
        animator = transform.Find("Model").GetComponent<Animator>();
    }
    void LoadDestination()
    {
        foreach (Transform t in destination)
        {
            posDestination.Add(t.position);
        }
        navMeshAgent.SetDestination(posDestination[currentPoit]);
    }
    // Update is called once per frame
    void Update()
    {
      
    }
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, posDestination[currentPoit]);
       
        if(distance < 0.5f&& navMeshAgent.isStopped== false)
        {
            navMeshAgent.isStopped = true;
            currentPoit++;
            if(currentPoit>= destination.childCount) currentPoit = 0;
            StartCoroutine(SetNewDestination());
        }
    }

    IEnumerator SetNewDestination()
    {
        animator.SetInteger("Standing", Random.Range(0, 3));
        animator.SetTrigger("Stand");
        yield return new WaitForSeconds(5f);
        navMeshAgent.SetDestination(posDestination[currentPoit]);
        navMeshAgent.isStopped = false;
        animator.SetTrigger("Walking");
    }
}
