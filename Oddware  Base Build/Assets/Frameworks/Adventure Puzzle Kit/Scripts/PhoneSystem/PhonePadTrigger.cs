using AdventurePuzzleKit;
using UnityEngine;

namespace PhoneInputSystem
{
    public class PhonePadTrigger : MonoBehaviour
    {
        [Header("Keypad Object")]
        [SerializeField] private PhonePadItemController myKeypad = null;

        private const string playerTag = "Player";

        private bool canUse;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = true;
                AKUIManager.instance.triggerInteractPrompt.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = false;
                AKUIManager.instance.triggerInteractPrompt.SetActive(false);
            }
        }

        private void Update()
        {
            if (canUse)
            {
                if (Input.GetKeyDown(AKInputManager.instance.triggerInteractKey))
                {
                    myKeypad.ShowKeypad();
                }
            }
        }
    }
}
