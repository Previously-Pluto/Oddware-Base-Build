using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItem : Command
{
    public StringParameter ItemName;
    private StringEvent ShowItemEvent;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        ShowItemEvent = Resources.Load<StringEvent>("ItemsEvents/ShowItemEvent");
        Engine.GetService<CameraManager>().Camera.enabled = false;//we need to swicth the camera
        ShowItemEvent.Raise(ItemName);
        return UniTask.CompletedTask;
    }
}
