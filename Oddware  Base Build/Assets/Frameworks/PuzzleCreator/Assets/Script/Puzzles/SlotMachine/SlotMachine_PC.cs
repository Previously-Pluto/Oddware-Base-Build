using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine_PC : MonoBehaviour
{
    public bool SeeInspector;
    public bool helpBoxEditor;
    public int toolbarCurrentValue;
    public List<GameObject> tilesList = new List<GameObject>();
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
    public List<GotchaObject> m_gotchaObjects;

    public Camera cameraUseForFocus;
    public List<EditorMethodsList_Pc.MethodsList> methodsList      // Create a list of Custom Methods that could be edit in the Inspector
     = new List<EditorMethodsList_Pc.MethodsList>();

    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab", "Reticule Mode" };

    public AP_PuzzleDetector_Pc aP_PuzzleDetector;

    public AP_PuzzleMoveType_Pc aP_PuzzleMoveType;

    public bool b_PuzzleSolved = false;         // Know if the puzzle is solved

    public conditionsToAccessThePuzzle_Pc _conditionsToAccessThePuzzle;    // access conditionsToAccessThePuzzle component
    public actionsWhenPuzzleIsSolved_Pc _actionsWhenPuzzleIsSolved;      // access actionsWhenPuzzleIsSolved component


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
}

[System.Serializable]
public class GotchaObject
{
    public string m_name;
    public Sprite m_image;
}
