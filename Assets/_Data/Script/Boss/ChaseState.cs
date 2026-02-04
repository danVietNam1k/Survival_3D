using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private BossCtl ctl;
    float timeCount;
    public ChaseState(BossCtl ctl)
    {
        this.ctl = ctl;
    }
    public void Enter()
    {
        ctl.agent.SetDestination(ctl.playerPos);
        

    }

    public void Exit()
    {
        
    }

    public void Execute()
    {
        if (ConditionExecute()) return;
        timeCount-= Time.deltaTime;
        ctl.agent.SetDestination(ctl.playerPos);
        float rangeAtk = Vector3.Distance(ctl.transform.position, ctl.player.position);
        if (rangeAtk < 3f && CheckDirectionAttack())
        {
            if (ctl.phase2)
                ctl.ChangeState(ctl.atkCombo);
            else
                ctl.ChangeState(ctl.attackState);

        }
        else if (rangeAtk < 7f&& rangeAtk>3f && timeCount < 0)
        {
            ctl.ChangeState(ctl.jumpState);
            timeCount = 20f;
        }
    }
    bool ConditionExecute()
    {
        if (ctl.notChangeState) return true;
        if (ctl.lostTarget == true) return true;

        return false;

    }
    public bool CheckDirectionAttack()
    {
        if (Physics.Raycast(ctl.transform.position +Vector3.up, ctl.transform.forward, 6f, LayerMask.GetMask("Player")))
        {
            return true;
        }
        return false;
    }
}
