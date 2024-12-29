using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.FPS.Gameplay;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [Tooltip("UI text object for description")]
    [SerializeField] TMP_Text descriptionText;

    [Tooltip("UI text object for counter")]
    [SerializeField] TMP_Text counterText;


    Objective _objective;


    private void UpdateText(string description, string counter = "")
    {
        descriptionText.text = description;
        counterText.text = counter;
    }

    public void SetObjective(Objective obj)
    {
        _objective = obj;

        UpdateText(_objective.Description);

        EventManager.AddListener<ObjectiveUpdateEvent>(UpdateObjective);
    }

    private void UpdateObjective(ObjectiveUpdateEvent evt)
    {
        if (evt.Objective == _objective)
        {
            UpdateText(evt.DescriptionText, evt.CounterText);
        }
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener<ObjectiveUpdateEvent>(UpdateObjective);
    }
}
