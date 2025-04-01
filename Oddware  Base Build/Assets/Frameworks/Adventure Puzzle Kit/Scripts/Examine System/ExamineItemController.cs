using UnityEngine;
using FlashlightSystem;
using GeneratorSystem;
using GasMaskSystem;
using ThemedKeySystem;
using ChessPuzzleSystem;
using FuseboxSystem;
using AdventurePuzzleKit;

namespace ExamineSystem
{
    public class ExamineItemController : MonoBehaviour
    {
        [Header("Parent & Children Settings")]
        [Tooltip("Select this option if the object which has this script has no mesh renderer and it's an empty parent which holds children")]
        [SerializeField] private bool isEmptyParent = false;
        [Tooltip("Select this option if the object you're examining has multiple children - Add the child objects to the array")]
        [SerializeField] private bool hasChildren = false;
        [SerializeField] private GameObject[] childObjects = null;

        [Header("Item Name")]
        public string itemName;

        [Header("Can you collect it?")]
        [SerializeField] private bool isCollectable = false;
        [SerializeField] private bool showHelpUI = false;
        [SerializeField] private SystemType _systemType = SystemType.None;
        private enum SystemType { None, FlashlightSys, GeneratorSys, GasMaskSys, ThemedKeySys, ChessSys, FuseBoxSys }

        [Header("Item Name Settings")]
        [SerializeField] private int textSize = 40;
        [SerializeField] private Font fontType = null;
        [SerializeField] private FontStyle fontStyle = FontStyle.Normal;
        [SerializeField] private Color fontColor = Color.white;

        [Space(5)] [TextArea] public string itemDescription;

        [Header("Item Descriptor Settings")]
        [SerializeField] private int textSizeDesc = 32;
        [SerializeField] private Font fontTypeDesc = null;
        [SerializeField] private FontStyle fontStyleDesc = FontStyle.Normal;
        [SerializeField] private Color fontColorDesc = Color.white;

        [Header("Initial Rotation for objects")]
        [SerializeField] private Vector3 initialRotationOffset = new Vector3(0, 0, 0);

        [Header("Zoom Settings")]
        [SerializeField] private float initialZoom = 1f;
        [SerializeField] private Vector2 zoomRange = new Vector2(0.5f, 2f);
        [SerializeField] private float zoomSensitivity = 0.1f;

        [Header("Examine Rotation")]
        [SerializeField] private float horizontalSpeed = 5.0F;
        [SerializeField] private float verticalSpeed = 5.0F;

        [Header("Emissive Highlight")]
        [SerializeField] private bool showEmissionHighlight = false;
        [SerializeField] private bool showNameHighlight = false;

        [Header("Item UI Type")]
        [SerializeField] private UIType _UIType = UIType.None;

        [Header("IspectPoints - ONLY add the Inspect points that you want to appear when first examining")]
        [SerializeField] private GameObject[] inspectPoints = null;
        private LayerMask myMask;
        private bool hasInspectPoints = false;
        private float viewDistance = 25;

        [Header("Item Interaction Sound")]
        [SerializeField] private string pickupSound = "YourSound";

        private Material thisMat;
        Vector3 originalPosition;
        Quaternion originalRotation;
        private Vector3 startPos;
        private bool canRotate;
        private float currentZoom = 1;
        private const string emissive = "_EMISSION";
        private const string mouseX = "Mouse X";
        private const string mouseY = "Mouse Y";
        private const string interact = "Interact";
        private const string examineLayer = "ExamineLayer";
        private const string defaultLayer = "Default";

        private FlashlightItemController _flashlightItemController;
        private GeneratorItemController _generatorItemController;
        private GasMaskItemController _gasMaskItemController;
        private ThemedKeyItemController _themedKeyItemController;
        private ChessItemController _chessItemController;
        private FuseItemController _fuseboxItemController;

        private Camera mainCamera;
        private Transform examinePoint;

