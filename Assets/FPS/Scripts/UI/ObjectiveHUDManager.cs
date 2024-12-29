using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Gameplay;


namespace Unity.FPS.UI
{
    public class ObjectiveHUDManager : MonoBehaviour
    {
        [Tooltip("UI panel containing the layoutGroup for displaying objectives")]
        public RectTransform ObjectivePanel;

        [Tooltip("Prefab for the primary objectives")]
        public GameObject PrimaryObjectivePrefab;

        [Tooltip("Prefab for the primary objectives")]
        public GameObject SecondaryObjectivePrefab;

        //new
        Dictionary<QuestStage, ObjectiveToast> m_StagesDictionary;

        void Awake()
        {
            m_StagesDictionary = new Dictionary<QuestStage, ObjectiveToast>();

            //EventManager.AddListener<ObjectiveUpdateEvent>(OnUpdateObjective);

            EventManager.AddListener<StageStartedEvent>(OnStageStarted);

        }

        private void OnStageStarted(StageStartedEvent evt)
        {
            evt.Stage.OnStageStarted += RegisterStage;
            evt.Stage.OnStageCompleted += UnregisterStage;

        }

        public void RegisterStage(QuestStage stage)
        {
            // instanciate the Ui element for the new objective

            //----NEW-----
            GameObject objectiveUIInstance =
                Instantiate(PrimaryObjectivePrefab, ObjectivePanel);

            //if (!objective.IsOptional)
            //    objectiveUIInstance.transform.SetSiblingIndex(0);

            ObjectiveToast toast = objectiveUIInstance.GetComponent<ObjectiveToast>();
            DebugUtility.HandleErrorIfNullGetComponent<ObjectiveToast, ObjectiveHUDManager>(toast, this,
                objectiveUIInstance.gameObject);

            // initialize the element and give it the objective description

            //----NEW-----
            toast.Initialize(stage.Title, stage.Objectives, stage.DelayVisible);

            m_StagesDictionary.Add(stage, toast);

            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(ObjectivePanel);
        }

        public void UnregisterStage(QuestStage stage)
        {
            // if the objective if in the list, make it fade out, and remove it from the list
            if (m_StagesDictionary.TryGetValue(stage, out ObjectiveToast toast) && toast != null)
            {
                toast.Complete();
            }

            m_StagesDictionary.Remove(stage);
        }

        //void OnUpdateObjective(ObjectiveUpdateEvent evt)
        //{
        //    

        //    if (m_ObjectivesDictionnary.TryGetValue(evt.Objective, out ObjectiveToast toast) && toast != null)
        //    {
        //        // set the new updated description for the objective, and forces the content size fitter to be recalculated
        //        Canvas.ForceUpdateCanvases();
        //        if (!string.IsNullOrEmpty(evt.DescriptionText))
        //            toast.DescriptionTextContent.text = evt.DescriptionText;

        //        if (!string.IsNullOrEmpty(evt.CounterText))
        //            toast.CounterTextContent.text = evt.CounterText;

        //        if (toast.GetComponent<RectTransform>())
        //        {
        //            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(toast.GetComponent<RectTransform>());
        //        }
        //    }
        //}

        void OnDestroy()
        {
            EventManager.RemoveListener<StageStartedEvent>(OnStageStarted);
        }
    }
}