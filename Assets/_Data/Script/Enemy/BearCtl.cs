using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class BearCtl : MonoBehaviour
{
    [Header("------------Bool--------")]

    public bool targetDetection = false;
    bool canAttack = true;
    public bool thisCanAttack = true;
    [Header("----------other---------")]
    Animator animator;
    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    public Transform atkColider;
    public float walkSpeed = 2f;
    public float timeWalking = 0f;

    float speed = 0;

    public float attackingDistance = 5f;
    public float startChasingDistance = 10f;
    public float stopChasingDistance = 20f;
    public Transform target = null;
    public int numberOfAnimAtk;


    public float distaneRamPos = 10f;
    Vector3 newPos;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        animator = GetComponent<Animator>();
        if (target == null) { target = ReferenceManager.Instance.player.transform; }

    }
    private void OnEnable()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        State();

    }
    void State()
    {
        if (GetComponent<Animal>().isDeath)
        {
            agent.isStopped = true;
            return; }
        CheckState();
        Movement();
        SetAnimMove();
        SwitchNavMesh();
    }
   public void SetTarget()
    {
        if(agent.enabled == true) 
        agent.SetDestination(target.transform.position);
    }
   
    void Movement()
    {
        
        
        timeWalking -= Time.deltaTime;
        if (targetDetection && canAttack)
        { float distanceChasing = Vector3.Distance(transform.position, target.position);
            
            if (distanceChasing < attackingDistance)
            {
                Attack();
            }
            if (!agent.enabled) return;
            agent.isStopped = false;
            Invoke(nameof(SetTarget), 0.5f);
        }
        else if (timeWalking < 0)
        {
            if (!agent.enabled) return;
            agent.isStopped = false;
            agent.SetDestination(RandomPosDestination());
            timeWalking = 20f;
        }
    }
    void DelayAtack()
    {
        canAttack = true;
    }
    void SwitchNavMesh()
    {
        if (canAttack && obstacle.enabled)
        {
            obstacle.enabled = false;
            agent.enabled = true;
        }
        else if(!canAttack && agent.enabled)
        {
            agent.enabled = false;
            obstacle.enabled = true;
           
        }
    }
    void Attack()
    {
        if (!thisCanAttack) return;
        Vector3 rayStart = this.transform.position;
        rayStart.y += 0.5f;
        if (Physics.SphereCast(rayStart, 1f,transform.forward, out RaycastHit infor, 1f) && infor.transform.CompareTag("Player"))
        {
            StartCoroutine(AtkRangeTurnOn());
            agent.isStopped = true;
            agent.updateRotation = true;
            animator.SetTrigger("AttackTrigger");
            int i = Random.Range(0, numberOfAnimAtk);
            animator.SetFloat("Attack", (float)i);
            GetComponent<Animal>().PlaySound("Attack");
            canAttack = false;
            Invoke("DelayAtack", 2f);
            print(infor.transform.tag);
       
        }
    }
    IEnumerator AtkRangeTurnOn()
    {
        yield return new WaitForSeconds(0.2f);
        atkColider.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.25f);
        atkColider.gameObject.SetActive(false);

    }
    void CheckState()
    {
        if (Vector3.Distance(transform.position, target.position)< startChasingDistance && !targetDetection)
        {
            targetDetection = true;
        }else if(Vector3.Distance(transform.position,target.position)>stopChasingDistance && targetDetection)
        {
            targetDetection = false;
        }

        if (targetDetection)
        {
            float speedRun = walkSpeed * 4;
            if (speed < speedRun)
            {
                speed += Time.deltaTime;
            }
        }
        else 
        {
            if (Vector3.Distance(this.transform.position, newPos) > 0.2f)
                speed = walkSpeed;   
        }
       
            agent.speed = speed;
        
       
    }
    void SetAnimMove()
    {
        animator.SetFloat("SpeedMovement", agent.velocity.magnitude / walkSpeed);
        
        
    }
    Vector3 RandomPosDestination()
    {
        
        Vector3 pos =this.transform.position;
        pos.x = Random.Range(pos.x - distaneRamPos,pos.x + distaneRamPos);
        pos.z = Random.Range(pos.z - distaneRamPos,pos.z + distaneRamPos);
        
        newPos = pos;
        // pos = Random.insideUnitSphere * distance;
        return newPos;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackingDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, startChasingDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopChasingDistance);
    }
  
}
