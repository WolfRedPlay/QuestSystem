using System;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "Quests")]
public class Quest : ScriptableObject
{
    [Tooltip("Name of the quest")]
    [SerializeField] string title;
    public string Title => title;

    [Tooltip("Quest description")]
    [SerializeField][TextArea] string description;
    public string Description => description;

    [Tooltip("Stages need to be done to finish quest")]
    [SerializeField] List<QuestStage> stages;
    public List<QuestStage> StageList => stages;

    [Tooltip("Quest need to be done to unlock this quest, if it is avaliable from the begining leave it empty")]
    [SerializeField] Quest requiredQuest;
    public Quest RequiredQuest => requiredQuest;


    public QuestState CurrentState { set; get; }



    int _currentStageIndex;
    public int CurrentStageIndex => _currentStageIndex;

    private void ProceedToNextStage(QuestStage stage)
    {
        _currentStageIndex++;
        if (_currentStageIndex == stages.Count) FinishQuest();
        else stages[_currentStageIndex].OnAcceptance();
    }

    public void StartQuest()
    {
        foreach (var stage in stages)
        {
            stage.OnStageCompleted += ProceedToNextStage;
        }
        _currentStageIndex = 0;
        stages[_currentStageIndex].OnAcceptance();

        DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
        displayMessage.Message = Title + " started";
        displayMessage.DelayBeforeDisplay = 0.0f;
        EventManager.Broadcast(displayMessage);
    }

    private void FinishQuest()
    {
        //quest finished
        CurrentState = QuestState.FINISHED;

        QuestFinishedEvent evt = new QuestFinishedEvent();
        evt.FinishedQuest = this;
        EventManager.Broadcast(evt);

        DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
        displayMessage.Message = Title + " finished";
        displayMessage.DelayBeforeDisplay = 0.0f;
        EventManager.Broadcast(displayMessage);

    }

}



public enum QuestState 
{
    LOCKED,
    AVAILABLE,
    ACTIVE,
    FINISHED
}



