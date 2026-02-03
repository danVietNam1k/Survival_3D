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
        if(ctl.isAttacking) return;
        timeCount-= Time.deltaTime;
        if (ctl.lostTarget == true) return;
        ctl.agent.SetDestination(ctl.playerPos);
        float rangeAtk = Vector3.Distance(ctl.transform.position, ctl.player.position);
        if (rangeAtk < 3f && CheckDirectAttack())
        {

            ctl.ChangeState(ctl.attackState);

        }
        else if (rangeAtk < 7f&& rangeAtk>3f && timeCount < 0)
        {
            ctl.ChangeState(ctl.jumpState);
            timeCount = 20f;
        }
    }
    public bool CheckDirectAttack()
    {
        if (Physics.Raycast(ctl.transform.position +Vector3.up, ctl.transform.forward, 6f, LayerMask.GetMask("Player")))
        {
            return true;
        }
        return false;
    }
}
