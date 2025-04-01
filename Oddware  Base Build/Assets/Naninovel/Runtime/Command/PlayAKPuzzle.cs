using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayAKPuzzle : Command
{
    public StringParameter PuzzleName;
    private StringEvent PlayPuzzleEvent;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        PlayPuzzleEvent = Resources.Load<StringEvent>("PuzzlesEvents/PlayAKPuzzleEvent");
        Engine.GetService<CameraManager>().Camera.enabled = false;//we need to swicth the camera
        PlayPuzzleEvent.Raise(PuzzleName);
        return UniTask.CompletedTask;
    }
}
