using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Tooltip("Root object of dialogue box")]
    [SerializeField] GameObject dialogueBox;

    [Tooltip("Text object where phrases will be shown")]
    [SerializeField] TMP_Text textField;


    ResponsesHandler _responseHandler;
    public bool IsActive => dialogueBox.activeSelf;

    private void Start()
    {
        _responseHandler = FindObjectOfType<ResponsesHandler>(true);
        DebugUtility.HandleErrorIfNullFindObject<ResponsesHandler, DialogueUI>(_responseHandler, this);

        EventManager.AddListener<DialogueStartedEvent>(ShowDialogue);
    }


    private void ShowDialogue(DialogueStartedEvent evnt)
    {
        // check if quest is available
        if (evnt.StartedDialogue is QuestDialogueObject &&
            (evnt.StartedDialogue as QuestDialogueObject).QuestToGive.CurrentState != QuestState.AVAILABLE)
            return;

        OpenDialogueBox();
        StartCoroutine(StepThroughDialogue(evnt.StartedDialogue));
    }

    IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogueLine in dialogueObject.DialogueLine)
        {
            textField.text = dialogueLine;
            yield return new WaitForSecondsRealtime(.5f);
            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        dialogueObject.FinishDialogue();

        if (dialogueObject.Responses.Count != 0) 
        {
            _responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
    }


    private void OpenDialogueBox()
    {
        dialogueBox.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void CloseDialogueBox()
    {
        textField.text = string.Empty;
        dialogueBox.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener<DialogueStartedEvent>(ShowDialogue);
    }
}
