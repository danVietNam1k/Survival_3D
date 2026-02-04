using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCombo : IState
{
    BossCtl ctl;
    
    float damgeCB = 30f;
   
    float countTimeCB;

    public AtkCombo(BossCtl ctl)
    {
        this.ctl = ctl;
    }
    public void Enter()
    {
        ComboStart();
    }

    public void Exit()
    {
      ctl.ExitStateAtk();
    }
    public void Execute()
    {
        countTimeCB-=Time.deltaTime;
        if (countTimeCB < 0) ctl.ChangeState(ctl.chaseState);
    }
    void ComboStart()
    {
        countTimeCB = 5f;
        ctl.EnterStateAtk(damgeCB);
        int i = Random.Range(0, 2);
        ctl.animator.SetInteger("comboType", i);
        ctl.animator.SetTrigger("combo");
    }
  
}
