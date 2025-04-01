using Naninovel;
using UnityEngine;

public class SetupGame : MonoBehaviour
{
    public Camera AdventureModeCamera;
    public bool StartWithNaniOn;

    private async void Start()
    {
        if (StartWithNaniOn)
        {
            // 1. Initialize Naninovel.
            await RuntimeInitializer.InitializeAsync();
        }

        // 2. Enter adventure mode.
        var switchCommand = new SwitchToAdventureMode { ResetState = false };
        await switchCommand.ExecuteAsync();
    }
}