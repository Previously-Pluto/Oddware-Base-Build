using Naninovel;
using UnityEngine;

[CommandAlias("activateContextButton")]
public class ActivateContextButton : Command
{
    [ParameterAlias("eventHash")]
    public IntegerParameter EventHash;
    [ParameterAlias("toggle")]
    public BooleanParameter Toggle;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        //Use EventHash to identify the context event id an set is condition met
        Debug.Log($"Running context button verification. Values: {EventHash} {Toggle}");
        ContextMenuManager.Instance.SetEventStatus(EventHash, Toggle);
    }
}