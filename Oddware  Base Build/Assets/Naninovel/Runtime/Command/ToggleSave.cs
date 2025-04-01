using Naninovel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSave : Command
{

    public StringParameter Value;
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        GameObject parent = GameObject.Find("ControlPanel");
        if(parent != null)
        {
            if (Value.ToString().ToLower().Equals("true"))
            {
                parent.transform.GetChild(0).Find("QuickSaveButton").gameObject.SetActive(true);
                parent.transform.GetChild(0).Find("SaveButton").gameObject.SetActive(true);
                parent.transform.GetChild(0).Find("QuickLoadButton").gameObject.SetActive(true);
                parent.transform.GetChild(0).Find("LoadButton").gameObject.SetActive(true);
            }
            else
            {
                parent.transform.GetChild(0).Find("QuickSaveButton").gameObject.SetActive(false);
                parent.transform.GetChild(0).Find("SaveButton").gameObject.SetActive(false);
                parent.transform.GetChild(0).Find("QuickLoadButton").gameObject.SetActive(false);
                parent.transform.GetChild(0).Find("LoadButton").gameObject.SetActive(false);
            }
        }
        return UniTask.CompletedTask;
    }
}
