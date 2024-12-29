using System;
using TMPro;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestLogManager : MonoBehaviour
{
    [Tooltip("Master volume when menu is open")]
    [Range(0.001f, 1f)]
    public float VolumeWhenQuestLogOpen = 0.5f;

    [Tooltip("Root object of quest log window")]
    [SerializeField] GameObject questLogRoot;
    public bool IsActive => questLogRoot.activeSelf;

    [Tooltip("Prefab object for quest button")]
    [SerializeField] GameObject questButtonPrefab;

    [Tooltip("Panel for adding quests in progress")]
    [SerializeField] RectTransform inProgressQuestsPanel;

    [Tooltip("Panel for adding completed quests")]
    [SerializeField] RectTransform completedQuestsPanel;
    
    [Tooltip("Panel with all descriptions")]
    [SerializeField] RectTransform descriptionPanel;


    [Tooltip("Text UI object for title of chosen quest")]
    [SerializeField] TMP_Text chosenQuestTitle;

    [Tooltip("Text UI object for stages of chosen quest")]
    [SerializeField] TMP_Text chosenQuestStages;

    [Tooltip("Text UI object for description of chosen quest")]
    [SerializeField] TMP_Text chosenQuestDescription;



    QuestsManager _questsManager;

    private void Start()
    {
        questLogRoot.SetActive(false);

        _questsManager = FindObjectOfType<QuestsManager>();
        DebugUtility.HandleErrorIfNullFindObject<QuestsManager, QuestLogManager>(_questsManager, this);
    }

    private void Update()
    {
        if (Input.GetButtonDown(GameConstants.k_ButtonNameQuestLog)
                || (questLogRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
        {
            SetQuestLogActivation(!questLogRoot.activeSelf);
        }
    }

    public void CloseQuestLog()
    {
        SetQuestLogActivation(false);
    }

    void SetQuestLogActivation(bool active)
    {
        questLogRoot.SetActive(active);

        if (questLogRoot.activeSelf)
        {
            chosenQuestStages.text = string.Empty;
            chosenQuestTitle.text = string.Empty;
            chosenQuestDescription.text = string.Empty;


            inProgressQuestsPanel.parent.gameObject.SetActive(true);
            completedQuestsPanel.parent.gameObject.SetActive(true);

            FillQuestsList();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            AudioUtility.SetMasterVolume(VolumeWhenQuestLogOpen);

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            CleanQuestsList();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            AudioUtility.SetMasterVolume(1);
        }

    }

    void FillQuestsList()
    {
        int activeAmount = 0;
        int finishedAmount = 0;

        foreach (Quest quest in _questsManager.Quests)
        {
            if (quest.CurrentState == QuestState.ACTIVE)
            {
                CreateQuestButton(quest, inProgressQuestsPanel);
                activeAmount++;
            }

            if (quest.CurrentState == QuestState.FINISHED)
            {
                CreateQuestButton(quest, completedQuestsPanel);
                finishedAmount++;
            }
        }

        HideEmptyLists(activeAmount, finishedAmount);
    }

    private void HideEmptyLists(int activeAmount, int finishedAmount)
    {
        if (activeAmount == 0) inProgressQuestsPanel.parent.gameObject.SetActive(false);
        if (finishedAmount == 0) completedQuestsPanel.parent.gameObject.SetActive(false);
    }

    private void CreateQuestButton(Quest quest, RectTransform panel)
    {
        GameObject newQuestButton = Instantiate(questButtonPrefab, panel);
        newQuestButton.GetComponentInChildren<TMP_Text>().text = quest.Title;
        newQuestButton.GetComponent<Button>().onClick.AddListener(() => FillQuestData(quest));
    }

    void CleanQuestsList()
    {
        foreach (RectTransform child in inProgressQuestsPanel)
        {
            Destroy(child.gameObject);
        }
        
        foreach (RectTransform child in completedQuestsPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void FillQuestData(Quest questData)
    {
        chosenQuestTitle.text = questData.Title;
        chosenQuestDescription.text = questData.Description;


        chosenQuestStages.text = string.Empty;

        if (questData.CurrentState == QuestState.FINISHED)
            FillFinishedQuestStages(questData); 
        else
            FillInProgressQuestStages(questData);


        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionPanel);

    }

    private void FillInProgressQuestStages(Quest questData)
    {
        for (int i = 0; i <= questData.CurrentStageIndex; i++)
        {
            if (i < questData.CurrentStageIndex)
                chosenQuestStages.text += $"<s><color=#585858>";

            FillQuestStagesText(questData, i);

            if (i < questData.CurrentStageIndex)
                chosenQuestStages.text += "</s></color>";

            chosenQuestStages.text += "\n";
        }
    }

    private void FillFinishedQuestStages(Quest questData)
    {
        for (int i = 0; i < questData.StageList.Count; i++)
        {
            chosenQuestStages.text += $"<s><color=#585858>";

            FillQuestStagesText(questData, i);

            chosenQuestStages.text += "</s></color>\n";
        }
    }

    private void FillQuestStagesText(Quest questData, int index)
    {
        chosenQuestStages.text += $"{questData.StageList[index].Title}\n";

        foreach (Objective obj in questData.StageList[index].Objectives)
            chosenQuestStages.text += $"- {obj.Description}\n";
    }
}