        private AdventureKitRaycast raycastManager;

        public enum UIType { None, BasicLowerUI, RightSideUI }
        private void Start()
        {
            Init();
        }

        public void Init()
        {
            myMask = 1 << LayerMask.NameToLayer("InspectPointMask");

            initialZoom = Mathf.Clamp(initialZoom, zoomRange.x, zoomRange.y);
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            startPos = gameObject.transform.localEulerAngles;

            DisableEmissionOnChildren();
            if (!isEmptyParent)
            {
                thisMat = GetComponent<Renderer>().material;
                thisMat.DisableKeyword(emissive);
            }

            if (isCollectable)
            {
                SetType();
            }

            mainCamera = Camera.main;
            raycastManager = mainCamera.GetComponent<AdventureKitRaycast>();
            examinePoint = GameObject.FindWithTag("ExaminePoint").GetComponent<Transform>();
        }

        public void MainHighlight(bool isHighlighted)
        {
            if (showNameHighlight)
            {
                if (isHighlighted)
                {
                    ExamineUIController.instance.interactionItemNameUI.text = itemName;
                    ExamineUIController.instance.interactionNameMainUI.SetActive(true);
                }
                else
                {
                    ExamineUIController.instance.interactionItemNameUI.text = itemName;
                    ExamineUIController.instance.interactionNameMainUI.SetActive(false);
                }
            }

            if (showEmissionHighlight)
            {
                if (isHighlighted)
                {
                    if (!isEmptyParent)
                    {
                        thisMat.EnableKeyword(emissive);
                    }
                    if (hasChildren)
                    {
                        foreach (GameObject gameobjectToLayer in childObjects)
                        {
                            Material thisMat = gameobjectToLayer.GetComponent<Renderer>().material;
                            thisMat.EnableKeyword(emissive);
                        }
                    }
                }
                else
                {
                    if (!isEmptyParent)
                    {
                        thisMat.DisableKeyword(emissive);
                    }
                    DisableEmissionOnChildren();
                }
            }
        }

        /// <summary>
        /// Handles adjusting the zoom amount of the object
        /// </summary>
        /// <param name="value">The distance from the camera to position the object</param>
        /// <param name="moveSelf">Whether to move the actual object. If set to false the object may not move, but only the represented point.</param>
        private void MoveZoom(float value, bool moveSelf = true)
        {
            examinePoint.transform.localPosition = new Vector3(0, 0, value);

            if(moveSelf)
            {
                transform.position = examinePoint.transform.position;
            }
        }

        public void StopInteractingObject()
        {
            if (hasChildren)
            {
                foreach (GameObject gameobjectToLayer in childObjects)
                {
                    gameobjectToLayer.layer = LayerMask.NameToLayer(defaultLayer);
                    Material thisMat = gameobjectToLayer.GetComponent<Renderer>().material;
                    thisMat.DisableKeyword(emissive);
                }
            }
            gameObject.layer = LayerMask.NameToLayer(interact);
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            ExamineUIController.instance.interestPointParentUI.SetActive(false);
            AKDisableManager.instance.DisablePlayerExamine(false);
            if (showHelpUI)
            {
                ExamineUIController.instance.ShowHelpPrompt(false);
            }
            canRotate = false;
            hasInspectPoints = false;

            switch (_UIType)
            {
                case UIType.None:
                    ExamineUIController.instance.noUICloseButton.SetActive(false);
                    break;
                case UIType.BasicLowerUI:
                    ExamineUIController.instance.basicItemNameUI.text = null;
                    ExamineUIController.instance.basicExamineUI.SetActive(false);
                    break;
                case UIType.RightSideUI:
                    ExamineUIController.instance.rightItemNameUI.text = null;
                    ExamineUIController.instance.rightExamineUI.SetActive(false);
                    break;
            }
            gameObject.SetActive(false);
        }

