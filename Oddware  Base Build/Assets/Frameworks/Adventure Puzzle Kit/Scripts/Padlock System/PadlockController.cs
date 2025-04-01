using AdventurePuzzleKit;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PadlockSystem
{
    public class PadlockController : MonoBehaviour
    {      
        private string playerCombi;

        [Header("Padlock Code")]
        [SerializeField] private string yourCombination = null;
        private bool hasUnlocked;
        private bool isShowing;

        //Hidden from the inspector - These are only integers to hold some information for later.
        [HideInInspector] public int combinationRow1;
        [HideInInspector] public int combinationRow2;
        [HideInInspector] public int combinationRow3;
        [HideInInspector] public int combinationRow4;

        [Header("Player References")]
        private Camera mainCamera;
        private AdventureKitRaycast mainCameraRaycast;

        [Header("Camera GameObject References")]
        [SerializeField] private GameObject cameraPadlock = null;
        [SerializeField] private Animator lockAnim = null;

        [Header("World Objects")]
        [SerializeField] private GameObject interactableLock = null;

        [Header("Sounds")]
        [SerializeField] private string padlockInteract = "PadlockInteract";
        [SerializeField] private string padlockSpin = "PadlockSpin";
        [SerializeField] private string padlockUnlock = "PadlockUnlock";
        [SerializeField] private string lockOpen = "LockOpen";

        [Header("Trigger Type - ONLY if using a trigger event")]
        [SerializeField] private PadlockTrigger triggerObject = null;
        [SerializeField] private bool isPadlockTrigger = false;

        [Header("Unlock Events")]
        [SerializeField] private UnityEvent unlock = null;

        void Awake()
        {
            mainCamera = Camera.main;
            mainCameraRaycast = mainCamera.GetComponent<AdventureKitRaycast>();
            combinationRow1 = 1;
            combinationRow2 = 1;
            combinationRow3 = 1;
            combinationRow4 = 1;
        }

        void UnlockPadlock()
        {
            unlock.Invoke();
        }

        public void ShowPadlock()
        {
            cameraPadlock.SetActive(true);
            isShowing = true;
            AKDisableManager.instance.DisablePlayerDefault(true);
            mainCamera.transform.localEulerAngles = new Vector3(0, 0, 0);
            InteractSound();

            if (isPadlockTrigger)
            {
                AKUIManager.instance.triggerInteractPrompt.SetActive(false);
                triggerObject.enabled = false;
            }
        }

        void Disable()
        {
            AKDisableManager.instance.DisablePlayerDefault(false);
            mainCameraRaycast.enabled = true;
            cameraPadlock.SetActive(false);
            isShowing = false;

            if (isPadlockTrigger)
            {
                AKUIManager.instance.triggerInteractPrompt.SetActive(true);
                triggerObject.enabled = true;
            }
        }

        IEnumerator CorrectCombination()
        {
            lockAnim.Play(lockOpen);
            UnlockSound();

            const float waitDuration = 1.2f;
            yield return new WaitForSeconds(waitDuration);

            cameraPadlock.SetActive(false);
            interactableLock.SetActive(false);
            UnlockPadlock();

            AKDisableManager.instance.DisablePlayer(false);
            mainCameraRaycast.enabled = true;
            gameObject.SetActive(false);
        }

        public void CheckCombination()
        {
            playerCombi = combinationRow1.ToString("0") + combinationRow2.ToString("0") + combinationRow3.ToString("0") + combinationRow4.ToString("0");

            if (playerCombi == yourCombination)
            {
                if (!hasUnlocked)
                {
                    StartCoroutine(CorrectCombination());
                    hasUnlocked = true;
                }
            }
        }

        void Update()
        {
            if (isShowing)
            {
                if (Input.GetKeyDown(AKInputManager.instance.padlockCloseKey))
                {
                    Disable();
                }
            }
        }

        void InteractSound()
        {
            AKAudioManager.instance.Play(padlockInteract);
        }

        public void SpinSound()
        {
            AKAudioManager.instance.Play(padlockSpin);
        }

        public void UnlockSound()
        {
            AKAudioManager.instance.Play(padlockUnlock);
        }
    }
}
