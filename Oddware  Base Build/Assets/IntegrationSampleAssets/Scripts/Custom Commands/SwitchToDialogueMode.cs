using Naninovel;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[CommandAlias("dialogue")]
public class SwitchToDialogueMode : Command
{
    public StringParameter ScriptName;
    public StringParameter Label;

    public override async UniTask ExecuteAsync (AsyncToken asyncToken = default)
    {
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;
        var continueInputUI = Object.FindObjectOfType<ContinueInputUI>(true);
        continueInputUI.gameObject.SetActive(true);
        
        // Load and play specified script (is required).
        if (Assigned(ScriptName))
        {
            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            await scriptPlayer.PreloadAndPlayAsync(ScriptName, label: Label);
        }

        // Enable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = true;
    }
}