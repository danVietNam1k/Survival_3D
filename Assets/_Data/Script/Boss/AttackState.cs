using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private BossCtl ctl;
    float countTimeState;
    float damgeNomarl = 20f;
    float damgeHeavy = 30f;
    public AttackState(BossCtl ctl)
    {
        this.ctl = ctl;
    }
    public void Enter()
    {
        AttackStart();
      
        
    }

    public void Exit()
    {
       ctl.ExitStateAtk();
    }

    public void Execute()
    {
        countTimeState -=Time.deltaTime;
        if (countTimeState < 0) 
        {
            ctl.ChangeState(ctl.chaseState); 
        
        }
    }
    void AttackStart()
    {
        float random = Random.Range(0, 2f);
        if(random > 1.5f)
            HeavyAttackStart();
        else 
            NomarlAttackStart();
    }
    void HeavyAttackStart()
    {
      int ran =  Random.Range(0, 3);
      Animator animator = ctl.animator;
        animator.SetInteger("atkHeavyType", ran);
        animator.SetTrigger("atkHeavy");
        ctl.EnterStateAtk(damgeHeavy);
        countTimeState = 4f;
    }
    void NomarlAttackStart()
    {
        int ran = Random.Range(0, 3);
        Animator animator = ctl.animator;
        animator.SetFloat("atkType1", ran);
        animator.SetTrigger("atkNormal");
        ctl.EnterStateAtk(damgeNomarl);
        countTimeState = 3f;
    }

}
