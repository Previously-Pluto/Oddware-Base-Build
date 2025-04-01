using Naninovel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FocusOn : Command
{
    public StringParameter CustomUIName;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        GameObject parent = GameObject.Find(CustomUIName);
        Button[] buttons = parent.GetComponentsInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject, null);
        return UniTask.CompletedTask;
    }
}
