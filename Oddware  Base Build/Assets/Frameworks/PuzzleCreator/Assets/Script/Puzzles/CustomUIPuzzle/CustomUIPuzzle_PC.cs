using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CUSTOM_UI_GAME_TYPE
{
    PRESS_THE_BUTTON,
    COLOR,
    NUMBER
}

public enum CUSTOM_UI_COLORS
{
    RED,
    GREEN,
    BLUE,
    PINK,
    YELLOW
}


public enum CUSTOM_UI_NUMBERS
{
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    SEVEN,
    EIGHT,
    NINE,
    TEN
}
public class CustomUIPuzzle_PC : MonoBehaviour
{
    public bool SeeInspector;
    public bool helpBoxEditor;
    public int toolbarCurrentValue;
    public List<GameObject> tilesList = new List<GameObject>();

    public GameObject anchorObject;
    public AudioClip inputFailedClip;
    public AudioClip inputSucceededClip;
    public CUSTOM_UI_GAME_TYPE customUIGameType;

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

    public float rotationSpeed;

    public List<LockStruct> locks;

    public string scriptName;

    public string label;

    public bool b_PuzzleSolved = false;         // Know if the puzzle is solved

    public conditionsToAccessThePuzzle_Pc _conditionsToAccessThePuzzle;    // access conditionsToAccessThePuzzle component
    public actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;      // access actionsWhenPuzzleIsSolved component
    public int currentIndex;
    public TextMeshProUGUI m_hintText;

    private void OnEnable()
    {
        UpdateHintText();
    }

    public void UpdateHintText()
    {
        CUSTOM_UI_COLORS[] colors = (CUSTOM_UI_COLORS[])Enum.GetValues(typeof(CUSTOM_UI_COLORS));
        CUSTOM_UI_NUMBERS[] numbers = (CUSTOM_UI_NUMBERS[])Enum.GetValues(typeof(CUSTOM_UI_NUMBERS));
        if (customUIGameType == CUSTOM_UI_GAME_TYPE.NUMBER)
            m_hintText.text = "PRESS " + numbers[currentIndex].ToString();
        if (customUIGameType == CUSTOM_UI_GAME_TYPE.COLOR)
            m_hintText.text = "PRESS " + colors[currentIndex].ToString();

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

    public bool VerifiedIfTheClickedButtonIsCorrect(CUSTOM_UI_COLORS _color, CUSTOM_UI_NUMBERS _number)
    {
        if (customUIGameType == CUSTOM_UI_GAME_TYPE.PRESS_THE_BUTTON)
        {
            puzzleSolved();
            return true;
        }
        else if (customUIGameType == CUSTOM_UI_GAME_TYPE.NUMBER)
        {
            CUSTOM_UI_NUMBERS[] numbers = (CUSTOM_UI_NUMBERS[])Enum.GetValues(typeof(CUSTOM_UI_NUMBERS));
            if (currentIndex < numbers.Length - 1 && _number.ToString().Equals(numbers[currentIndex].ToString()))
            {
                currentIndex++;
                UpdateHintText();
                return true;
            }
            else if (currentIndex == numbers.Length - 1)
            {
                puzzleSolved();
                return true;
            }
            else
                return false;
        }
        else if (customUIGameType == CUSTOM_UI_GAME_TYPE.COLOR)
        {
            CUSTOM_UI_COLORS[] colors = (CUSTOM_UI_COLORS[])Enum.GetValues(typeof(CUSTOM_UI_COLORS));
            if (currentIndex < colors.Length - 1 && _color.ToString().Equals(colors[currentIndex].ToString()))
            {
                currentIndex++;
                UpdateHintText();
                return true;
            }
            else if (currentIndex == colors.Length - 1)
            {
                puzzleSolved();
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }
    }
}
