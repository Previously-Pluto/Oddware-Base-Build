using Naninovel;
using UnityEngine;

[CommandAlias("closeQuestJournal")]
public class CloseQuestJournalCommand : Command
{
    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        //Use EventHash to identify the context event id an set is condition met
        Debug.Log($"Running context button verification. Values:");
        QuestHUDUI.Instance.CloseQuestJournal();
    }
}