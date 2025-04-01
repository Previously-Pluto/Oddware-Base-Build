using Naninovel;
using UnityEngine;

public class SetGlobalValue : MonoBehaviour
{
    public void SetVariableValue(string _value)
    {
        StringEventListener _eventListener = GetComponent<StringEventListener>();
        var variableManager = Engine.GetService<ICustomVariableManager>();
        var globalVariableName = _eventListener.Event.m_previousValue;
        variableManager.SetVariableValue(globalVariableName, _value);
        Debug.Log("Global Variable value: " + variableManager.GetVariableValue(globalVariableName));
    }
}
