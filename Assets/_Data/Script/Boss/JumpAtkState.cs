using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAtkState : IState
{
    BossCtl ctl;
    float countTime;
    float damge = 50f;
    public JumpAtkState( BossCtl ctl)
    {
        this.ctl = ctl;
    }
    public void Enter()
    {
        ctl.EnterStateAtk(damge);
        ctl.agent.enabled = false;
        ctl.animator.SetTrigger("JumpAtk");
        ctl.triggerDamge.damge = damge;

        countTime = 2.4f;
    }

    public void Execute()
    {
        countTime-= Time.deltaTime;
        if (countTime < 0f)
        {
            countTime = 2f;
            ctl.ChangeState(ctl.chaseState);
        }
    }

    public void Exit()
    {
        ctl.agent.enabled = true;
        ctl.ExitStateAtk();
    }
}
