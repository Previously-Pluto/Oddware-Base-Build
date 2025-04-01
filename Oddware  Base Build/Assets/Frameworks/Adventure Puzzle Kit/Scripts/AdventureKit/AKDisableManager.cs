using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Utility;
using UnityStandardAssets.ImageEffects;

namespace AdventurePuzzleKit
{
    public class AKDisableManager : MonoBehaviour
    {
        [Header("First Person Variables")]
        [SerializeField] private bool isFirstPerson = false;
        public FirstPersonController player = null;

        [Header("Third Person Variables")]
        [SerializeField] private bool isThirdPerson = false;
        public ThirdPersonUserControl thirdPersonController = null;
        public SimpleMouseRotator thirdPersonRotator = null;

        [Header("Generic Variables")]
        [SerializeField] private Image crosshair = null;        
        [SerializeField] private AdventureKitRaycast raycastManager = null;
        [SerializeField] private BlurOptimized blur = null;

        public static AKDisableManager instance;
        public PuzzleEventHandler puzzleEventHandler;
        public Transform m_itemsContainer;
        public Transform m_notesContainer;
        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }

            if (isThirdPerson)
            {
                ShowCursor(false);
            }
        }

        void ShowCursor(bool showCursor)
        {
            if (showCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void DisablePlayerDefault(bool disable)
        {
            if (disable)
            {
                raycastManager.enabled = false;
                ShowCursor(true);
                AKUIManager.instance.isInteracting = true;
                crosshair.enabled = false;

                if (isFirstPerson)
                {
                    player.enabled = false;
                }

                if (isThirdPerson)
                {
                    thirdPersonController.enabled = false;
                    thirdPersonRotator.enabled = false;
                }
            }

            else
            {
                DisableAllItems();
                raycastManager.enabled = true;
                //ShowCursor(false);
                AKUIManager.instance.isInteracting = false;
                crosshair.enabled = true;

                if (isFirstPerson)
                {
                    player.enabled = true;
                }

                if (isThirdPerson)
                {
                    thirdPersonController.enabled = true;
                    thirdPersonRotator.enabled = true;
                }
                puzzleEventHandler.ActivateNaninovel(); //activated Naninovel
            }
        }

        public void DisablePlayerExamine(bool disable)
        {
            if (disable)
            {
                raycastManager.enabled = false;
                ShowCursor(true);
                AKUIManager.instance.isInteracting = true;
                crosshair.enabled = false;
                blur.enabled = true;

                if (isFirstPerson)
                {
                    player.enabled = false;
                }

                if (isThirdPerson)
                {
                    thirdPersonController.enabled = false;
                    thirdPersonRotator.enabled = false;
                }
            }
            else
            {
                raycastManager.enabled = true;
                //ShowCursor(false);
                AKUIManager.instance.isInteracting = false;
                crosshair.enabled = true;
                blur.enabled = false;

                if (isFirstPerson)
                {
                    player.enabled = true;
                }

                if (isThirdPerson)
                {
                    thirdPersonController.enabled = true;
                    thirdPersonRotator.enabled = true;
                }
                CloseNotesObjects();
                puzzleEventHandler.ActivateNaninovel(); //activated Naninovel
            }
        }

        public void CloseNotesObjects()
        {
            foreach (Transform t in m_notesContainer)
                t.gameObject.SetActive(false);
            m_notesContainer.gameObject.SetActive(false);
        }
        public void DisablePlayer(bool disable)
        {
            if (disable)
            {
                AKUIManager.instance.isInteracting = true;
                crosshair.enabled = false;

                if (isFirstPerson)
                {
                    player.enabled = false;
                }

                if (isThirdPerson)
                {
                    thirdPersonController.enabled = false;
                    thirdPersonRotator.enabled = false;
                }
            }
            else
            {
                AKUIManager.instance.isInteracting = false;
                crosshair.enabled = true;

                if (isFirstPerson)
                {
                    player.enabled = true;
                }

                if (isThirdPerson)
                {
                    thirdPersonController.enabled = true;
                    thirdPersonRotator.enabled = true;
                }
            }
        }

        public void DisablePlayerInventory(bool disable)
        {
            if (disable)
            {
                raycastManager.enabled = false;
                crosshair.enabled = false;

                if (isFirstPerson)
                {
                    player.enabled = false;
                }

                if (isThirdPerson)
                {
                    thirdPersonController.enabled = false;
                    thirdPersonRotator.enabled = false;
                }
            }
            else
            {
                raycastManager.enabled = true;           
                crosshair.enabled = true;

                if (isFirstPerson)
                {
                    player.enabled = true;
                }

                if (isThirdPerson)
                {
                    thirdPersonController.enabled = true;
                    thirdPersonRotator.enabled = true;
                }
            }
        }

        public void DisableAllItems()
        {
            foreach(Transform t in m_itemsContainer)
            {
                t.gameObject.SetActive(false);
            }
        }
    }
}