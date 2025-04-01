using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetGlobalVariableValue : MonoBehaviour
{
    public void SetVariableValue(StringEventListener _eventListener)
    {
        var variableManager = Engine.GetService<ICustomVariableManager>();
        var myValue = variableManager.GetVariableValue(_eventListener.Event.testProperty);
        myValue = "1";
        variableManager.SetVariableValue(_eventListener.Event.testProperty, myValue);
    }
}