        public void ExamineObject()
        {
            ExamineUIController.instance.examineController = gameObject.GetComponent<ExamineItemController>();
            AKAudioManager.instance.Play(pickupSound);

            if (inspectPoints.Length >= 1)
            {
                hasInspectPoints = true;

                foreach (GameObject pointToEnable in inspectPoints)
                {
                    pointToEnable.SetActive(true);
                }
            }

            currentZoom = initialZoom; MoveZoom(initialZoom);

            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
            transform.Rotate(initialRotationOffset);

            AKDisableManager.instance.DisablePlayerExamine(true);
            ExamineUIController.instance.interactionNameMainUI.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer(examineLayer);
            if (showHelpUI)
            {
                ExamineUIController.instance.ShowHelpPrompt(true);
            }

            if (hasChildren)
            {
                foreach (GameObject gameobjectToLayer in childObjects)
                {
                    gameobjectToLayer.layer = LayerMask.NameToLayer(examineLayer);
                    Material thisMat = gameobjectToLayer.GetComponent<Renderer>().material;
                    thisMat.DisableKeyword(emissive);
                }
            }

            if (!isEmptyParent)
            {
                thisMat.DisableKeyword(emissive);
            }
            canRotate = true;

            switch (_UIType)
            {
                case UIType.None:
                    ExamineUIController.instance.noUICloseButton.SetActive(true);
                    break;
                case UIType.BasicLowerUI:
                    ExamineUIController.instance.basicItemNameUI.text = itemName;
                    ExamineUIController.instance.basicItemDescUI.text = itemDescription;
                    TextCustomisation();
                    ExamineUIController.instance.basicExamineUI.SetActive(true);
                    break;
                case UIType.RightSideUI:
                    ExamineUIController.instance.rightItemNameUI.text = itemName;
                    ExamineUIController.instance.rightItemDescUI.text = itemDescription;
                    TextCustomisation();
                    ExamineUIController.instance.rightExamineUI.SetActive(true);
                    break;
            }
        }

        private void TextCustomisation()
        {
            switch (_UIType)
            {
                case UIType.BasicLowerUI:
                    ExamineUIController.instance.basicItemNameUI.fontSize = textSize;
                    ExamineUIController.instance.basicItemNameUI.fontStyle = fontStyle;
                    ExamineUIController.instance.basicItemNameUI.font = fontType;
                    ExamineUIController.instance.basicItemNameUI.color = fontColor;
                    ExamineUIController.instance.basicItemDescUI.fontSize = textSizeDesc;
                    ExamineUIController.instance.basicItemDescUI.fontStyle = fontStyleDesc;
                    ExamineUIController.instance.basicItemDescUI.font = fontTypeDesc;
                    ExamineUIController.instance.basicItemDescUI.color = fontColorDesc;
                    break;
                case UIType.RightSideUI:
                    ExamineUIController.instance.rightItemNameUI.fontSize = textSize;
                    ExamineUIController.instance.rightItemNameUI.fontStyle = fontStyle;
                    ExamineUIController.instance.rightItemNameUI.font = fontType;
                    ExamineUIController.instance.rightItemNameUI.color = fontColor;
                    ExamineUIController.instance.rightItemDescUI.fontSize = textSizeDesc;
                    ExamineUIController.instance.rightItemDescUI.fontStyle = fontStyleDesc;
                    ExamineUIController.instance.rightItemDescUI.font = fontTypeDesc;
                    ExamineUIController.instance.rightItemDescUI.color = fontColorDesc;
                    break;
            }         
        }

