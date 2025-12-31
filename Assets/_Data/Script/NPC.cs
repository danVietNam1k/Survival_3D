using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    GameObject canvasDialog;
    TextMeshProUGUI npcDialogText, answearBTN1Text, answearBTN2Text, answearBTN3Text;
     Button answearBTN1, answearBTN2, answearBTN3;
    public int currentDialog = 0;
    public List<Quest> quests = new();
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;



    // Start is called before the first frame update
    void Start()
    {
        npcDialogText = DialogSystem.Instance.dialogText;
        answearBTN1 = DialogSystem.Instance.answearBTN1;
        answearBTN1Text = answearBTN1.transform.GetComponentInChildren<TextMeshProUGUI>();
        answearBTN2 = DialogSystem.Instance.answearBTN2;
        answearBTN2Text = answearBTN2.transform.GetComponentInChildren<TextMeshProUGUI>();
        answearBTN3 = DialogSystem.Instance.answearBTN3;
        answearBTN3Text = answearBTN3.transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlaySoundConvesation(AudioClip clip)
    {
        SoundManager.Instance.PlaySoundNPC(clip, this.transform);
    }
    public void StartConversation()
    {
        LookAtPlayer();
        if (firstTimeInteraction) // interactiong with the npc for the first time
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex];
            StartQuestInitialDialog();
            currentDialog = 0;
        }
        else// interactiong with the npc after the first time
        {
            if (currentActiveQuest.declined)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.comebackAfterDecline;
                PlaySoundConvesation(currentActiveQuest.info.comebackAfterDeclineClip);

                AcceptAndDeclineRequired();
            }


            if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
            {
                if (AreQuestRequirmentsCompleted())
                {
                    SubmitRequiredItems();
                    DialogSystem.Instance.OpenDialogUI();
                    npcDialogText.text = currentActiveQuest.info.comebackCompleted;
                    PlaySoundConvesation(currentActiveQuest.info.comebackCompletedClip);

                    TakeReward();

                }
                else
                {
                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackInProgress;
                    PlaySoundConvesation(currentActiveQuest.info.comebackInProgressClip);

                    CloseDialog();
                }
            }
            else if(activeQuestIndex >= quests.Count)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.finalWords;
                PlaySoundConvesation(currentActiveQuest.info.finalWordsClip);

                CloseDialog();
            }

        }
    }

    private void SubmitRequiredItems()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirmentItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        if(firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);
        }
        string seconRequiredItem = currentActiveQuest.info.secondRequirmentItem;
        int secondRequiredItem = currentActiveQuest.info.secondRequirementAmount;
        if(firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(seconRequiredItem, secondRequiredItem);
        }
    }

    private bool AreQuestRequirmentsCompleted()
    {
        //first item requirment
        string firstRequiredItem = currentActiveQuest.info.firstRequirmentItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        var firstItemCouter = 0;
   
        string seconRequiredItem = currentActiveQuest.info.secondRequirmentItem;
        int secondRequiredItem = currentActiveQuest.info.secondRequirementAmount;
        var secondItemCouter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == firstRequiredItem)
            {
                firstItemCouter++;
            }
            if (item == seconRequiredItem)
            {
                secondItemCouter++;
            }
          
        }
        SetQuestHasCheckpoints(currentActiveQuest);


        bool allCheckpointsCompleted= false;
        if (currentActiveQuest.info.hasCheckpoints)
        {
            foreach(Checkpoint cp in currentActiveQuest.info.checkpoints)
            {
                if(cp.isCompleted == false)
                {
                    allCheckpointsCompleted = false;
                    break;
                }
                allCheckpointsCompleted = true; 
            }
        }
        if (firstItemCouter >= firstRequiredAmount && secondItemCouter >= secondRequiredItem)
        {
            if(currentActiveQuest.info.hasCheckpoints)
            {
               
                    if (allCheckpointsCompleted)
                    {
                        return true;
                    }
                    else
                    {
                        return false;   
                    }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    void SetQuestHasCheckpoints(Quest currentQuest)
    {
        if (currentActiveQuest.info.checkpoints.Count > 0)
        {
            currentQuest.info.hasCheckpoints = true;
        }
        else
        {
            currentQuest.info.hasCheckpoints = false;
        }
    }
    void AcceptAndDeclineRequired()
    {

        answearBTN1Text.text = currentActiveQuest.info.acceptOption;
       
        answearBTN1.onClick.RemoveAllListeners();
        answearBTN1.onClick.AddListener(() =>
        {
            AccetedQuest();
        });
        answearBTN2.gameObject.SetActive(true);

        answearBTN2Text.text = currentActiveQuest.info.declineOption;
        answearBTN2.onClick.RemoveAllListeners();
        answearBTN2.onClick.AddListener(() =>
        {
            DeclinedQuest();
        });
    }

    void CheckIfDialogDone()
    {
        if(currentDialog == currentActiveQuest.info.initialDialog.Count - 1)
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            PlaySoundConvesation(currentActiveQuest.info.initialDialogClips[currentDialog]);


            currentActiveQuest.initialDialogCompleted = true;

            AcceptAndDeclineRequired();
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            PlaySoundConvesation(currentActiveQuest.info.initialDialogClips[currentDialog]);

            answearBTN1Text.text = "Next";
            answearBTN1.onClick.RemoveAllListeners();
            answearBTN1.onClick.AddListener(() =>
            {
                currentDialog++;
                CheckIfDialogDone();

            });
        }
    }
    private void StartQuestInitialDialog()
    {
        DialogSystem.Instance.OpenDialogUI();
        npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
        PlaySoundConvesation(currentActiveQuest.info.initialDialogClips[currentDialog]);
        answearBTN1Text.text = "Next";
        answearBTN1.onClick.RemoveAllListeners();
        answearBTN1.onClick.AddListener(() =>
        {
            currentDialog++;
            CheckIfDialogDone();

        });
        answearBTN2.gameObject.SetActive(false);
        answearBTN3.gameObject.SetActive(false);
    }

    private void AccetedQuest()
    {
        QuestManager.Instance.AddActiveQuest(currentActiveQuest);

        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;
        if (currentActiveQuest.hasNoRequirement)
        {
            npcDialogText.text = currentActiveQuest.info.comebackCompleted;
            PlaySoundConvesation(currentActiveQuest.info.comebackCompletedClip);

            TakeReward();
            answearBTN2.gameObject.SetActive(false);
        }else
        {
            npcDialogText.text = currentActiveQuest.info.acceptAnswer;
            PlaySoundConvesation(currentActiveQuest.info.acceptAnswerClip);


            CloseDialog();


            answearBTN2.gameObject.SetActive(false);


        }
    }
    void TakeReward()
    {
        
        answearBTN1Text.text = "Take Reward";
        answearBTN1.onClick.RemoveAllListeners();
        answearBTN1.onClick.AddListener(() =>
        {
            ReceiveRewardAndCompleteQuest();
            DialogSystem.Instance.CloseDialogUI();

        });
    }
    void CloseDialog()
    {
        answearBTN1Text.text = "Close";
        answearBTN1.onClick.RemoveAllListeners();
        answearBTN1.onClick.AddListener(() =>
        {
            DialogSystem.Instance.CloseDialogUI();
        });
    }

    private void ReceiveRewardAndCompleteQuest()
    {
        QuestManager.Instance.MarkQuestCompleted(currentActiveQuest);
        currentActiveQuest.isCompleted = true;
        var coinsRecievec = currentActiveQuest.info.coinReward;
        
        if(currentActiveQuest.info.rewardItem1 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1);
        }
        if (currentActiveQuest.info.rewardItem2 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2);
        }
        activeQuestIndex++;
        // start next quest
        if(activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            currentDialog = 0;
            DialogSystem.Instance.CloseDialogUI();
        }
        else
        {
            DialogSystem.Instance.CloseDialogUI();

        }
    }

    private void DeclinedQuest()
    {
        currentActiveQuest.declined = true;

        npcDialogText.text = currentActiveQuest.info.declineAnswer;
        PlaySoundConvesation(currentActiveQuest.info.declineAnswerClip);

        answearBTN1Text.text = "Close";
        answearBTN1.onClick.RemoveAllListeners();
        answearBTN1.onClick.AddListener(() =>
        {
            DialogSystem.Instance.CloseDialogUI();
        });
        answearBTN2.gameObject.SetActive(false);
    }
    public void LookAtPlayer()
    {
        var player = ReferenceManager.Instance.player;
        //transform.LookAt(player.position);

        Vector3 direction = player.position - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);
        var yRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    IEnumerator Conversation()
    {

        yield return null;
    }
}
