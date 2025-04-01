using System.Collections;
using System.Collections.Generic;
using TypingGameKit;
using TypingGameKit.Demo;
using UnityEngine;

public class TypingGamePuzzle_Pc : MonoBehaviour
{
    public bool SeeInspector;
    public bool helpBoxEditor;
    public int toolbarCurrentValue;
    public List<GameObject> tilesList = new List<GameObject>();

    public int initualSequenceCount;
    public GameObject anchorObject;
    public TextCollection texts;
    public float positionRange = 10f;
    public AudioClip inputFailedClip;
    public AudioClip inputSucceededClip;

    public bool distanceScaling;
    public bool overlapAvoidance;
    public bool restrictToFrame;
    public bool softMovement;

    public AudioClip a_KeyPressed;
    public float a_KeyPressedVolume;
    public AudioClip a_Reset;
    public float a_ResetVolume;
    public AudioClip a_WrongCode;
    public float a_WrongCodeVolume;
    private AudioSource a_source;


    public Camera cameraUseForFocus;
    public List<EditorMethodsList_Pc.MethodsList> methodsList      // Create a list of Custom Methods that could be edit in the Inspector
     = new List<EditorMethodsList_Pc.MethodsList>();

    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab", "Reticule Mode" };

    public AP_PuzzleDetector_Pc aP_PuzzleDetector;

    public AP_PuzzleMoveType_Pc aP_PuzzleMoveType;

    public DemoManager demoManager;

    public float rotationSpeed;

    public List<LockStruct> locks;

    public string scriptName;

    public string label;

    public bool b_PuzzleSolved = false;         // Know if the puzzle is solved
    private detectPuzzleClick_Pc _detectClick;
    public LayerMask myLayer;                        // Raycast is done only on layer 15 : Puzzle

    public conditionsToAccessThePuzzle_Pc _conditionsToAccessThePuzzle;    // access conditionsToAccessThePuzzle component
    public actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;      // access actionsWhenPuzzleIsSolved component
    private bool type2Once = false;
    private bool type1Once = false;
    public CallMethods_Pc callMethods;                        // Access script taht allow to call public function in this script.
    public bool b_OnlyTheFirstTime = true;

    private void Start()
    {
        //--> Every Puzzle  ----> BEGIN <----
        _conditionsToAccessThePuzzle = GetComponent<conditionsToAccessThePuzzle_Pc>();
        _actionsWhenPuzzleIsSolved = GetComponent<actionsWhenPuzzleIsSolved_Pc>();
        //--> Common for all puzzle ----> BEGIN <----
        _detectClick = new detectPuzzleClick_Pc();                 // Access Class that allow to detect click (Mouse, Joystick, Mobile) 
    }

    // Update is called once per frame
    void Update()
    {
        #region
        if (_conditionsToAccessThePuzzle.b_PuzzleIsActivated/* && !ingameGlobalManager.instance.b_Ingame_Pause*/)
        {
            if (b_OnlyTheFirstTime)
            {
                b_OnlyTheFirstTime = false;
                callMethods.Call_A_Method(methodsList);
            }
        }


        if (_conditionsToAccessThePuzzle.b_PuzzleIsActivated && // All Case puzzle is not solved
        _conditionsToAccessThePuzzle.b_PuzzleStateButtons

        ||

           b_PuzzleSolved &&                                    // Case Focus: Puzzle is already solved
        _conditionsToAccessThePuzzle.b_PuzzleStateButtons &&
          aP_PuzzleDetector.b_FocusActivated)
        {
            aP_PuzzleMoveType.feedbackPuzzleState(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, b_PuzzleSolved);
            aP_PuzzleMoveType.puzzleStateButtons(_detectClick, aP_PuzzleDetector, myLayer, cameraUseForFocus, GetComponent<TypingGamePuzzle_Pc>(), gameObject, b_PuzzleSolved);
        }
        #endregion
    }

    public void UpdateGameOptions()
    {
        SetDemoManagerDatas();
    }