        void Update()
        {
            if (canRotate)
            {
                float h = horizontalSpeed * Input.GetAxis(mouseX);
                float v = verticalSpeed * Input.GetAxis(mouseY);

                if (hasInspectPoints)
                {
                    FindInspectPoints();
                }

                if (Input.GetKey(AKInputManager.instance.rotateKey))
                {
                    gameObject.transform.Rotate(v, h, 0);
                }

                else if (Input.GetKeyDown(AKInputManager.instance.dropKey))
                {
                    StopInteractingObject();
                    raycastManager.doOnce = false;
                }

                else if(Input.GetKeyDown(AKInputManager.instance.pickupItemKey))
                {
                    if (isCollectable)
                    {
                        CollectItem();
                    }
                }

                //Handle zooming
                bool zoomAdjusted = false;
                float scrollDelta = Input.mouseScrollDelta.y;
                if (scrollDelta > 0)
                {
                    currentZoom += zoomSensitivity;
                    zoomAdjusted = true;
                }
                else if (scrollDelta < 0)
                {
                    currentZoom -= zoomSensitivity;
                    zoomAdjusted = true;
                }

                if(zoomAdjusted)
                {
                    currentZoom = Mathf.Clamp(currentZoom, zoomRange.x, zoomRange.y);
                    MoveZoom(currentZoom);
                }
            }
        }

        void FindInspectPoints()
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, viewDistance, myMask))
            {
                if (hit.transform.CompareTag("InspectPoint"))
                {
                    InspectPointUI(hit.transform.gameObject, mainCamera, true); //Enable inspect point UI
                    if (Input.GetKeyDown(AKInputManager.instance.interactKey))
                    {
                        hit.transform.gameObject.GetComponent<ExamineInspectPoint>().InspectPointInteract();
                    }
                }
                else
                {
                    InspectPointUI(null, null, false); //Disable inspect point UI
                }
            }
            else
            {
                InspectPointUI(null, null, false); //Disable inspect point UI
            }
        }

        void InspectPointUI(GameObject item, Camera camera, bool detected) // Enable/disable inspect point UI
        {
            if (detected)
            {
                ExamineUIController.instance.interestPointParentUI.SetActive(true);
                ExamineUIController.instance.interestPointParentUI.transform.position = camera.WorldToScreenPoint(item.transform.position);
                ExamineUIController.instance.interestPointText.text = item.GetComponent<ExamineInspectPoint>().InspectInformation();
            }
            else
            {
                ExamineUIController.instance.interestPointParentUI.SetActive(false); //Disable inspect UI
            }
        }

        void DisableEmissionOnChildren()
        {
            if (hasChildren)
            {
                foreach (GameObject gameobjectToLayer in childObjects)
                {
                    Material thisMat = gameobjectToLayer.GetComponent<Renderer>().material;
                    thisMat.DisableKeyword(emissive);
                }
            }
        }

        void SetType()
        {
            switch (_systemType)
            {
                case SystemType.FlashlightSys: _flashlightItemController = GetComponent<FlashlightItemController>(); break;
                case SystemType.GeneratorSys: _generatorItemController = GetComponent<GeneratorItemController>(); break;
                case SystemType.GasMaskSys: _gasMaskItemController = GetComponent<GasMaskItemController>(); break;
                case SystemType.ThemedKeySys: _themedKeyItemController = GetComponent<ThemedKeyItemController>(); break;
                case SystemType.ChessSys: _chessItemController = GetComponent<ChessItemController>(); break;
                case SystemType.FuseBoxSys: _fuseboxItemController = GetComponent<FuseItemController>(); break;
            }
        }

        void CollectItem()
        {
            switch (_systemType)
            {
                case SystemType.FlashlightSys: _flashlightItemController.ObjectInteract(); break;
                case SystemType.GeneratorSys: _generatorItemController.ObjectInteract(); break;
                case SystemType.GasMaskSys: _gasMaskItemController.ObjectInteract(); break;
                case SystemType.ThemedKeySys: _themedKeyItemController.ObjectInteract(); break;
                case SystemType.ChessSys: _chessItemController.ObjectInteract(); break;
                case SystemType.FuseBoxSys: _fuseboxItemController.ObjectInteract(); break;
            }
            StopInteractingObject();
        }

        private void OnDestroy()
        {
            Destroy(thisMat);
        }
    }
}