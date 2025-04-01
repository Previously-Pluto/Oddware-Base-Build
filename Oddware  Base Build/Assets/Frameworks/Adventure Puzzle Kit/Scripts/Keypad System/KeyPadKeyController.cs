using UnityEngine;

namespace KeypadSystem
{
    public class KeyPadKeyController : MonoBehaviour
    {
        private KeyPadController _keypadController;

        void Awake()
        {
            _keypadController = gameObject.GetComponent<KeyPadController>();
        }

        public void KeyPressString(string keyString)
        {
            _keypadController.SingleBeep();

            if (!_keypadController.firstClick)
            {
                _keypadController.inputFieldCodeUI.text = string.Empty;
                _keypadController.firstClick = true;
            }

            if (_keypadController.inputFieldCodeUI.characterLimit <= (_keypadController.characterLim - 1))
            {
                _keypadController.inputFieldCodeUI.characterLimit++;
                _keypadController.inputFieldCodeUI.text += keyString;
            }
        }

        public void KeyPressEnt()
        {
            _keypadController.SingleBeep();
            _keypadController.CheckCode();
        }

        public void KeyPressClr()
        {
            _keypadController.SingleBeep();
            _keypadController.inputFieldCodeUI.characterLimit = 0;
            _keypadController.inputFieldCodeUI.text = string.Empty;
            _keypadController.firstClick = false;
        }

        public void KeyPressClose()
        {
            _keypadController.SingleBeep();
            KeyPressClr();
            _keypadController.CloseKeypad();
        }
    }
}