    public void SetDemoManagerDatas()
    {
        demoManager.SetDemoManagerData(anchorObject, positionRange, texts, inputFailedClip, inputSucceededClip, locks);
    }

    //--> Actions when puzzle is solved
    public void puzzleSolved()
    {
        #region
        //-> Actions done for all type of puzzle
        if (!b_PuzzleSolved || CanvasD_Pc.instance && CanvasD_Pc.instance._P)
            _actionsWhenPuzzleIsSolved.F_PuzzleSolved();                   // Call script actionsWhenPuzzleIsSolved. Do actions when the puzzle is solved the first time.
        else
            _actionsWhenPuzzleIsSolved.b_actionsWhenPuzzleIsSolved = true; // Use when focus is called. The variable b_actionsWhenPuzzleIsSolved in script puzzleSolved equal True

        _conditionsToAccessThePuzzle.changeSpriteAndLedWhenPuzzleSolved();

        //if (_conditionsToAccessThePuzzle.ledPuzzleSolved) _conditionsToAccessThePuzzle.ledPuzzleSolved.AP_Btn_On();       // Led switch On
        //if (_conditionsToAccessThePuzzle.puzzleSprite) _conditionsToAccessThePuzzle.puzzleSprite.AP_ChangeSprite(2);   // Sprite: Solved

        if (_actionsWhenPuzzleIsSolved.objectActivatedWhenPuzzleIsSolved)
            _actionsWhenPuzzleIsSolved.objectActivatedWhenPuzzleIsSolved.SetActive(true);

        b_PuzzleSolved = true;

        aP_PuzzleDetector.b_PuzzleIsSolved = true;
        aP_PuzzleMoveType.AP_InitAfterAPuzzleSolved();
        #endregion
    }

    // Detect if Button Reset | Exit Puzzle | Clue are selected by the player
    public void feedbackPuzzleState()
    {
        #region
        Transform objClicked;
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;


        //Focus Mode
        if (aP_PuzzleDetector.b_FocusActivated)
        {
            objClicked = _detectClick.DetectPuzzleStateObject(myLayer, aP_GlobalPuzzle, 0, cameraUseForFocus);
        }
        //VR Raycast
        else if (!aP_PuzzleDetector.b_FocusActivated &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 1)
        {
            if (!b_PuzzleSolved)
                objClicked = _detectClick.DetectPuzzleStateObjectsRaycast(myLayer, true);
            else
                objClicked = null;
        }
        //VR Hand
        else if (!aP_PuzzleDetector.b_FocusActivated &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 2)
        {

            objClicked = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent.SelectedObjectPuzzleState;
        }
        //Reticule Mode
        else
        {
            objClicked = _detectClick.DetectPuzzleStateObject(myLayer, AP_GlobalPuzzleManager_Pc.instance, 0, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera());
        }


        if (objClicked != null &&
            (objClicked.transform.name == "btnPuzzleReset" ||
             objClicked.transform.name == "ExitPuzzle" ||
             objClicked.transform.name == "Clue"))
        {

            AP_GlobalPuzzleManager_Pc aP_Global = AP_GlobalPuzzleManager_Pc.instance;

            if (aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 1 &&       // Mode Raycast
                aP_Global.aP_DragAndDropParent &&
                !type1Once)
            {
                aP_Global.aP_DragAndDropParent.callMethods.Call_A_Method(aP_Global.aP_DragAndDropParent.methodsListCanGrabLogicOrGearModeRaycast);
                type1Once = true;
            }

            if (aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 2 &&       // Mode Hand
                aP_Global.aP_DragAndDropParent &&
                !type2Once)
            {
                aP_Global.aP_DragAndDropParent.callMethods.Call_A_Method(aP_Global.aP_DragAndDropParent.methodsListCanGrabLogicOrGear);
                type2Once = true;
            }

            if (aP_Global.currentPuzzleWithNoFocus &&
                aP_Global.currentPuzzleWithNoFocus.puzlleIntearctionType == 3 &&
                aP_Global.b_DesktopInputs)
            {                                             // Mode Reticule except for Mobile

                AP_Reticule_Pc s_Reticule = aP_Global.reticule.GetComponent<AP_Reticule_Pc>();

                if (s_Reticule && !s_Reticule.b_CanGrab)
                    s_Reticule.callMethodsListCanGrabReticule();
            }
        }
        else
        {
            type2Once = false;
            type1Once = false;
        }
        #endregion
    }

