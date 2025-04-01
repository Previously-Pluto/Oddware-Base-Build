using UnityEngine;
using AdventurePuzzleKit;

namespace PhoneInputSystem
{
    public class PhonePadKeyController : MonoBehaviour
    {
        private PhonePadController _phonepadController;

        void Awake()
        {
            _phonepadController = gameObject.GetComponent<PhonePadController>();
        }

        public void KeyPressString(string keyString)
        {
            _phonepadController.SingleBeep();

            if (!_phonepadController.firstClick)
            {
                _phonepadController.inputFieldCodeUI.text = string.Empty;
                _phonepadController.firstClick = true;
            }

            if (_phonepadController.inputFieldCodeUI.characterLimit <= (_phonepadController.characterLim - 1))
            {
                _phonepadController.inputFieldCodeUI.characterLimit++;
                _phonepadController.inputFieldCodeUI.text += keyString;
            }
        }

        public void KeyPressCall()
        {
            _phonepadController.SingleBeep();
            _phonepadController.CheckCode();
        }

        public void KeyPressClr()
        {
            _phonepadController.SingleBeep();
            _phonepadController.StopAudio();
            _phonepadController.inputFieldCodeUI.characterLimit = 0;
            _phonepadController.inputFieldCodeUI.text = string.Empty;
            _phonepadController.firstClick = false;
        }

        public void KeyPressClose()
        {
            _phonepadController.SingleBeep();
            KeyPressClr();
            _phonepadController.CloseKeypad();
        }
    }
}
