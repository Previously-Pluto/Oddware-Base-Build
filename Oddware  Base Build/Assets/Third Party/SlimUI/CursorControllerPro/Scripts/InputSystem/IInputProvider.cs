using UnityEngine;

namespace SlimUI.CursorControllerPro.InputSystem{
    // Provides functions that allow CursorController to not directly reference Unity's Input system
    public interface IInputProvider{        
        bool IsGamepadActive(string currentXAxis, string currentYAxis, float deadZone);
        bool IsMouseKeyboard();
        Vector3 GetMousePosition();
        float GetAxis(string axisName);
        bool GetButtonDown(string buttonName);
        bool GetButtonUp(string buttonName);
    }
}