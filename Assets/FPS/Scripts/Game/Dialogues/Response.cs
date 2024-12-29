using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Response
{
    [Tooltip("Text on response button")]
    [SerializeField] string responseText;
    public string ResponseText => responseText;

    [Tooltip("Dialogue which will be started after choosing this response")]
    [SerializeField] DialogueObject dialogueToStart;
    public DialogueObject DialogueToStart => dialogueToStart;

    [Tooltip("If response needs any quest to be available set it here, if not leave it empty")]
    [SerializeField] Quest requiredQuest = null;
    public Quest RequiredQuest => requiredQuest;

    [Tooltip("If response needs any objective to be active set it here, if not leave it empty")]
    [SerializeField] ObjectiveTalkTo requiredObjective = null;
    public ObjectiveTalkTo RequiredObjective => requiredObjective;

    
}
