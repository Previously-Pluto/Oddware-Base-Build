using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using AdventurePuzzleKit;

namespace SafeUnlockSystem
{
    public class SafeController : MonoBehaviour
    {
        [Header("Safe Model Reference")]
        [SerializeField] private GameObject safeModel = null;
        [SerializeField] private Transform safeDial = null;

        [Header("Animation References")]
        [SerializeField] private string safeAnimationName = "SafeDoorOpen";
        private Animator safeAnim;

        [Header("Animation Timers - Default: 1.0f / 0.5f")]
        [SerializeField] private float beforeAnimationStart = 1.0f;
        [SerializeField] private float beforeOpenDoor = 0.5f;

        [Header("Safe UI")]
        [SerializeField] private GameObject safeUI = null;

        [Header("Safe Solution: 0-15")]
        [Range(0, 15)][SerializeField] private int safeSolutionNum1 = 0;
        [Range(0, 15)][SerializeField] private int safeSolutionNum2 = 0;
        [Range(0, 15)][SerializeField] private int safeSolutionNum3 = 0;

        [Header("UI Numbers")]
        [SerializeField] private Text firstNumberUI = null;
        [SerializeField] private Text secondNumberUI = null;
        [SerializeField] private Text thirdNumberUI = null;

        [Header("UI Arrows")]
        [SerializeField] private Button firstArrowUI = null;
        [SerializeField] private Button secondArrowUI = null;
        [SerializeField] private Button thirdArrowUI = null;

        [Header("Sounds")]
        [SerializeField] private string interactSound = "SafeInteractSound";
        [SerializeField] private string dialClick = "SafeClick";
        [SerializeField] private string boltUnlock = "SafeBoltUnlock";
        [SerializeField] private string handleSpin = "SafeHandleSpin";
        [SerializeField] private string doorOpen = "SafeDoorOpen";
        [SerializeField] private string lockRattle = "SafeLockRattle";

        private bool firstNumber;
        private bool secondNumber;
        private bool thirdNumber;

        private bool disableClose = false;
        private int lockNumberInt;

        [Header("Trigger Type - ONLY if using a trigger event")]
        [SerializeField] private GameObject triggerObject = null;
        [SerializeField] private bool isTriggerInteraction = false;

        [Header("Unity Event - What happens when you open the safe?")]
        [SerializeField] private UnityEvent safeOpened = null;

        void Awake()
        {
            if (isTriggerInteraction)
            {
                disableClose = true;
            }

            disableClose = true;
            firstNumber = true;
            safeAnim = safeModel.gameObject.GetComponent<Animator>();

            firstNumberUI.color = Color.white;
            secondNumberUI.color = Color.gray;
            thirdNumberUI.color = Color.gray;

            firstArrowUI.interactable = true;
            secondArrowUI.interactable = false;
            thirdArrowUI.interactable = false;

            ColorBlock firstArrowCB = firstArrowUI.colors; firstArrowCB.normalColor = Color.white; firstArrowUI.colors = firstArrowCB;
            ColorBlock secondArrowCB = secondArrowUI.colors; secondArrowCB.normalColor = Color.gray; secondArrowUI.colors = secondArrowCB;
            ColorBlock thirdArrowCB = thirdArrowUI.colors; thirdArrowCB.normalColor = Color.gray; thirdArrowUI.colors = thirdArrowCB;
        }

        public void ShowSafeLock()
        {
            if (isTriggerInteraction)
            {
                disableClose = false;
                triggerObject.SetActive(false);
            }

            disableClose = false;
            safeUI.SetActive(true);
            AKDisableManager.instance.DisablePlayerDefault(true);
            AKAudioManager.instance.Play(interactSound);
        }

        private void Update()
        {
            if (!disableClose)
            {
                if (Input.GetKeyDown(AKInputManager.instance.padlockCloseKey))
                {
                    if (isTriggerInteraction)
                    {
                        disableClose = true;
                        triggerObject.SetActive(true);
                    }

                    AKDisableManager.instance.DisablePlayerDefault(false);
                    safeUI.SetActive(false);
                }
            }
        }

        private IEnumerator CheckCode()
        {
            string playerInputNumber = firstNumberUI.text + secondNumberUI.text + thirdNumberUI.text;
            string safeSolution = safeSolutionNum1.ToString("0") + safeSolutionNum2.ToString("0") + safeSolutionNum3.ToString("0");

            if (playerInputNumber == safeSolution)
            {
                AKDisableManager.instance.DisablePlayerDefault(false);
                safeUI.SetActive(false);
                safeModel.tag = "Untagged";

                AKAudioManager.instance.Play(boltUnlock);
                yield return new WaitForSeconds(beforeAnimationStart);
                safeAnim.Play(safeAnimationName, 0, 0.0f);
                AKAudioManager.instance.Play(handleSpin);
                yield return new WaitForSeconds(beforeOpenDoor);
                AKAudioManager.instance.Play(doorOpen);

                if (isTriggerInteraction)
                {
                    disableClose = true;
                    triggerObject.SetActive(false);
                }

                safeOpened.Invoke();
            }
            else
            {
                AKAudioManager.instance.Play(lockRattle);
                firstNumberUI.text = "0";
                secondNumberUI.text = "0";
                thirdNumberUI.text = "0";
                firstNumber = true;
                secondNumber = false;
                thirdNumber = false;

                firstArrowUI.interactable = true;
                secondArrowUI.interactable = false;
                thirdArrowUI.interactable = false;

                firstNumberUI.color = Color.white;
                secondNumberUI.color = Color.gray;
                thirdNumberUI.color = Color.gray;

                ColorBlock firstArrowCB = firstArrowUI.colors; firstArrowCB.normalColor = Color.white; firstArrowUI.colors = firstArrowCB;
                ColorBlock secondArrowCB = secondArrowUI.colors; secondArrowCB.normalColor = Color.gray; secondArrowUI.colors = secondArrowCB;
                ColorBlock thirdArrowCB = thirdArrowUI.colors; thirdArrowCB.normalColor = Color.gray; thirdArrowUI.colors = thirdArrowCB;

                safeDial.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                lockNumberInt = 0;
            }
        }

