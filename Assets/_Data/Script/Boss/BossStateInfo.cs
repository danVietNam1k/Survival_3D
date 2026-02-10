using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStateInfo : MonoBehaviour
{
    BossCtl bossCtl;
    public float maxHealth;
    public float maxHealthPersenPhase2;
    float currentHealth;
    bool phase2 = false;
    bool dead = false;
    public Slider healthSlider;
    public GameObject checkPoint;
    void Start()
    {
        bossCtl = this.GetComponentInParent<BossCtl>();
        currentHealth = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = currentHealth/maxHealth;
    
    }
    public void TakeDamge(float damge)
    {
        currentHealth -= damge;
        if (currentHealth <= 0)
        {
            if (phase2 == false)
            {
                phase2 = true;
                bossCtl.phase2 = phase2;
                currentHealth = (maxHealth / 100) * maxHealthPersenPhase2;
                bossCtl.animator.SetTrigger("Phase2");
                StartCoroutine(WaitStartPhase2());
            }
            else
            {
                 BossTobeDead();
            }


        }
    }
    public bool IsDead() { 
    return dead;    
    }
    IEnumerator WaitStartPhase2()
    {
        bossCtl.agent.isStopped = true;
        bossCtl.notChangeState =true;
        yield return new WaitForSeconds(2.7f);
        bossCtl.notChangeState = false;
        bossCtl.agent.isStopped = false;
       
    }
    void BossTobeDead()
    {
        if (dead) return;
        checkPoint.SetActive(true);
        dead = true;
        currentHealth = 0;
        bossCtl.animator.SetTrigger("Dead");
        bossCtl.enabled = false;
        bossCtl.agent.enabled = false;
        bossCtl.triggerDamge.gameObject.SetActive(false);
        bossCtl.transform.Find("UI").gameObject.SetActive(false);
        bossCtl.transform.Find("FX").gameObject.SetActive(false);
    }
  
}
