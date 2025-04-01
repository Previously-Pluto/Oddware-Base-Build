using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPuzzle : Command
{
    public StringParameter PuzzleName;
    private StringEvent PlayPuzzleEvent;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        PlayPuzzleEvent = Resources.Load<StringEvent>("PuzzlesEvents/PlayPuzzleEvent");
        Engine.GetService<CameraManager>().Camera.enabled = false;//we need to swicth the camera
        PlayPuzzleEvent.Raise(PuzzleName);
        return UniTask.CompletedTask;
    }
}
