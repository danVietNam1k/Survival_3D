using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Quest currentTrackingQuest;
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI Requirements;
    private void Start()
    {
        StopTrackingQuest();
    }
    //public void StartTrackingQuest(Quest quest)
    //{
    //    this.currentTrackingQuest = quest;
    //    questName.text = quest.questName;
    //    questDescription.text = quest.description;
    //    Requirements.text = quest.info.firstRequirementAmount+ " " + quest.info.firstRequirmentItem + "\n" +
    //        quest.info.secondRequirementAmount + " " + quest.info.secondRequirmentItem
    //        ;
    //}
    public void StopTrackingQuest()
    {
        currentTrackingQuest = null;
        questName.text = "Not quest tracking";
        questDescription.text = "";
        Requirements.text = "";


    }

}
