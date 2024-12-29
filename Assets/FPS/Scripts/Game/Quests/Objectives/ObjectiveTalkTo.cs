using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ObjectiveTalk", menuName = "Objectives/ObjectiveTalk")]
public class ObjectiveTalkTo : Objective
{
    [Tooltip("Dialogue needs to be finished to complete objective")]
    [SerializeField] DialogueObject requiredDialogue;
    public DialogueObject RequiredDialogue => requiredDialogue;


    bool _isTaken = false;
    public bool IsTaken => _isTaken;

    public override void OnAcceptance()
    {
        base.OnAcceptance();

        _isTaken = true;

        requiredDialogue.OnDialogueFinished +=() => CompleteObjective(string.Empty, string.Empty, "Objective complete ");
        requiredDialogue.OnDialogueFinished += SetNotActive;


    }

    public void SetNotActive()
    {
        _isTaken = false;
    }

    void OnDestroy()
    {
        requiredDialogue.OnDialogueFinished -= () => CompleteObjective(string.Empty, string.Empty, "Objective complete ");
        requiredDialogue.OnDialogueFinished -= SetNotActive;

    }
}
