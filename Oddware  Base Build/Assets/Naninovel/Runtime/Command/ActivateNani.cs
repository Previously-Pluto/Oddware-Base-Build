using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateNani : Command
{
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        Engine.GetService<CameraManager>().Camera.enabled = true;//we need to swicth the camera on
        return UniTask.CompletedTask;
    }
}
