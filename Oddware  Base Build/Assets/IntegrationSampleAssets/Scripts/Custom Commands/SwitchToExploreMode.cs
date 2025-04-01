using System.Collections.Generic;
using Highlighters_BuiltIn;
using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[CommandAlias("explore")]
public class SwitchToExploreMode : Command
{
    [ParameterAlias("reset")]
    public BooleanParameter ResetState = true;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        // 1. Disable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = false;

        // 2. Deactivate dialogue continuation input canvas.
        var continueInputUI = Object.FindObjectOfType<ContinueInputUI>(true);
        continueInputUI.gameObject.SetActive(false);

        // 3. Stop script player.
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        scriptPlayer.Stop();

        // 4. Hide text printer.
        var hidePrinter = new HidePrinter();
        hidePrinter.ExecuteAsync(asyncToken).Forget();
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;
        
        // 5. Add components for Context Item detection
        Physics2DRaycaster raycaster2D = naniCamera.GetComponent<Physics2DRaycaster>();
        PhysicsRaycaster raycaster3D = naniCamera.GetComponent<PhysicsRaycaster>();

        if (raycaster2D == null)
            raycaster2D = naniCamera.gameObject.AddComponent<Physics2DRaycaster>();
        if (raycaster3D == null)
            raycaster3D = naniCamera.gameObject.AddComponent<PhysicsRaycaster>();
    }
}