    public void puzzleStateButtons()
    {
        #region
        Transform objClicked;
        AP_GlobalPuzzleManager_Pc aP_GlobalPuzzle = AP_GlobalPuzzleManager_Pc.instance;


        //Focus Mode
        if (aP_PuzzleDetector.b_FocusActivated)
        {
            objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, aP_GlobalPuzzle, 0, cameraUseForFocus);
        }
        //VR Raycast
        else if (!aP_PuzzleDetector.b_FocusActivated &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 1)
        {
            if (!b_PuzzleSolved)
                objClicked = _detectClick.Dec_VRRaycastCheckClick(myLayer);
            else
                objClicked = null;
        }
        //VR Hand
        else if (!aP_PuzzleDetector.b_FocusActivated &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus &&
                 aP_GlobalPuzzle.currentPuzzleWithNoFocus.puzlleIntearctionType == 2 &&
                 ((!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(aP_GlobalPuzzle.validationButtonKeyboard) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))) && aP_GlobalPuzzle.b_DesktopInputs && !aP_GlobalPuzzle.b_Joystick)
                 ||
                  (!AP_GlobalPuzzleManager_Pc.instance.b_Pause && (Input.GetKeyDown(aP_GlobalPuzzle.validationButtonJoystick) || (AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown.Count > 0 && AP_GlobalPuzzleManager_Pc.instance.callMethods.Call_One_Bool_Method(AP_GlobalPuzzleManager_Pc.instance.methodsListVRValidationDown, 0))) && aP_GlobalPuzzle.b_DesktopInputs && aP_GlobalPuzzle.b_Joystick)))
        {

            objClicked = AP_GlobalPuzzleManager_Pc.instance.aP_DragAndDropParent.SelectedObjectPuzzleState;
        }
        //Reticule Mode
        else
        {
            objClicked = _detectClick.Dec_F_detectPuzzleClick(myLayer, AP_GlobalPuzzleManager_Pc.instance, 0, AP_GlobalPuzzleManager_Pc.instance.returnMainCamera());
        }


        if (objClicked != null && objClicked.transform.name == "btnPuzzleReset" && !b_PuzzleSolved) // Player press button reset 
        {
            F_ResetPuzzle();
            Debug.Log("Reset");
        }
        else if (objClicked != null && objClicked.transform.name == "ExitPuzzle") // Player press button Exit puzzle 
        {
            if (aP_PuzzleDetector.b_FocusActivated)
            {
                Debug.Log("Exit Puzzle");
                aP_PuzzleDetector.Ap_DeactivatePuzzle();
            }
            else
            {
                Debug.Log("Focus is not activate so you can delete the Object ExitPuzzle inside the puzzle.");
            }

        }
        else if (objClicked != null && objClicked.transform.name == "Clue") // Player press button Clue
        {
            Debug.Log("Display Clue");
            AP_GlobalPuzzleManager_Pc.instance.AP_DisplayClue(gameObject.transform);
        }
        #endregion
    }


    //--> Reset Puzzle when button iconResetPuzzle in Canvas_PlayerInfos is pressed
    public void F_ResetPuzzle()
    {
        #region
        demoManager.ResetDemo();
        #endregion
    }

    //--> Use to load object state. Initialize the puzzle  (T = True or F = False)
    public void saveSystemInitGameObject(string s_ObjectDatas)
    {
        #region
        GetComponent<conditionsToAccessThePuzzle_Pc>().checkAccessAllowed();
        //if(!b_PuzzleSolved)
        //   _actionsWhenPuzzleIsSolved.initSolvedSection();    // Init Popup object in script actionsWhenPuzzleIsSolved.cs
        //----> END <----
        #endregion
    }
}
