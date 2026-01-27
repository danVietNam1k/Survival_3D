using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    public string nameCutscene;
    private void OnValidate()
    {
        this.name = nameCutscene;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           CutSceneManager.Instance.PlayCutscene(nameCutscene);
            
        }
    }
}
