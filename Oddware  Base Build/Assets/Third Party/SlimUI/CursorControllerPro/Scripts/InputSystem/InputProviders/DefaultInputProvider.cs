using UnityEngine;

namespace SlimUI.CursorControllerPro.InputSystem
{
// Provides default implementation of IInputProvider
    // The combination of IInputProvider plus DefaultInputProvider should be all you need
    // This is the only class that should access Unity's Input system
    // All public functions in this class should also be in the interface
    public class DefaultInputProvider : MonoBehaviour, IInputProvider {
        public bool IsGamepadActive(string currentXAxis, string currentYAxis, float deadZone) {
            if(Input.GetKey(KeyCode.Joystick1Button0)  ||
                Input.GetKey(KeyCode.Joystick1Button1)  ||
                Input.GetKey(KeyCode.Joystick1Button2)  ||
                Input.GetKey(KeyCode.Joystick1Button3)  ||
                Input.GetKey(KeyCode.Joystick1Button4)  ||
                Input.GetKey(KeyCode.Joystick1Button5)  ||
                Input.GetKey(KeyCode.Joystick1Button6)  ||
                Input.GetKey(KeyCode.Joystick1Button7)  ||
                Input.GetKey(KeyCode.Joystick1Button8)  ||
                Input.GetKey(KeyCode.Joystick1Button9)  ||
                Input.GetKey(KeyCode.Joystick1Button10) ||
                Input.GetKey(KeyCode.Joystick1Button11) ||
                Input.GetKey(KeyCode.Joystick1Button12) ||
                Input.GetKey(KeyCode.Joystick1Button13) ||
                Input.GetKey(KeyCode.Joystick1Button14) ||
                Input.GetKey(KeyCode.Joystick1Button15) ||
                Input.GetKey(KeyCode.Joystick1Button16) ||
                Input.GetKey(KeyCode.Joystick1Button17) ||
                Input.GetKey(KeyCode.Joystick1Button18) ||
                Input.GetKey(KeyCode.Joystick1Button19)) {
                return true;
            }
            
            // joystick axis
            if((Input.GetAxis(currentXAxis) > deadZone || Input.GetAxis(currentXAxis) < -deadZone) || (Input.GetAxis(currentYAxis) > deadZone || Input.GetAxis(currentYAxis) < -deadZone)){
                return true;
            }

            return false;
        }

        public bool IsMouseKeyboard(){
            if (Event.current.isMouse){
                return true;
            }else if( Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f ){
                return true;
            }
            
            return false;
        }

        public Vector3 GetMousePosition(){
            return Input.mousePosition;
        }

        public float GetAxis (string axisName){
            return Input.GetAxis(axisName);
        }

        public bool GetButtonDown(string buttonName){
            return Input.GetButtonDown(buttonName);
        }

        public bool GetButtonUp(string buttonName){
            return Input.GetButtonUp(buttonName);
        }
    }
}