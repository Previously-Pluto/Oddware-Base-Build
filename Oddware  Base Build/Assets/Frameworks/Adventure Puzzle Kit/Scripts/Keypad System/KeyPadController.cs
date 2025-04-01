//Script written by Matthew Rukas - Speed-tutor.com || speedtutoruk@gmail.com
using UnityEngine;
using UnityEngine.UI;
using AdventurePuzzleKit;
using UnityEngine.Events;

namespace KeypadSystem
{
    public class KeyPadController : MonoBehaviour
    {
        [Header("Keypad Parameters")]
        [SerializeField] private string validCode = null;
        public int characterLim;
        [HideInInspector] public bool firstClick;
        private bool keypadOpen = false;

        [Header("UI Elements")]
        public InputField inputFieldCodeUI;
        [SerializeField] private GameObject keyPadCanvas = null;

        [Header("GameObjects")]
        [SerializeField] private GameObject keypadModel = null;

        [Header("Sounds")]
        [SerializeField] private string accessDenied = "KeypadAccessDenied";
        [SerializeField] private string shortBeep = "KeypadShortBeep";

        [Header("Trigger Type - ONLY if using a trigger event")]
        [SerializeField] private KeyPadTrigger triggerObject = null;
        [SerializeField] private bool isKeypadTrigger = false;

        [Header("Unlock Event")]
        [SerializeField] private UnityEvent unlock = null;

        public void CheckCode()
        {
            if (inputFieldCodeUI.text == validCode)
            {
                keypadModel.tag = "Untagged";
                ValidCode();
            }

            else
            {
                AKAudioManager.instance.Play(accessDenied);
            }
        }

        void ValidCode()
        {
            unlock.Invoke();
        }

        private void Update()
        {
            if (keypadOpen)
            {
                if (Input.GetKeyDown(AKInputManager.instance.dropKey))
                {
                    CloseKeypad();
                }
            }
        }

         public void ShowKeypad()
         {
            keypadOpen = true;
            AKDisableManager.instance.DisablePlayerDefault(true);
            keyPadCanvas.SetActive(true);

            if (isKeypadTrigger)
            {
                AKUIManager.instance.triggerInteractPrompt.SetActive(false);
                triggerObject.enabled = false;
            }
         }

         public void CloseKeypad()
         {
            keypadOpen = false;
            AKDisableManager.instance.DisablePlayerDefault(false);
            keyPadCanvas.SetActive(false);

            if (isKeypadTrigger)
            {
                AKUIManager.instance.triggerInteractPrompt.SetActive(true);
                triggerObject.enabled = true;
            }
         }

         public void SingleBeep()
         {
            AKAudioManager.instance.Play(shortBeep);
         }
    }
}
