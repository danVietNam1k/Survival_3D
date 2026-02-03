using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.GlobalVariables;

public class IdleState : IState
{
    private BossCtl ctl;
    private float countTime;
    public IdleState(BossCtl ctl)
    {
        this.ctl = ctl;
    }
    public void Enter()
    {
        countTime = 2f;

    }

    public void Exit()
    {
    }

    public void Execute()
    {
        countTime-=Time.deltaTime;
        if (countTime < 2f)
        {
            ctl.ChangeState(ctl.patrolState);
        }

    }
}
