using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogues/Dialogue")]
public class DialogueObject : ScriptableObject
{
    [Tooltip("All phrases will be shown one by one")]
    [TextArea][SerializeField] string[] dialogueLine;
    public string[] DialogueLine => dialogueLine;
    
    [Tooltip("All responses for the dialogue")]
    [SerializeField] List<Response> responses;
    public List<Response> Responses => responses;


    public UnityAction OnDialogueFinished;

    public virtual void FinishDialogue()
    {
        OnDialogueFinished?.Invoke();
    }
}
