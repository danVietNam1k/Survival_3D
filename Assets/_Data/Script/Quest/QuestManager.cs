using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }

    public List<Quest> allActiveQuest;
    public List<Quest> allCompletedQuests;

    [Header("--------QuestMenu--------")]
    public GameObject questMenu;
    public bool isQuestMenuOpen;
    public GameObject activeQuestPrefab;
    public GameObject completedQuestPrefab;
    public GameObject questMenuContent;

    [Header("------------QuestTracker-----------")]
    public Tracker tracker;
    public GameObject questTrackerContent;
    public QuestRow isTrackingPrevious;
    public int firstRequirementAmountInventory = 0;
    public int secondRequirementAmountInventory = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddActiveQuest(Quest quest)
    {
        allActiveQuest.Add(quest);
        RefreshQuestList();
    }
    public void MarkQuestCompleted(Quest quest)
    {
        allActiveQuest.Remove(quest);

        allCompletedQuests.Add(quest);
        RefreshQuestList();

    }
    
    public void NewTrackingQuestRow(QuestRow newQuestRow)
    {
        if (isTrackingPrevious == newQuestRow) return;
        isTrackingPrevious.isTracking = false;
        isTrackingPrevious.SetTrackingButton();
        isTrackingPrevious = newQuestRow;
    }
    public void RefreshQuestList()
    {
        foreach (Transform child in questMenuContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Quest activeQuest in allActiveQuest)
        {
            if(isTrackingPrevious != null)
            {
                isTrackingPrevious.isTracking = false;
                isTrackingPrevious.SetTrackingButton();
            }
            GameObject questPrefab = Instantiate(activeQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);
            QuestRow qRow = questPrefab.GetComponent<QuestRow>();
            qRow.StartQuestRow(activeQuest);
            isTrackingPrevious = qRow;
            SetNewTrackingQuest(activeQuest);
        }

        foreach (Quest completedQuest in allCompletedQuests)
        {
            GameObject questPrefab = Instantiate(completedQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);
            QuestRow qRow = questPrefab.GetComponent<QuestRow>();

            qRow.FininhQuestRow(completedQuest);
            
            tracker.StopTrackingQuest();

        }

    }
    public void TrackingQuest(Quest quest)
    {
        tracker.questName.text = quest.questName;
        tracker.questDescription.text = quest.description;
        if(quest.info.secondRequirmentItem != "")
        {
            tracker.Requirements.text = firstRequirementAmountInventory+"/" +quest.info.firstRequirementAmount + " "
                + quest.info.firstRequirmentItem + "\n" + secondRequirementAmountInventory+ "/"+
           quest.info.secondRequirementAmount + " " + quest.info.secondRequirmentItem;
        }
        else if(quest.info.firstRequirmentItem != "")
        {
            tracker.Requirements.text = firstRequirementAmountInventory + "/" + quest.info.firstRequirementAmount + " "
                + quest.info.firstRequirmentItem;
        }
        else
        {
            tracker.Requirements.text = "";
        }
        if (quest.info.hasCheckpoints)
        {
            var existingText = tracker.Requirements.text;
            tracker.Requirements.text = PrintCheckpoints(quest, existingText);
        }
            
    }

    public void SetNewTrackingQuest(Quest newQuest)
    {
        tracker.currentTrackingQuest = newQuest;
        RefreshTrackerAmountItem();
    }

    public void RefreshTrackerAmountItem() //
    {
        if (tracker.currentTrackingQuest == null) return;
        firstRequirementAmountInventory = 0;
        secondRequirementAmountInventory = 0;
        
        QuestInfo quest = tracker.currentTrackingQuest.info;
        foreach (var item in InventorySystem.Instance.itemList)
        {
            if(quest.firstRequirmentItem == item)
            {
                firstRequirementAmountInventory++;
            }
            if(quest.secondRequirmentItem == item) 
            { secondRequirementAmountInventory++; }
        }
        TrackingQuest(tracker.currentTrackingQuest);
    }
    private string PrintCheckpoints(Quest trackedQuest, string exisrtingText)
    {
        var finalText = exisrtingText;

        foreach (Checkpoint cp in trackedQuest.info.checkpoints)
        {
            if (cp.isCompleted)
            {
                finalText = finalText + "\n" + cp._name + "[Completed]";
            }
            else
            {
                finalText = finalText + "\n" + cp._name;
            }
        }
        return finalText;
    }
   
}