        public void AcceptKey()
        {
            EventSystem.current.SetSelectedGameObject(null);
            AKAudioManager.instance.Play(interactSound);

            if (firstNumber)
            {
                firstNumber = false;
                secondNumber = true;
                thirdNumber = false;

                firstArrowUI.interactable = false;
                secondArrowUI.interactable = true;
                thirdArrowUI.interactable = false;

                secondNumberUI.text = lockNumberInt.ToString("0");

                firstNumberUI.color = Color.gray;
                secondNumberUI.color = Color.white;
                thirdNumberUI.color = Color.gray;

                ColorBlock firstArrowCB = firstArrowUI.colors; firstArrowCB.normalColor = Color.gray; firstArrowUI.colors = firstArrowCB;
                ColorBlock secondArrowCB = secondArrowUI.colors; secondArrowCB.normalColor = Color.white; secondArrowUI.colors = secondArrowCB;
                ColorBlock thirdArrowCB = thirdArrowUI.colors; thirdArrowCB.normalColor = Color.gray; thirdArrowUI.colors = thirdArrowCB;
            }
            else if (secondNumber)
            {
                firstNumber = false;
                secondNumber = false;
                thirdNumber = true;

                thirdNumberUI.text = lockNumberInt.ToString("0");

                firstArrowUI.interactable = false;
                secondArrowUI.interactable = false;
                thirdArrowUI.interactable = true;

                firstNumberUI.color = Color.gray;
                secondNumberUI.color = Color.gray;
                thirdNumberUI.color = Color.white;

                ColorBlock firstArrowCB = firstArrowUI.colors; firstArrowCB.normalColor = Color.gray; firstArrowUI.colors = firstArrowCB;
                ColorBlock secondArrowCB = secondArrowUI.colors; secondArrowCB.normalColor = Color.gray; secondArrowUI.colors = secondArrowCB;
                ColorBlock thirdArrowCB = thirdArrowUI.colors; thirdArrowCB.normalColor = Color.white; thirdArrowUI.colors = thirdArrowCB;
            }
            else if (thirdNumber)
            {
                firstNumber = false;
                secondNumber = false;
                thirdNumber = false;

                firstArrowUI.interactable = false;
                secondArrowUI.interactable = false;
                thirdArrowUI.interactable = false;

                firstNumberUI.color = Color.gray;
                secondNumberUI.color = Color.gray;
                thirdNumberUI.color = Color.gray;

                ColorBlock firstArrowCB = firstArrowUI.colors; firstArrowCB.normalColor = Color.gray; firstArrowUI.colors = firstArrowCB;
                ColorBlock secondArrowCB = secondArrowUI.colors; secondArrowCB.normalColor = Color.gray; secondArrowUI.colors = secondArrowCB;
                ColorBlock thirdArrowCB = thirdArrowUI.colors; thirdArrowCB.normalColor = Color.gray; thirdArrowUI.colors = thirdArrowCB;

                StartCoroutine(CheckCode());
            }
        }

        public void UpKey(int lockNumberSelection)
        {
            EventSystem.current.SetSelectedGameObject(null);
            AKAudioManager.instance.Play(dialClick);

            if (firstNumber && lockNumberSelection == 1)
            {
                if (lockNumberInt <= 14)
                {
                    safeDial.transform.Rotate(0.0f, 0.0f, -22.5f, Space.Self);
                    lockNumberInt++;
                    firstNumberUI.text = lockNumberInt.ToString("0");
                }
                else
                {
                    lockNumberInt = 0;
                    safeDial.transform.Rotate(0.0f, 0.0f, -22.5f, Space.Self);
                    firstNumberUI.text = lockNumberInt.ToString("0");
                }
            }

            if (secondNumber && lockNumberSelection == 2)
            {
                if (lockNumberInt >= 1)
                {
                    safeDial.transform.Rotate(0.0f, 0.0f, 22.5f, Space.Self);
                    lockNumberInt--;
                    secondNumberUI.text = lockNumberInt.ToString("0");
                }
                else
                {
                    lockNumberInt = 15;
                    safeDial.transform.Rotate(0.0f, 0.0f, 22.5f, Space.Self);
                    secondNumberUI.text = lockNumberInt.ToString("0");
                }
            }

            if (thirdNumber && lockNumberSelection == 3)
            {
                if (lockNumberInt <= 14)
                {
                    safeDial.transform.Rotate(0.0f, 0.0f, -22.5f, Space.Self);
                    lockNumberInt++;
                    thirdNumberUI.text = lockNumberInt.ToString("0");
                }
                else
                {
                    lockNumberInt = 0;
                    safeDial.transform.Rotate(0.0f, 0.0f, -22.5f, Space.Self);
                    thirdNumberUI.text = lockNumberInt.ToString("0");
                }
            }
        }
    }
}