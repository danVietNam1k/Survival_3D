using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugTreeCheckpoint : MonoBehaviour
{
   public Checkpoint checkpoint;
   public QuestInfo questInfo;

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (questInfo.hasCheckpoints)
            {
                checkpoint.isCompleted = true;
            }
        }
    }
}

