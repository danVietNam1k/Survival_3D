using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Quest
{
    public string questGiver;
    public string questName;
    public string description;

    [Header("------------Bools-------------")]
    public bool accepted;
    public bool declined;
    public bool initialDialogCompleted;

    public bool isCompleted;
    public bool hasNoRequirement;

    [Header("------------Quest Infor------------------")]
    public QuestInfo info;

}
