using System;
using System.Collections;
using System.Collections.Generic;
using Naninovel;
using PixelCrushers.QuestMachine;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject _questCanvas;
    private QuestGiver _mainQuestGiver;
    private QuestControl _questController;
    private QuestMachineConfiguration _questMachine;
    
    public static QuestManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _mainQuestGiver = GetComponentInChildren<QuestGiver>(true);
        _questController = GetComponentInChildren<QuestControl>(true);
        _questMachine = GetComponentInChildren<QuestMachineConfiguration>(true);
    }

    public void GiveQuest(StringParameter questID)
    {
        // Quest quest = QuestMachine.GetQuestAsset(questID);
        // _mainQuestGiver.AddQuest(quest);
        //_questController.SendToMessageSystem($"StartQuest:{questID}");
        _mainQuestGiver.StartSpecifiedQuestDialogueWithPlayer(questID);
    }
    
    public void ProgressQuest(StringParameter completionMessage)
    {
        _questController.SendToMessageSystem(completionMessage);
    }
}