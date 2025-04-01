using UnityEngine;
using UnityEngine.UI;

namespace AdventurePuzzleKit
{
    public class AKUIManager : MonoBehaviour
    {
        [Header("UI Canvas")]
        [SerializeField] private GameObject adventureKitCanvas = null;
        [SerializeField] private GameObject generatorUICanvas = null;
        [SerializeField] private GameObject flashlightUICanvas = null;
        [SerializeField] private GameObject gasMaskUICanvas = null;
        [SerializeField] private GameObject themedKeyUICanvas = null;
        [SerializeField] private GameObject chessPuzzleCanvas = null;
        [SerializeField] private GameObject fuseBoxCanvas = null;

        [Header("Indicator Prompts")]
        public Image radialIndicator = null;
        public GameObject triggerInteractPrompt = null;

        [HideInInspector] public bool hasFlashlight;
        [HideInInspector] public bool hasJerrycan;
        [HideInInspector] public bool hasGasMask;
        [HideInInspector] public bool hasThemedKey;
        [HideInInspector] public bool hasChessPiece;
        [HideInInspector] public bool hasFuse;

        public bool isInteracting;
        [HideInInspector] public bool isInventoryOpen;

        private bool showUI;

        public static AKUIManager instance;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void OpenInventory()
        {
            adventureKitCanvas.SetActive(true);
            chessPuzzleCanvas.SetActive(true);
            isInventoryOpen = true;
            AKDisableManager.instance.DisablePlayerDefault(true);
            isInteracting = false;
            showUI = true;
        }

        private void Update()
        {
            if (!isInteracting)
            {
                if (Input.GetKeyDown(AKInputManager.instance.openInventoryKey))
                {
                    showUI = !showUI;

                    if (showUI)
                    {
                        adventureKitCanvas.SetActive(true);
                        isInventoryOpen = true;
                        AKDisableManager.instance.DisablePlayerInventory(true);

                        if (hasChessPiece)
                        {
                            if (chessPuzzleCanvas)
                            {
                                chessPuzzleCanvas.SetActive(true);
                            }
                            else
                            {
                                Debug.Log("Add the Chess piece canvas to avoid errors!");
                            }
                        }

                        if (hasFlashlight)
                        {
                            if (flashlightUICanvas)
                            {
                                flashlightUICanvas.SetActive(true);
                            }
                            else
                            {
                                Debug.Log("Add the Flashlight Canvas to avoid errors!");
                            }
                        }

                        if (hasJerrycan)
                        {
                            if (generatorUICanvas)
                            {
                                generatorUICanvas.SetActive(true);
                            }
                            else
                            {
                                Debug.Log("Add the Generator Canvas to avoid errors!");
                            }
                        }

                        if (hasGasMask)
                        {
                            if (gasMaskUICanvas)
                            {
                                gasMaskUICanvas.SetActive(true);
                            }
                            else
                            {
                                Debug.Log("Add the Gas Mask Canvas to avoid errors!");
                            }
                        }

                        if (hasThemedKey)
                        {
                            if (themedKeyUICanvas)
                            {
                                themedKeyUICanvas.SetActive(true);
                            }
                            else
                            {
                                Debug.Log("Add the Themed Key Canvas to avoid errors!");
                            }
                        }

                        if (hasFuse)
                        {
                            if (fuseBoxCanvas)
                            {
                                fuseBoxCanvas.SetActive(true);
                            }
                            else
                            {
                                Debug.Log("Add the Fuse Box Canvas to avoid errors!");
                            }
                        }
                    }

                    else
                    {
                        adventureKitCanvas.SetActive(false);
                        isInventoryOpen = false;
                        AKDisableManager.instance.DisablePlayerInventory(false);

                        if (/*hasChessPiece &&*/ chessPuzzleCanvas)
                        {
                            chessPuzzleCanvas.SetActive(false);
                        }

                        if (hasFlashlight && flashlightUICanvas)
                        {
                            flashlightUICanvas.SetActive(false);
                        }

                        if (hasJerrycan && generatorUICanvas)
                        {
                            generatorUICanvas.SetActive(false);
                        }

                        if (hasGasMask && gasMaskUICanvas)
                        {
                            gasMaskUICanvas.SetActive(false);
                        }

                        if (hasThemedKey && themedKeyUICanvas)
                        {
                            themedKeyUICanvas.SetActive(false);
                        }

                        if (hasFuse && fuseBoxCanvas)
                        {
                            fuseBoxCanvas.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
