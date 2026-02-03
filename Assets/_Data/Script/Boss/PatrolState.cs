using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private BossCtl ctl;
    float countTime;
    public PatrolState(BossCtl ctl)
    {
        this.ctl = ctl;
    }
    public void Enter()
    {
        countTime = 5f;
    }

    public void Exit()
    {

    }

    public void Execute()
    {
        countTime -= Time.deltaTime;
        if (countTime < 0) {
            countTime = 5f;
            ctl.agent.SetDestination(NewPosPatrol());
        }

    }
    private Vector3 NewPosPatrol() {
       Vector3 pos = ctl.transform.position;
        pos.x = Random.Range(pos.x + 5f, pos.x - 5f);
        pos.z = Random.Range(pos.z + 5f, pos.z - 5f);

        return pos;


    }
}
