using System.Collections.Generic;
using TMPro;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class ResponsesHandler : MonoBehaviour
{
    [Tooltip("Root object of response box")]
    [SerializeField] RectTransform responseBox;

    [Tooltip("Prefab for response buttons")]
    [SerializeField] GameObject responsePrefab;

    public void ShowResponses(List<Response> responsesToShow)
    {
        foreach (Response response in responsesToShow)
        {
            if (response.RequiredQuest != null || response.RequiredObjective != null)
            {
                if ((response.RequiredQuest != null && response.RequiredQuest.CurrentState == QuestState.AVAILABLE) ||
                        (response.RequiredObjective != null && response.RequiredObjective.IsTaken))
                {
                    AddResponse(response);

                }
            }
            else
            {
                AddResponse(response);
            }
        }
        
        responseBox.gameObject.SetActive(true);
    }

    private void AddResponse(Response response)
    {
        GameObject newResponse = Instantiate(responsePrefab, responseBox);
        newResponse.GetComponentInChildren<TMP_Text>().text = response.ResponseText;
        newResponse.GetComponent<Button>().onClick.AddListener(() => OnResponsePicked(response));
    }

    public void OnResponsePicked(Response chosenResponse)
    {
        DialogueStartedEvent evt = new DialogueStartedEvent();
        evt.StartedDialogue = chosenResponse.DialogueToStart;
        EventManager.Broadcast(evt);

        foreach(RectTransform child in responseBox)
        {
            Destroy(child.gameObject);
        }

        responseBox.gameObject.SetActive(false);
    }
}
