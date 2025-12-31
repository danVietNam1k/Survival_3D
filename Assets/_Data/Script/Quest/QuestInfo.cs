using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]
public class QuestInfo : ScriptableObject
{
    public List<string> initialDialog;
    public List<AudioClip> initialDialogClips;

    [Header("Options")]
    public string acceptOption;
    public string acceptAnswer;
    public AudioClip acceptAnswerClip;

    public string declineOption;
    public string declineAnswer;
    public AudioClip declineAnswerClip;

    public string comebackAfterDecline;
    public AudioClip comebackAfterDeclineClip;

    public string comebackInProgress;
    public AudioClip comebackInProgressClip;

    public string comebackCompleted;
    public AudioClip comebackCompletedClip;

    public string finalWords;
    public AudioClip finalWordsClip;

    [Header("Rewards")]
    public int coinReward;
    public string rewardItem1;
    public string rewardItem2;

    [Header("Requirements")]
    public string firstRequirmentItem;
    public int firstRequirementAmount;
    public string secondRequirmentItem;
    public int secondRequirementAmount;

    [Header("")]
    public bool hasCheckpoints;
    public List<Checkpoint> checkpoints;

}