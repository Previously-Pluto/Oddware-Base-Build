using UnityEngine;

namespace PhoneInputSystem
{
    public class PhonePadItemController : MonoBehaviour
    {
        [SerializeField] private PhonePadController _phonepadController = null;

        public void ShowKeypad()
        {
            _phonepadController.ShowKeypad();
        }
    }
}
