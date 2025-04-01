using UnityEngine;

namespace KeypadSystem
{
    public class KeypadItemController : MonoBehaviour
    {
        [SerializeField] private KeyPadController _keypadController = null;

        public void ShowKeypad()
        {
            _keypadController.ShowKeypad();
        }
    }
}
