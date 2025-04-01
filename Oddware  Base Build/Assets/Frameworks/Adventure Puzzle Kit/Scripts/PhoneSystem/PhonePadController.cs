using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using AdventurePuzzleKit;

namespace PhoneInputSystem
{
    [System.Serializable]
    public class PhoneCodes
    {
        public string phoneCode;
        public AudioClip phoneClip;
    }

    public class PhonePadController : MonoBehaviour
    {
        [Header("Keypad Parameters")]
        public int characterLim;
        [HideInInspector] public bool firstClick;
        private bool keypadOpen = false;

        [Header("Phone Codes")]
        public PhoneCodes[] phoneCodesList;

        [Header("UI Elements")]
        public InputField inputFieldCodeUI;
        [SerializeField] private GameObject keyPadCanvas = null;

        [Header("Sounds")]
        [SerializeField] private string phoneDialTone = "PhoneDeadDialTone";
        [SerializeField] private string phoneSingleBeep = "PhoneSingleBeep";
        [SerializeField] private string phoneDeadDialTone = "PhoneDeadDialTone";

        [Header("Trigger Type - ONLY if using a trigger event")]
        [SerializeField] private PhonePadTrigger triggerObject = null;
        [SerializeField] private bool isPhoneTrigger = false;


        private AudioSource mainAudio;

        private void Awake()
        {
            mainAudio = GetComponent<AudioSource>();
        }

        public void CheckCode()
        {
            try
            {
                StopAudio();
                var code = phoneCodesList.First(x => x.phoneCode == inputFieldCodeUI.text);
                mainAudio.PlayOneShot(code.phoneClip, 1f);
            }
            catch
            {
                StopAudio();
                AKAudioManager.instance.Play(phoneDialTone);
            }
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

            if (isPhoneTrigger)
            {
                AKUIManager.instance.triggerInteractPrompt.SetActive(false);
                triggerObject.enabled = false;
            }
        }

        public void CloseKeypad()
        {
            keypadOpen = false;
            keyPadCanvas.SetActive(false);
            AKDisableManager.instance.DisablePlayerDefault(false);
            StopAudio();

            if (isPhoneTrigger)
            {
                AKUIManager.instance.triggerInteractPrompt.SetActive(true);
                StopAudio();
                triggerObject.enabled = true;
            }
        }

        public void SingleBeep()
        {
            AKAudioManager.instance.Play(phoneSingleBeep);
        }

        public void StopAudio()
        {
            mainAudio.Stop();
            AKAudioManager.instance.StopPlaying(phoneDeadDialTone);
        }
    }
}
