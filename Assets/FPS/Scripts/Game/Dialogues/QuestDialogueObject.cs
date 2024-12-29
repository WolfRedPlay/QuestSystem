using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "QuestDialogue", menuName = "Dialogues/Quest dialogue")]
public class QuestDialogueObject : DialogueObject
{
    [Tooltip("Quest which will be given at the end of the dialogue")]
    [SerializeField] Quest questToGive;
    public Quest QuestToGive => questToGive;

    public override void FinishDialogue()
    {
        base.FinishDialogue();
        GameObject.FindAnyObjectByType<QuestsManager>().StartQuest(questToGive);
        questToGive.CurrentState = QuestState.ACTIVE;
    }
}
