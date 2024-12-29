using System;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;


[Serializable]
public class QuestStage
{
    [Tooltip("Delay before the objective becomes visible")]
    public float DelayVisible;

    [Tooltip("Name of the stage")]
    public string Title;

    [Tooltip("Objectives need to be done to finish stage")]
    public List<Objective> Objectives;

    public event Action<QuestStage> OnStageStarted;
    public event Action<QuestStage> OnStageCompleted;


    public void CheckTasksCompletion(Objective objectiveToCheck)
    {
        bool checker = true;

        foreach (Objective objective in Objectives) 
        {
            if (!objective.IsCompleted)
            {
                checker = false; 
                break; 
            }
        }

        if (checker) 
        {
            FinishStage();
        }
    }

    private void FinishStage()
    {
        OnStageCompleted?.Invoke(this);
        OnStageStarted = null;
        OnStageCompleted = null;
    }

    public void OnAcceptance()
    {        
        StageStartedEvent stageStarted = Events.StageStartedEvent;
        stageStarted.Stage = this;
        EventManager.Broadcast(stageStarted);

        OnStageStarted?.Invoke(this);

        foreach (var obj in Objectives)
        {
            obj.OnObjectiveCompleted += CheckTasksCompletion;
            obj.OnAcceptance();
        }
    }
}


