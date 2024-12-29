using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

public class QuestsManager : MonoBehaviour
{
    [Tooltip("All created quests")]
    [SerializeField] List<Quest> allQuests;
    public List<Quest> Quests => allQuests;

    [Tooltip("Active quests")]
    [SerializeField] List<Quest> activeQuests;

    private void Start()
    {
        if (activeQuests == null) activeQuests = new List<Quest>();
        else
            foreach(Quest quest in activeQuests)
                quest.StartQuest();


        foreach (Quest quest in allQuests)
        {
            if (quest.RequiredQuest == null)
                quest.CurrentState = QuestState.AVAILABLE;
            else
                quest.CurrentState = QuestState.LOCKED;
            UpdateTalkObjective(quest);
        }

        EventManager.AddListener<QuestFinishedEvent>(OnQuestFinished);
    }

    private void UpdateTalkObjective(Quest quest)
    {
        foreach (QuestStage stage in quest.StageList)
        {
            foreach (Objective obj in stage.Objectives)
            {
                if (obj is ObjectiveTalkTo) (obj as ObjectiveTalkTo).SetNotActive();
            }
        }
    }


    private void OnQuestFinished(QuestFinishedEvent evt)
    {
        activeQuests.Remove(evt.FinishedQuest);
        CheckForAvailability(evt.FinishedQuest);
    }

    private void CheckForAvailability(Quest finishedQuest)
    {
        foreach (Quest quest in allQuests)
        {
            if (quest.RequiredQuest == finishedQuest) quest.CurrentState = QuestState.AVAILABLE;
        }
    }


    public void StartQuest(Quest questToAdd)
    {
        activeQuests.Add(questToAdd);
        questToAdd.StartQuest();
    }
}
