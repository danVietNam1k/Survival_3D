using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
        ctl.agent.enabled = false;
        ctl.animator.SetTrigger("JumpAtk");

        countTime = 2.4f;
        ctl.EnterStateAtk(damge);
        Debug.Log("enter jumpState");

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
    IEnumerator WaitAtk()
    {
        yield return new WaitForSeconds(1.1f);
        ctl.EnterStateAtk(damge);
    }
}
