using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossCtl : MonoBehaviour
{
    [Header("debug")]
    public float radiusGizmos;
    [Header("Parameter")]
    public IState currentState;
    public GameObject fxPhase2;
    public NavMeshAgent agent;
    private StateMachine stateMachine;
    public float currentSpeed = 0f;
    public Animator animator;
    public Transform player;
    public TriggerDamge triggerDamge;
    public AudioSource AudioSource;
    private BossStateInfo stateInfo;
    //State
    public IdleState idleState;
    public AttackState attackState;
    public ChaseState chaseState;
    public PatrolState patrolState;
    public AtkCombo atkCombo;
    public JumpAtkState jumpState;
    // condition
    public bool lostTarget = false;
    public bool notChangeState = false;
    public bool phase2 = false;
    // Target position
    [HideInInspector] public Vector3 playerPos;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateInfo = GetComponentInChildren<BossStateInfo>();
        player = ReferenceManager.Instance.player;
        animator = this.GetComponent<Animator>();
        SetUpState();
        stateMachine.Initialize(idleState);


    }
    private void Awake()
    {
        stateMachine = new StateMachine(this);
        
    }
    private void Update()
    {
        print(currentState?.ToString());
        BehaviorEnemy();
        stateMachine.Execute();
        SetSpeedAnim();
    }
    void SetSpeedAnim()
    {
        currentSpeed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("move", currentSpeed);
    }

    public void ChangeState(IState state)
    {
        if (currentState == state || state == null) return;
        currentState = state;
        stateMachine.ChangeState(state);

    }

    private void BehaviorEnemy()
    {
        if (notChangeState) return;
        if (CanSeePlayer())
        {
            if (lostTarget) lostTarget = false;
            playerPos = player.position;
            ChangeState(chaseState);
        }
        else
        {
            if (!lostTarget) lostTarget = true;
            ChangeState(idleState);


        }

    }
    private bool CanSeePlayer()
    {
        Ray ray = new Ray(this.transform.position, player.position - this.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
            if (hitInfo.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    void SetUpState()
    {
        idleState = new IdleState(this);
        attackState = new AttackState(this);
        chaseState = new ChaseState(this);
        patrolState = new PatrolState(this);
        atkCombo = new AtkCombo(this);
        jumpState = new JumpAtkState(this);
    }
    public void ExitStateAtk()
    {
       agent.isStopped = false;
       notChangeState = false;
       triggerDamge.damge = 0;
    }
    public void EnterStateAtk(float damge)
    {
        if(agent.enabled)
        {
            agent.isStopped = true;
            notChangeState = true;
        }
        
        triggerDamge.damge = damge;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, radiusGizmos);
    }
    public void TakeDamge(float damge) => stateInfo.TakeDamge(damge);
    public void TurnOnFxPhase2()
    {
        fxPhase2.SetActive(true);

    }
}