using System;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    //----NEW-----
    [Serializable]
    public abstract class Objective : ScriptableObject //----NEW-----
    {
        //[Tooltip("Name of the objective that will be shown on screen")]
        //public string Title;

        [Tooltip("Short text explaining the objective that will be shown on screen")]
        public string Description;

        //[Tooltip("Whether the objective is required to win or not")]
        //public bool IsOptional;

        public bool IsCompleted { get; private set; }
        //public bool IsBlocking() => !(IsOptional || IsCompleted);


        

        public Action OnObjectiveCreated;
        public Action<Objective> OnObjectiveCompleted;

        public virtual void OnAcceptance()
        {
            OnObjectiveCreated?.Invoke();

            //DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
            //displayMessage.Message = Title;
            //displayMessage.DelayBeforeDisplay = 0.0f;
            //EventManager.Broadcast(displayMessage);
            IsCompleted = false;
        }

        public void UpdateObjective(string descriptionText, string counterText, string notificationText)
        {
            ObjectiveUpdateEvent evt = Events.ObjectiveUpdateEvent;
            evt.Objective = this;
            evt.DescriptionText = descriptionText;
            evt.CounterText = counterText;
            evt.NotificationText = notificationText;
            evt.IsComplete = IsCompleted;
            EventManager.Broadcast(evt);
        }

        public void CompleteObjective(string descriptionText, string counterText, string notificationText)
        {
            IsCompleted = true;

            ObjectiveUpdateEvent evt = Events.ObjectiveUpdateEvent;
            evt.Objective = this;
            evt.DescriptionText = descriptionText;
            evt.CounterText = counterText;
            evt.NotificationText = notificationText;
            evt.IsComplete = IsCompleted;
            EventManager.Broadcast(evt);

            OnObjectiveCompleted?.Invoke(this);
        }
    }
}