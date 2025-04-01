using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    private static GameProgress INSTANCE;
    private ICustomVariableManager VariableManager => Engine.GetService<ICustomVariableManager>();
    private IStateManager StateManager => Engine.GetService<IStateManager>();

    private void Awake()
    {
        INSTANCE = this;
    }

    public static bool GetNani(string eventKey, bool defaultValue)
    {
        string res = INSTANCE.VariableManager.GetVariableValue(eventKey);
        if (res == null)
            return defaultValue;
        return bool.Parse(res);
    }

    public static int GetNani(string eventKey, int defaultValue)
    {
        string res = INSTANCE.VariableManager.GetVariableValue(eventKey);
        if (res == null)
            return defaultValue;
        return int.Parse(res);
    }

    public static string GetNani(string eventKey, string defaultValue)
    {
        string res = INSTANCE.VariableManager.GetVariableValue(eventKey);
        if (res == null)
            return defaultValue;
        return res;
    }

    public static async void SetNani(string eventKey, string value)
    {
        INSTANCE.VariableManager.SetVariableValue(eventKey, value);
        await INSTANCE.StateManager.SaveGlobalAsync();
    }

    public static async void SetNani(string eventKey, bool value)
    {
        INSTANCE.VariableManager.SetVariableValue(eventKey, value.ToString());
        await INSTANCE.StateManager.SaveGlobalAsync();
    }

    public static async void SetNani(string eventKey, int value)
    {
        INSTANCE.VariableManager.SetVariableValue(eventKey, value.ToString());
        await INSTANCE.StateManager.SaveGlobalAsync();
    }

    public static void ClearEverything()
    {
        INSTANCE.VariableManager.ResetGlobalVariables();
        INSTANCE.VariableManager.ResetLocalVariables();
    }

}
