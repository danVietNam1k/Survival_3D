using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class QuestRow : MonoBehaviour
{
    public TextMeshProUGUI questName, questGiver;
    public Button trackingBTN;
    public bool isTracking, isActive;

    public TextMeshProUGUI coinAmount;
    public Image firstReward, secondReward;
    public TextMeshProUGUI firstRewardName, secondRweardName;
    public Quest quest;
    public Tracker tracker;
    private void Start()
    {
        tracker = QuestManager.Instance.tracker;
    }
    public void StartQuestRow(Quest quest)         //show in QuestMenu

    {
        this.quest = quest;
        isActive = true;
        isTracking = true;
        questName.text = quest.questName;
        questGiver.text = quest.questGiver;
        

        coinAmount.text = $"{quest.info.coinReward}" + " Coin";
        if (quest.info.rewardItem1 != "")
        {
            firstReward.sprite = GetSpriteForItem(quest.info.rewardItem1);
            firstRewardName.text = quest.info.rewardItem1;
        }
        else
        {
            firstReward.gameObject.SetActive(false);
        }
        coinAmount.text = $"{quest.info.coinReward}" + " Coin";
        if (quest.info.rewardItem2 != "")
        {
            secondReward.sprite = GetSpriteForItem(quest.info.rewardItem2);

            secondRweardName.text = quest.info.rewardItem2;
        }
        else
        {
            secondReward.gameObject.SetActive(false);
        }
       
        SetTrackingButton();
    }
    public void SetTrackingButton()
    {
        if (isTracking)
        {
            trackingBTN.GetComponentInChildren<TextMeshProUGUI>().text = "Tracking";
            trackingBTN.onClick.RemoveAllListeners();
            trackingBTN.onClick.AddListener(() =>
            {
                StopTrackingQuest();
                
            });
        }
        else
        {
            trackingBTN.GetComponentInChildren<TextMeshProUGUI>().text = "Start Tracking";
            trackingBTN.onClick.RemoveAllListeners();
            trackingBTN.onClick.AddListener(() =>
            {
                print("press button");
                StartTrackingQuest();
            });
        }
    }

    public void StopTrackingQuest()
    {
        isTracking =false;
        tracker.StopTrackingQuest();
        tracker.currentTrackingQuest = null;
        SetTrackingButton();

    }
    private void StartTrackingQuest()
    {
        isTracking = true;
        QuestManager.Instance.NewTrackingQuestRow(this);
        QuestManager.Instance.SetNewTrackingQuest(quest);
        SetTrackingButton();

    }
    public void FininhQuestRow(Quest quest)
    {
        isActive =  false;
        isTracking = false;
        questName.text = quest.questName;
        questGiver.text = quest.questGiver;
        coinAmount.text = $"{quest.info.coinReward}" + " Coin";
        firstRewardName.text = $"{quest.info.firstRequirementAmount}" + " " + quest.info.firstRequirmentItem;
        secondRweardName.text = quest.info.secondRequirementAmount + " " + quest.info.secondRequirmentItem;
    }
    Sprite GetSpriteForItem(string itemName)
    {
        Sprite image = Resources.Load<GameObject>("Item_Inventory/" + itemName).GetComponent<Image>().sprite;
        return image;
    }
}
