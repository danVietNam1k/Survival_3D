using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkCombo : IState
{
    BossCtl ctl;
    int comboNumber = 0;
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
        ctl.transform.LookAt(ctl.player);
        countTimeCB-=Time.deltaTime;
        if (countTimeCB < 0) ctl.ChangeState(ctl.chaseState);
    }
    void ComboStart()
    {
        countTimeCB = 5f;
        ctl.EnterStateAtk(damgeCB);
        Animator anim = ctl.animator;
        anim.SetInteger("Combo", NewCombo());
        anim.Play("Combo");
    }
    int NewCombo()
    {
        return Random.Range(0, comboNumber);
    }
}
