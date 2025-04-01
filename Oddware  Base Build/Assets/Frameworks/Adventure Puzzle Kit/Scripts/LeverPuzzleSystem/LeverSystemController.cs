using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using AdventurePuzzleKit;

namespace LeverPuzzleSystem
{
    public class LeverSystemController : MonoBehaviour
    {
        [Header("In-game order - Used for debugging (Can be hidden)")]
        public string playerOrder = null;

        [Header("Custom Order")]
        [SerializeField] private string leverOrder = "12345";

        [Header("Pull Limit - Match this with the number of values in the leverOrder")]
        public int pullLimit;
        [HideInInspector] public int pulls;

        [Header("Time before pulling lever after interacting")]
        [SerializeField] private float waitTimer = 1.0f;

        [HideInInspector] public bool canPull = true;
        private bool resetting;

        [Header("Add all of the required objects to the Array")]
        [SerializeField] private GameObject[] interactiveObjects = null;

        [Header("Control Box Switches (Animated)")]
        [SerializeField] private Animator readySwitch = null;
        [SerializeField] private Animator limitReachedSwitch = null;
        [SerializeField] private Animator acceptedSwitch = null;
        [SerializeField] private Animator resettingSwitch = null;


        [Header("Control Unit Lights")]
        [SerializeField] private GameObject readyLight = null;
        [SerializeField] private GameObject limitReachedLight = null;
        [SerializeField] private GameObject acceptedLight = null;
        [SerializeField] private GameObject resettingLight = null;
        private Material readyBtnMat;
        private Material resettingBtnMat;
        private Material acceptedBtnMat;
        private Material limitBtnMat;

        [Header("Accept / Reset Buttons")]
        [SerializeField] private Animator redButton1 = null;
        [SerializeField] private Animator redButton2 = null;

        [Header("Switch Object - Animation Names")]
        [SerializeField] private string switchOnName = "Switch_On";
        [SerializeField] private string switchOffName = "Switch_Off";
        [SerializeField] private string redButtonName = "RedButton_Push";

        [Header("Audio Sounds")]
        [SerializeField] private string switchPullSound = "SFXSwitchPull";
        [SerializeField] private string switchEchoSound = "SFXSwitchEcho";
        [SerializeField] private string acceptedSound = "SFXDoorBuzz";

        [Header("Unity Events")]
        [SerializeField] private UnityEvent LeverPower = null;

        private void Start()
        {
            readyBtnMat = readyLight.GetComponent<Renderer>().material;
            resettingBtnMat = resettingLight.GetComponent<Renderer>().material;
            acceptedBtnMat = acceptedLight.GetComponent<Renderer>().material;
            limitBtnMat = limitReachedLight.GetComponent<Renderer>().material;
            readyBtnMat.color = Color.green;

            readySwitch.Play(switchOnName, 0, 0.0f);
            resettingSwitch.Play(switchOffName, 0, 0.0f);
            acceptedSwitch.Play(switchOffName, 0, 0.0f);
            limitReachedSwitch.Play(switchOffName, 0, 0.0f);
        }

        void LeverInteraction()
        {
            LeverPower.Invoke();
        }

        IEnumerator Timer()
        {
            canPull = false;
            yield return new WaitForSeconds(waitTimer);
            canPull = true;
        }

        public void LeverPull()
        {        
            if (canPull)
            {
                StartCoroutine(Timer());
                AKAudioManager.instance.Play(switchPullSound);
                if (pulls >= pullLimit)
                {
                    readyBtnMat.color = Color.red;
                    limitBtnMat.color = Color.green;

                    readySwitch.Play(switchOffName, 0, 0.0f);
                    limitReachedSwitch.Play(switchOnName, 0, 0.0f);
                }
            }
        }

        public void LeverReset()
        {
            pulls = 0;
            playerOrder = "";
            AKAudioManager.instance.Play(switchEchoSound);
            redButton2.Play(redButtonName, 0, 0.0f);

            StartCoroutine(Timer(1.0f));
            if (resetting)
            {
                readyBtnMat.color = Color.red;
                resettingBtnMat.color = Color.green;
                acceptedBtnMat.color = Color.red;
                limitBtnMat.color = Color.red;

                readySwitch.Play(switchOffName, 0, 0.0f);
                resettingSwitch.Play(switchOnName, 0, 0.0f);
                acceptedSwitch.Play(switchOffName, 0, 0.0f);
                limitReachedSwitch.Play(switchOffName, 0, 0.0f);
            }
        }

        public void LeverCheck()
        {
            redButton1.Play(redButtonName, 0, 0.0f);
            if (playerOrder == leverOrder)
            {
                pulls = 0;
                AKAudioManager.instance.Play(acceptedSound);

                LeverInteraction();

                for (int i = 0; i < interactiveObjects.Length; i++)
                {
                    interactiveObjects[i].gameObject.tag = "Untagged";
                }

                readyBtnMat.color = Color.red;
                resettingBtnMat.color = Color.red;
                acceptedBtnMat.color = Color.green;
                limitBtnMat.color = Color.red;

                readySwitch.Play(switchOffName, 0, 0.0f);
                resettingSwitch.Play(switchOffName, 0, 0.0f);
                acceptedSwitch.Play(switchOnName, 0, 0.0f);
                limitReachedSwitch.Play(switchOffName, 0, 0.0f);
            }

            else
            {
                LeverReset();
            }
        }

        IEnumerator Timer(float waitTime)
        {
            resetting = true;
            yield return new WaitForSeconds(waitTime);
            readyBtnMat.color = Color.green;
            resettingBtnMat.color = Color.red;
            
            readySwitch.Play(switchOnName, 0, 0.0f);
            resettingSwitch.Play(switchOffName, 0, 0.0f);

            resetting = false;
        }

        private void OnDestroy()
        {
            Destroy(readyBtnMat);
            Destroy(resettingBtnMat);
            Destroy(acceptedBtnMat);
            Destroy(limitBtnMat);
        }
    }
}
