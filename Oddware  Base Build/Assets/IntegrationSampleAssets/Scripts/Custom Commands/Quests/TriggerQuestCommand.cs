using System.Collections;
using System.Collections.Generic;
using Naninovel;
using UnityEngine;

[CommandAlias("triggerQuest")]
public class TriggerQuestCommand : Command
{
    [ParameterAlias("questID")]
    public StringParameter QuestID;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        //Use EventHash to identify the context event id an set is condition met
        Debug.Log($"Running context button verification. Values: {QuestID}");
        QuestManager.Instance.GiveQuest(QuestID);
    }
}