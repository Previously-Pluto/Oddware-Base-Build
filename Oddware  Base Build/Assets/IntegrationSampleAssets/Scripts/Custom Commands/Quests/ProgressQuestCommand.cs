using Naninovel;
using UnityEngine;
using UnityEngine.Serialization;

[CommandAlias("progressQuest")]
public class ProgressQuestCommand : Command
{
    [ParameterAlias("completionMessage")]
    public StringParameter CompletionMessage;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        //Use EventHash to identify the context event id an set is condition met
        Debug.Log($"Running context button verification. Values: {CompletionMessage}");
        QuestManager.Instance.ProgressQuest(CompletionMessage);
    }
}