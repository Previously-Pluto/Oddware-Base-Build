﻿// Description : Custom Editor for PipesPuzzle_Pc.cs
#if (UNITY_EDITOR)
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(PipesPuzzle_Pc))]
public class PipesPuzzleEditor_Pc : Editor
{
    SerializedProperty SeeInspector;                                            // use to draw default Inspector

    SerializedProperty pipeType;

    SerializedProperty startTile;
    SerializedProperty endTile;

    SerializedProperty mazeStartTileX;
    SerializedProperty mazeStartTileY;

    SerializedProperty mazeEndTileX;
    SerializedProperty mazeEndTileY;

    SerializedProperty PipesSolutionList;
    SerializedProperty linkPipes;
    SerializedProperty b_SelectPipessToLink;

    SerializedProperty _NumberOfKey;
    SerializedProperty _Column;
    SerializedProperty tilesList;
    SerializedProperty toolbarCurrentValue;
    SerializedProperty SquareSize;
    SerializedProperty currentSelectedSprite;
    SerializedProperty PipesPositionList;

    SerializedProperty PipesTypeList;
    SerializedProperty selectDefaultTile;
 
    SerializedProperty helpBoxEditor;

    SerializedProperty a_KeyPressed;
    SerializedProperty a_KeyPressedVolume;
    SerializedProperty a_Reset;
    SerializedProperty a_ResetVolume;

    SerializedProperty cameraUseForFocus;
    SerializedProperty methodsList;
    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;


    public Color _cBlue = new Color(0f, .9f, 1f, .5f);
    public Color _cRed = new Color(1f, .5f, 0f, .5f);
    public Color _cGray = new Color(.9f, .9f, .9f, 1);
    public Color _cGreen = Color.green;

    public Transform    spriteTransform;   // Use to display key sprite
    public Text         tmpText;        // use to display Key Text
    public Transform    objPIVOT;

    public string[]     toolbarStrings = new string[] { "Generate","Pipe Creation", "Link (Optional)", "Init Position", "Game Options" };
    public string[]     PipesTypeStrings = new string[] { "Empty", "Vertical","T","Elbow","No Move"};

    private Texture2D MakeTex(int width, int height, Color col)
    {                       // use to change the GUIStyle
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private Texture2D Tex_01;
    private Texture2D Tex_02;
    private Texture2D Tex_03;
    private Texture2D Tex_04;
    private Texture2D Tex_05;


    private Texture2D Tex_Empty;

    private Texture2D Tex_Vertical_01;
    private Texture2D Tex_Vertical_02;

    private Texture2D Tex_T_01;
    private Texture2D Tex_T_02;
    private Texture2D Tex_T_03;
    private Texture2D Tex_T_04;

    private Texture2D Tex_Elbow_01;
    private Texture2D Tex_Elbow_02;
    private Texture2D Tex_Elbow_03;
    private Texture2D Tex_Elbow_04;

    private Texture2D Tex_NoMove;

    public EditorManipulate2DTexture_Pc manipulate2DTex;

    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab", "Reticule Mode" };
    SerializedProperty aP_PuzzleDetector;
   
     void OnEnable()
    {
        #region
        manipulate2DTex = new EditorManipulate2DTexture_Pc();
        // Setup the SerializedProperties.
        SeeInspector = serializedObject.FindProperty("SeeInspector");
        helpBoxEditor = serializedObject.FindProperty("helpBoxEditor");

        PipesPuzzle_Pc myScript = (PipesPuzzle_Pc)target;

        pipeType = serializedObject.FindProperty("pipeType");

        startTile = serializedObject.FindProperty("startTile");
        endTile = serializedObject.FindProperty("endTile");

        mazeStartTileX = serializedObject.FindProperty("mazeStartTileX");
        mazeStartTileY = serializedObject.FindProperty("mazeStartTileY");

        mazeEndTileX = serializedObject.FindProperty("mazeEndTileX");
        mazeEndTileY = serializedObject.FindProperty("mazeEndTileY");

        PipesSolutionList = serializedObject.FindProperty("PipesSolutionList");
        linkPipes = serializedObject.FindProperty("linkPipes");
        b_SelectPipessToLink = serializedObject.FindProperty("b_SelectPipessToLink");

        _NumberOfKey = serializedObject.FindProperty("_NumberOfKey");

        _Column = serializedObject.FindProperty("_Column");
        toolbarCurrentValue = serializedObject.FindProperty("toolbarCurrentValue");
        SquareSize = serializedObject.FindProperty("SquareSize");

        tilesList = serializedObject.FindProperty("tilesList");

        currentSelectedSprite = serializedObject.FindProperty("currentSelectedSprite");
        PipesPositionList = serializedObject.FindProperty("PipesPositionList");

        PipesTypeList = serializedObject.FindProperty("PipesTypeList");

     
        selectDefaultTile = serializedObject.FindProperty("selectDefaultTile");

        a_KeyPressed = serializedObject.FindProperty("a_KeyPressed");
        a_KeyPressedVolume = serializedObject.FindProperty("a_KeyPressedVolume");
        a_Reset = serializedObject.FindProperty("a_Reset");
        a_ResetVolume = serializedObject.FindProperty("a_ResetVolume");

        cameraUseForFocus = serializedObject.FindProperty("cameraUseForFocus");
        methodsList = serializedObject.FindProperty("methodsList");
        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();

        aP_PuzzleDetector = serializedObject.FindProperty("aP_PuzzleDetector");
      
        Tex_01 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_02 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_03 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_04 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_05 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        #endregion
    }

    public override void OnInspectorGUI()
    {
        #region
        if (SeeInspector.boolValue)                         // If true Default Inspector is drawn on screen
            DrawDefaultInspector();

        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("See Inspector :", GUILayout.Width(85));
            EditorGUILayout.PropertyField(SeeInspector, new GUIContent(""), GUILayout.Width(30));
            EditorGUILayout.LabelField("See Help Boxes :", GUILayout.Width(85));
            EditorGUILayout.PropertyField(helpBoxEditor, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;

        PipesPuzzle_Pc myScript = (PipesPuzzle_Pc)target;


        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Puzzle could not be edited in play mode", MessageType.Info);
        }
        else
        {
// --> Display Tab sections in the Inspector
            EditorGUI.BeginChangeCheck();
            toolbarCurrentValue.intValue = GUILayout.Toolbar(toolbarCurrentValue.intValue, toolbarStrings);

            bool b_TilesExist = true;
            if (tilesList.arraySize > 0)
            {

                for (var i = 0; i < tilesList.arraySize; i++)
                {
                    if (tilesList.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    {
                        b_TilesExist = false;
                        break;
                    }
                }
            }

    //-> Update the tab visualization and scene view if nedded
            if (EditorGUI.EndChangeCheck())
            {                
                if ((toolbarCurrentValue.intValue == 0) || (toolbarCurrentValue.intValue == 1) || (toolbarCurrentValue.intValue == 2)){
                    loadPipesPosition(myScript,  null);
                }
                   
                if (toolbarCurrentValue.intValue == 3 ){
                    loadPipesPosition(myScript, null);
                }
            }

// --> Display Generate Section
            if (toolbarCurrentValue.intValue == 0)
                displayGeneratePuzzleSection(myScript, style_Orange);

// --> Display Link Section
            if (toolbarCurrentValue.intValue == 2)
                LinkSection(myScript, style_Orange);

// --> Display Other Section
            if (toolbarCurrentValue.intValue == 4)
                otherSection(myScript, style_Orange);


            if (tilesList.arraySize > 0)
            {
                if (b_TilesExist)
                {
// --> Display Pipe Creation
                    if (toolbarCurrentValue.intValue == 3)
                        displaySelectSpriteSection(myScript, style_Blue);
// --> Display Init Position Section
                    if (toolbarCurrentValue.intValue == 1)
                        displaySolutionSection(myScript, style_Yellow_01);
                }
                else
                {
                    if (toolbarCurrentValue.intValue == 3 || toolbarCurrentValue.intValue == 1)
                        puzzleNeedToBeGenerated();
                }
            }
        }


        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.LabelField("");
        #endregion
    }

    private void puzzleNeedToBeGenerated()
    {
        EditorGUILayout.HelpBox("You need to generate the puzzle first.", MessageType.Error);
    }

    //--> Section : Game Options
    private void otherSection(PipesPuzzle_Pc myScript, GUIStyle style_Orange)
    {
        #region
        EditorGUILayout.BeginVertical(style_Orange);

            EditorGUILayout.HelpBox("Section : Other Options.", MessageType.Info);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Camera Use for focus : ", GUILayout.Width(180));
            EditorGUILayout.PropertyField(cameraUseForFocus, new GUIContent(""));
        string camState = "Test";
        Camera refObj = (Camera)cameraUseForFocus.objectReferenceValue;
        if (refObj.gameObject.activeInHierarchy)
        {
            camState = "Stop";
        }

        if (GUILayout.Button(camState, GUILayout.Width(70)))
        {
            SerializedObject serializedObject4 = new UnityEditor.SerializedObject(myScript.cameraUseForFocus.gameObject);
            SerializedProperty m_IsActive = serializedObject4.FindProperty("m_IsActive");
            serializedObject4.Update();
            if (camState == "Test")
            {
                camState = "Stop";
                m_IsActive.boolValue = true;
                Selection.activeTransform = myScript.cameraUseForFocus.transform.parent;
            }
            else if (camState == "Stop")
            {
                camState = "Test";
                m_IsActive.boolValue = false;
            }

            serializedObject4.ApplyModifiedProperties();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Puzzle Detetcor : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(aP_PuzzleDetector, new GUIContent(""));
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("");

            EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Play Audio when Pipes is pressed : ", GUILayout.Width(180));
                EditorGUILayout.PropertyField(a_KeyPressed, new GUIContent(""), GUILayout.Width(100));
                GUILayout.Label("Volume : ", GUILayout.Width(60));
                a_KeyPressedVolume.floatValue = EditorGUILayout.Slider(a_KeyPressedVolume.floatValue, 0, 1);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Play Audio when puzzle is Reset : ", GUILayout.Width(180));
                EditorGUILayout.PropertyField(a_Reset, new GUIContent(""), GUILayout.Width(100));
                GUILayout.Label("Volume : ", GUILayout.Width(60));
                a_ResetVolume.floatValue = EditorGUILayout.Slider(a_ResetVolume.floatValue, 0, 1);
            EditorGUILayout.EndHorizontal();

        GUILayout.Label("");


        PuzzleDetection(style_Orange, style_Orange);
        GUILayout.Label("");
        displayFirstTimePuzzle(style_Orange, style_Orange);


        EditorGUILayout.EndVertical();
        #endregion
    }

    private void PuzzleDetection(GUIStyle _color_01, GUIStyle _color_02)
    {
        #region
        EditorGUILayout.BeginVertical(_color_01);

        PipesPuzzle_Pc myScript = (PipesPuzzle_Pc)target;

        SerializedObject serializedObject3 = new UnityEditor.SerializedObject(myScript.gameObject.GetComponent<AP_PuzzleMoveType_Pc>());

        SerializedProperty dragAndDropMode = serializedObject3.FindProperty("puzzleMoveMode");

        serializedObject3.Update();

        GUILayout.Label("Puzzle Detection Options:", EditorStyles.boldLabel);
        /*if (helpBoxEditor.boolValue)
        {
            EditorGUILayout.HelpBox("Desktop: (0) Focus Mode and (3) Free Mode: Reticule" +
                                    "\nVR: Free Mode: (1) VR Raycast and Free Mode: (2) VR Hand", MessageType.Info);

            EditorGUILayout.HelpBox("VR: Free Mode: (1) VR Raycast and Free Mode: (2) VR Hand doesn't work on Mobile", MessageType.Warning);
        }*/


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Mode: ", GUILayout.Width(170));
        dragAndDropMode.intValue = EditorGUILayout.Popup(dragAndDropMode.intValue, listDragAndDropMode);
        EditorGUILayout.EndHorizontal();

        if(dragAndDropMode.intValue == 3){
            Transform[] allChildren = myScript.gameObject.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<AP_PuzzleDetector_Pc>())
                {
                    SerializedObject serializedObject4 = new UnityEditor.SerializedObject(child.GetComponent<AP_PuzzleDetector_Pc>());
                    SerializedProperty b_ReticuleState = serializedObject4.FindProperty("b_ReticuleState");
                    serializedObject4.Update();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Reticule : ", GUILayout.Width(170));
                    EditorGUILayout.PropertyField(b_ReticuleState, new GUIContent(""), GUILayout.Width(100));
                    EditorGUILayout.EndHorizontal();

                    serializedObject4.ApplyModifiedProperties();
                    break;
                }
            } 
        }


        if (aP_PuzzleDetector.objectReferenceValue && myScript.aP_PuzzleDetector)
        {
            SerializedObject serializedObject5 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector);
            SerializedProperty b_FocusActivated = serializedObject5.FindProperty("b_FocusActivated");

            serializedObject5.Update();

            if (dragAndDropMode.intValue == 0 && !b_FocusActivated.boolValue)
                b_FocusActivated.boolValue = true;
            else if ((dragAndDropMode.intValue != 0) && b_FocusActivated.boolValue)
                b_FocusActivated.boolValue = false;

            if (b_FocusActivated.boolValue)
                GUILayout.Label("(This puzzle use focus.)");
            else
                GUILayout.Label("(This puzzle do not use focus.)");


            serializedObject5.ApplyModifiedProperties();
        }

        updatePuzlleDetectorPosition(dragAndDropMode.intValue);
        serializedObject3.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();
        #endregion
    }

    //--> Scetion : Generate
    private void displayGeneratePuzzleSection(PipesPuzzle_Pc myScript, GUIStyle style_Orange)
    {
        #region
        EditorGUILayout.BeginVertical(style_Orange);
        EditorGUILayout.HelpBox("Section : Generate Pipes.", MessageType.Info);
        _helpBox(0);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("How many Pipe :", GUILayout.Width(150));
        EditorGUILayout.PropertyField(_NumberOfKey, new GUIContent(""), GUILayout.Width(30));

        EditorGUILayout.LabelField("(2 minimum)");
        if (_NumberOfKey.intValue < 1)
            _NumberOfKey.intValue = 1;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Column :", GUILayout.Width(150));
        EditorGUILayout.PropertyField(_Column, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Generate Pipe"))
        {
            GenerateKeys(myScript);
        }
        EditorGUILayout.EndVertical();
        #endregion
    }

//--> Link Section
    private void LinkSection(PipesPuzzle_Pc myScript, GUIStyle style_Yellow_01)
    {
        #region
        EditorGUILayout.BeginVertical(style_Yellow_01);
            EditorGUILayout.HelpBox("Section : Link Pipes.", MessageType.Info);

            _helpBox(2);

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Choose Parent Pipes :", GUILayout.Width(120));
                if(!b_SelectPipessToLink.boolValue)
                    GUI.backgroundColor = _cRed;
                else
                    GUI.backgroundColor = _cGray;
                if (GUILayout.Button("Activate Mode", GUILayout.Width(120)))
                {
                    b_SelectPipessToLink.boolValue = false;
                }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Choose Linked Pipes :", GUILayout.Width(120));

                if (b_SelectPipessToLink.boolValue)
                    GUI.backgroundColor = _cBlue;
                else
                    GUI.backgroundColor = _cGray;
                if (GUILayout.Button("Activate Mode", GUILayout.Width(120)))
                {
                    b_SelectPipessToLink.boolValue = true;
                }
            EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        displayPipesInTheInspectorSectionLink(myScript, style_Yellow_01, 0);

        EditorGUILayout.LabelField("");
        #endregion
    }

//--> Section Pipe Creation
    private void displaySolutionSection(PipesPuzzle_Pc myScript, GUIStyle style_Yellow_01)
    {
        #region
        EditorGUILayout.BeginVertical(style_Yellow_01);
            EditorGUILayout.HelpBox("Section : Pipe Creation. " +
                                    "\n\n1-Select a pipe Type in the list by clicking on it." +
                                    "\n2-Click on a tile in the table to apply it." +
                                    "\n3-Rotate a pipe by clicking on the small button on the left under the panel." +
                                    "\n4-Deactivate/Activate a pipe by clicking on the small button on the right under the panel." +
                                    "\n\n5-Create the solution by pressing the squares below.(Show modification in scene view)." 
                                    , MessageType.Info);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset Solution"))
            {
                ResetPosition(myScript, PipesSolutionList, null);
               
            }

            EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = returnBackgroundColor(0);
            if (GUILayout.Button(myScript.pipeSprite[0], GUILayout.Width(60), GUILayout.Height(60))){
                pipeType.intValue = 0;
            }
            GUI.backgroundColor = returnBackgroundColor(1);
            if (GUILayout.Button(myScript.pipeSprite[1], GUILayout.Width(60), GUILayout.Height(60)))
            {
                pipeType.intValue = 1;
            }
            GUI.backgroundColor = returnBackgroundColor(2);
            if (GUILayout.Button(myScript.pipeSprite[2], GUILayout.Width(60), GUILayout.Height(60)))
            {
                pipeType.intValue = 2;
            }
            GUI.backgroundColor = returnBackgroundColor(3);
            if (GUILayout.Button(myScript.pipeSprite[3], GUILayout.Width(60), GUILayout.Height(60)))
            {
                pipeType.intValue = 3;
            }
            GUI.backgroundColor = returnBackgroundColor(4);
            if (GUILayout.Button(myScript.pipeSprite[4], GUILayout.Width(60), GUILayout.Height(60)))
            {
                pipeType.intValue = 4;
            }
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = _cGray;


        EditorGUILayout.BeginVertical(style_Yellow_01);

            if (GUILayout.Button("Update start and End Position"))
            {
                updatemazeStartEndTile(myScript);
            }

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Start Pipe Number :", GUILayout.Width(100));

                EditorGUI.BeginChangeCheck();
                startTile.intValue = EditorGUILayout.IntSlider(startTile.intValue, 0, _NumberOfKey.intValue - 1, GUILayout.Width(150));

                if (EditorGUI.EndChangeCheck())
                {
                    updatemazeStartEndTile(myScript);
                }

                EditorGUILayout.LabelField("Green : (Min : 0 )");
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("End Pipe Number :", GUILayout.Width(100));

                EditorGUI.BeginChangeCheck();
                endTile.intValue = EditorGUILayout.IntSlider(endTile.intValue, 0, _NumberOfKey.intValue - 1, GUILayout.Width(150));

                if (EditorGUI.EndChangeCheck())
                {
                    updatemazeStartEndTile(myScript);
                }

                EditorGUILayout.LabelField("Red : (Max :" + (_NumberOfKey.intValue - 1).ToString() + ")" );
            EditorGUILayout.EndHorizontal();

            if(startTile.intValue == endTile.intValue)
                EditorGUILayout.HelpBox("Start pipe and End pipe number must be different.", MessageType.Error);

        EditorGUILayout.EndVertical();

        displayKeysInTheInspector(myScript, style_Yellow_01, 1);

        EditorGUILayout.LabelField("");
        #endregion
    }


//--> Update the Start and End position in the scene view
    private void updatemazeStartEndTile(PipesPuzzle_Pc myScript)
    {
        #region
        int raw = Mathf.RoundToInt(_NumberOfKey.intValue / _Column.intValue);
       // Debug.Log("raw : " + raw);

        for (int i = 0; i <= raw; i++)
        {         // Column
            for (int j = 0; j < _Column.intValue; j++)       // Raw
            {
                if (startTile.intValue == j + i * _Column.intValue)
                {
                    /*Debug.Log("Start Tile : " + startTile.intValue + " : Raw : " + i + " : Column : " + j);*/
                    mazeStartTileX.intValue = ((j + 1) * 3) - 1;
                    mazeStartTileY.intValue = ((i + 1) * 3) - 1;
                    //break;
                }

                if (endTile.intValue == j + i * _Column.intValue)
                {
                    /*Debug.Log("End Tile : " + endTile.intValue + " : Raw : " + i + " : Column : " + j);*/
                    mazeEndTileX.intValue = ((j + 1) * 3) - 1;
                    mazeEndTileY.intValue = ((i + 1) * 3) - 1;
                    //break;
                }
            }
        }

        ActivateDeactivateBackgroundSprite(myScript);
        #endregion
    }


//--> Return valid exit position for a specific pipe depending his rotation and type
    private bool[] returnValidExitPosition(int selectedPosition){
        #region
        bool[] arrExistPosition = new bool[4] { false, false, false, false };

        if (PipesTypeList.GetArrayElementAtIndex(selectedPosition).intValue == 0)           // no position
        {
        }

        if (PipesTypeList.GetArrayElementAtIndex(selectedPosition).intValue == 1)           // Vertical
        {
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 0)
                arrExistPosition = new bool[4] { true, false, true, false };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 1)
                arrExistPosition = new bool[4] { false, true, false, true };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 2)
                arrExistPosition = new bool[4] { true, false, true, false };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 3)
                arrExistPosition = new bool[4] { false, true, false, true };
        }

        if (PipesTypeList.GetArrayElementAtIndex(selectedPosition).intValue == 2)           // T
        {
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 0)
                arrExistPosition = new bool[4] { true, true, true, false };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 1)
                arrExistPosition = new bool[4] { false, true, true, true };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 2)
                arrExistPosition = new bool[4] { true, false, true, true };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 3)
                arrExistPosition = new bool[4] { true, true, false, true };
        }


        if (PipesTypeList.GetArrayElementAtIndex(selectedPosition).intValue == 3)           // elbow
        {
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 0)
                arrExistPosition = new bool[4] { true, true, false, false };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 1)
                arrExistPosition = new bool[4] { false, true, true, false };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 2)
                arrExistPosition = new bool[4] { false, false, true, true };
            if (PipesSolutionList.GetArrayElementAtIndex(selectedPosition).intValue == 3)
                arrExistPosition = new bool[4] { true, false, false, true };
        }

        if (PipesTypeList.GetArrayElementAtIndex(selectedPosition).intValue == 4)           // Vertical
        {
            arrExistPosition = new bool[4] { true, true, true, true };
                
        }


        return arrExistPosition;
        #endregion
    }

    private Color returnBackgroundColor(int value){
        #region
        if (pipeType.intValue == value)
            return _cBlue;
        else
            return _cGray;
        #endregion
    }

    private Color returnBackgroundSelectedColor(int value)
    {
        #region
        if (value == 0)
            return _cBlue;
        else if (value == 1)
            return _cRed;
        else if (value == 2)
            return _cGreen;
        else
            return _cGray;
        #endregion
    }
   
//--> Section : Pipe Creation
    private void displaySelectSpriteSection(PipesPuzzle_Pc myScript, GUIStyle style_Blue)
    {
        #region
        EditorGUILayout.BeginVertical(style_Blue);
        EditorGUILayout.HelpBox("Section : Pipes Initial Position. " +
                                "\n\n1-Pivot a pipe by clicking on the small button on the left under the panel." +
                                "\n\n2-Create the Pipes initial position by pressing the squares below.(Show modification in scene view).", MessageType.Info);


        if (GUILayout.Button("Reset Position"))
        {
            ResetPosition(myScript,PipesPositionList,null);
        }

        EditorGUILayout.EndVertical();
       
        displayKeysInTheInspector(myScript, style_Blue, 0);
        #endregion
    }

//--> Display square that represent puzzle object
    public void displayKeysInTheInspector(PipesPuzzle_Pc myScript, GUIStyle style_Blue, int WhichSection)
    {
        #region
        EditorGUILayout.LabelField("");

        int number = 0;
        int raw = Mathf.RoundToInt(_NumberOfKey.intValue / _Column.intValue);

        for (var i = 0; i <= raw; i++)
        {
            EditorGUILayout.BeginHorizontal();

            for (var j = 0; j < _Column.intValue; j++)
            {
                tmpText = null;
                if (tilesList.arraySize > number)
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(SquareSize.intValue));
                    EditorGUILayout.BeginVertical();
                    //manipulate2DTex.FlippeTexture90 (DisplayTexture)
                    Texture2D spriteWithRotation = null;
                    if (WhichSection == 0)          //-> Section : Pipes Init Position
                    {
                        if (endTile.intValue == number || startTile.intValue == number){
                            if (PipesSolutionList.GetArrayElementAtIndex(number).intValue == 0)
                                spriteWithRotation = myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue];
                            if (PipesSolutionList.GetArrayElementAtIndex(number).intValue == 1)
                                spriteWithRotation = manipulate2DTex.FlippeTexture90(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]);
                            if (PipesSolutionList.GetArrayElementAtIndex(number).intValue == 2)
                                spriteWithRotation = manipulate2DTex.FlippeTexture180(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]);
                            if (PipesSolutionList.GetArrayElementAtIndex(number).intValue == 3)
                                spriteWithRotation = manipulate2DTex.FlippeTexture270(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]);
                        }
                        else{
                            if (PipesPositionList.GetArrayElementAtIndex(number).intValue == 0)
                                spriteWithRotation = myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue];
                            if (PipesPositionList.GetArrayElementAtIndex(number).intValue == 1)
                                spriteWithRotation = manipulate2DTex.FlippeTexture90(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]);
                            if (PipesPositionList.GetArrayElementAtIndex(number).intValue == 2)
                                spriteWithRotation = manipulate2DTex.FlippeTexture180(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]);
                            if (PipesPositionList.GetArrayElementAtIndex(number).intValue == 3)
                                spriteWithRotation = manipulate2DTex.FlippeTexture270(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]); 
                        }

                    }
                    if (WhichSection == 1)          //-> Section : Pipes Solution
                    {
                        if (PipesSolutionList.GetArrayElementAtIndex(number).intValue == 0)
                            spriteWithRotation = myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue];
                        if (PipesSolutionList.GetArrayElementAtIndex(number).intValue == 1)
                            spriteWithRotation = manipulate2DTex.FlippeTexture90(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]);
                        if (PipesSolutionList.GetArrayElementAtIndex(number).intValue == 2)
                            spriteWithRotation = manipulate2DTex.FlippeTexture180(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]);
                        if (PipesSolutionList.GetArrayElementAtIndex(number).intValue == 3)
                            spriteWithRotation = manipulate2DTex.FlippeTexture270(myScript.pipeSprite[PipesTypeList.GetArrayElementAtIndex(number).intValue]);
                    }

                    if (startTile.intValue == number)
                    GUI.backgroundColor = returnBackgroundSelectedColor(2);
                    else if (endTile.intValue == number)
                        GUI.backgroundColor = returnBackgroundSelectedColor(1);
                    else
                        GUI.backgroundColor = returnBackgroundSelectedColor(3);
                    if (GUILayout.Button(spriteWithRotation, GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                    {
                        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();
                        foreach (Transform child in ts)
                        {
                            if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == number)
                            {
                                objPIVOT = child;
                            }
                        }

                        currentSelectedSprite.intValue = number;

                        if (WhichSection == 0)          //-> Section : Pipes Init Position
                        {}

                        if (WhichSection == 1)          //-> Section : Solution 
                        {
                            Undo.RegisterFullObjectHierarchyUndo(objPIVOT, objPIVOT.name);

                            PipesTypeList.GetArrayElementAtIndex(number).intValue = pipeType.intValue;      // Choose pipe type for this pipe Tile
                            PipesSolutionList.GetArrayElementAtIndex(number).intValue = 0;

                            objPIVOT.transform.localEulerAngles = new Vector3(objPIVOT.transform.localEulerAngles.x,
                                                                              objPIVOT.transform.localEulerAngles.y,
                                                                                0);

                            updateEditorSpriteVisualization(myScript,number);
                        }
                    }


                    // Button to rotate the pipe and its link pipes
                    EditorGUILayout.BeginHorizontal();
                    if (startTile.intValue != number && endTile.intValue != number && WhichSection == 0
                       || 
                        WhichSection == 1)
                    {
                        if (GUILayout.Button("", GUILayout.Width(SquareSize.intValue * .65f), GUILayout.Height(SquareSize.intValue / 3)))
                        {
                            currentSelectedSprite.intValue = number;

                            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();
                            foreach (Transform child in ts)
                            {
                                if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == number)
                                {
                                    objPIVOT = child;
                                }
                            }


                            if (WhichSection == 0
                                && PipesTypeList.GetArrayElementAtIndex(number).intValue != 0
                                && PipesTypeList.GetArrayElementAtIndex(number).intValue != 4)          //-> Section : Pipes Init Position
                            {
                                Undo.RegisterFullObjectHierarchyUndo(objPIVOT, objPIVOT.name);

                                PipesPositionList.GetArrayElementAtIndex(number).intValue++;
                                PipesPositionList.GetArrayElementAtIndex(number).intValue %= 4;

                                movePipes(myScript, PipesPositionList.GetArrayElementAtIndex(number), null, 0, false);
                                ap_MoveLinkedPipes(myScript, 90, 3);                          // Move Linked Pipess
                            }
                            if (WhichSection == 1
                                && PipesTypeList.GetArrayElementAtIndex(number).intValue != 0
                                && PipesTypeList.GetArrayElementAtIndex(number).intValue != 4)          //-> Section : Solution 
                            {
                                Undo.RegisterFullObjectHierarchyUndo(objPIVOT, objPIVOT.name);

                                PipesSolutionList.GetArrayElementAtIndex(number).intValue++;
                                PipesSolutionList.GetArrayElementAtIndex(number).intValue %= 4;

                                movePipes(myScript, PipesSolutionList.GetArrayElementAtIndex(number), null, 0, false);
                                ap_MoveLinkedPipes(myScript, 90, 2);                          // Move Linked Pipess
                            }

                           
                        }
                    }
                    else{
                        EditorGUILayout.LabelField("", GUILayout.Width(SquareSize.intValue * .65f));
                    }

                    // Button to deactivate in the scene view the pipe
                    if(startTile.intValue != number && endTile.intValue != number && WhichSection == 1){
                        if (GUILayout.Button("", GUILayout.Width(SquareSize.intValue * .28f), GUILayout.Height(SquareSize.intValue / 3)))
                        {
                            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>(true);
                            foreach (Transform child in ts)
                            {
                                if (child != null && child.name.Contains("Cube") && child != myScript.transform && int.Parse(child.parent.name) == number)
                                {
                                    Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.name);
                                    if (child.gameObject.activeInHierarchy)
                                        child.gameObject.SetActive(false);
                                    else
                                        child.gameObject.SetActive(true);
                                }
                            }
                        } 
                    }
                    else{
                        EditorGUILayout.LabelField("", GUILayout.Width(SquareSize.intValue * .28f));
                    }
                   

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();

                    number++;

                    EditorGUILayout.EndVertical();
                }
            }
        

            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }

//--> Update scene view visualization 
    private void updateEditorSpriteVisualization(PipesPuzzle_Pc myScript,int number){
        #region

        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in ts)
        {
            if(pipeType.intValue == 0){ // Empty
                SpriteState(number, child, false, false, false, false);
            }
            if (pipeType.intValue == 1)
            { // Vertical
                SpriteState(number, child, true, true, false, false);
            }
            if (pipeType.intValue == 2)
            { // T
                SpriteState(number, child, true, true, false, true);
            }
            if (pipeType.intValue == 3)
            { // Elbow
                SpriteState(number, child, true, false, false, true);
            }
            if (pipeType.intValue == 4)
            { // No Move
                SpriteState(number, child, true, true, true, true);
            }
        }
        #endregion
    }

    private void SpriteState(int number, Transform child,bool b_Up,bool b_Down, bool b_Left, bool b_Right){
        #region
        string newName = "";
        if (number < 10)
            newName = "0" + number;
        else
            newName = number.ToString();

        if (child != null && child.name.Contains(newName + "_" + "Sprite_Up"))
        {
            Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.name);
            child.gameObject.SetActive(b_Up);
        }

        if (child != null && child.name.Contains(newName + "_" + "Sprite_Down"))
        {
            Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.name);
            child.gameObject.SetActive(b_Down);

        }
        if (child != null && child.name.Contains(newName + "_" + "Sprite_Left"))
        {
            Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.name);
            child.gameObject.SetActive(b_Left);

        }
        if (child != null && child.name.Contains(newName + "_" + "Sprite_Right"))
        {
            Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.name);
            child.gameObject.SetActive(b_Right);
        }
        #endregion
    }

    private void ActivateDeactivateBackgroundSprite(PipesPuzzle_Pc myScript)
    {
        #region
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in ts)
        {
           
            if (child != null && child.name.Contains("Sprite_BackGround"))
            {
                Undo.RegisterFullObjectHierarchyUndo(child.gameObject, child.name);
                if( int.Parse(child.parent.parent.parent.name) == startTile.intValue)
                    child.gameObject.SetActive(true);
                else if(int.Parse(child.parent.parent.parent.name) == endTile.intValue)
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false); 
            }
        }
        #endregion
    }

    private void movePipes(PipesPuzzle_Pc myScript, SerializedProperty position, SerializedProperty directionUp, float step,bool MovePipes)
    {
        #region
        if (MovePipes) position.intValue++;
            
        position.intValue = position.intValue % 4;

        float newAngle = 90 * position.intValue;

        objPIVOT.localEulerAngles = new Vector3(0, 0, -newAngle);
        #endregion
    }

//--> Generate the puzzle
    private void GenerateKeys(PipesPuzzle_Pc myScript)
    {
        #region
        currentSelectedSprite.intValue = 0;
        Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
        Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in ts)
        {
            if (child != null && child.name.Contains("Pipes") && child != myScript.transform)
            {
                Undo.DestroyObjectImmediate(child.gameObject);
            }
        }

        tilesList.ClearArray();
        PipesTypeList.ClearArray();
        PipesPositionList.ClearArray();
        PipesSolutionList.ClearArray();
        linkPipes.ClearArray();

        startTile.intValue = 0;
        endTile.intValue = _NumberOfKey.intValue - 1;

        for (var i = 0; i < _NumberOfKey.intValue; i++)
        {
            tilesList.InsertArrayElementAtIndex(0);
            PipesTypeList.InsertArrayElementAtIndex(0);
            PipesPositionList.InsertArrayElementAtIndex(0);
            PipesSolutionList.InsertArrayElementAtIndex(0);
            linkPipes.InsertArrayElementAtIndex(0);
        }

        selectDefaultTile.intValue = 0; // Default Pipe

        int number = 0;
        int raw = 0;
        for (var i = 0; i < _NumberOfKey.intValue; i++)
        {
            GameObject newTile = Instantiate(myScript.defaultTileList[selectDefaultTile.intValue], myScript.gameObject.transform);

            // Position for each Pipes
            if (raw != 0)
                newTile.transform.localPosition = new Vector3(.1f * (i % _Column.intValue), -.1f * raw, 0);
            else
                newTile.transform.localPosition = new Vector3(.1f * i, -.1f * raw, 0);


            if (number < 10)
                newTile.name = "Pipes_0" + number;
            else
                newTile.name = "Pipes_" + number;


            newTile.transform.GetChild(0).name = number.ToString();

            Undo.RegisterCreatedObjectUndo(newTile, newTile.name);

            ts = newTile.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT_Pipe"))
                {
                    child.name += "_" + number.ToString();
                    tilesList.GetArrayElementAtIndex(number).objectReferenceValue = child.gameObject;

                }

                if (child != null && child.name.Contains("Sprite_"))
                {
                    string newName = "";
                    if (number < 10)
                        newName = "0" + number;
                    else
                        newName = number.ToString();

                    child.name = newName + "_" + child.name;
                    child.gameObject.SetActive(false);
                }

            }

            if (i % _Column.intValue == _Column.intValue - 1)
                raw++;
            number++;
        }

        updatemazeStartEndTile(myScript);
        #endregion
    }

    private void ResetPosition(PipesPuzzle_Pc myScript,SerializedProperty _PositionList, SerializedProperty _DirectionList){
        #region
        for (var i = 0; i < _PositionList.arraySize;i++){
            _PositionList.GetArrayElementAtIndex(i).intValue = 0;
        }

        for (var i = 0; i < PipesPositionList.arraySize; i++)
        {
            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == i)
                {
                    objPIVOT = child;
                }
            }

            movePipes(myScript, _PositionList.GetArrayElementAtIndex(i), null, 0,false);
        }
        #endregion
    }

    private void loadPipesPosition(PipesPuzzle_Pc myScript,   SerializedProperty _DirectionList)
    {
        #region
        for (var i = 0; i < PipesPositionList.arraySize; i++)
        {
            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT") && child != myScript.transform && int.Parse(child.parent.parent.name) == i)
                {
                    objPIVOT = child;
                }
            }

            float newAngle = 0;

            if ((toolbarCurrentValue.intValue == 0) || (toolbarCurrentValue.intValue == 1) || (toolbarCurrentValue.intValue == 2))  // solution
            {
                newAngle = 90 * PipesSolutionList.GetArrayElementAtIndex(i).intValue;
            }
            else{
                if(i == startTile.intValue || i == endTile.intValue)
                    newAngle = 90 * PipesSolutionList.GetArrayElementAtIndex(i).intValue;
                else
                    newAngle = 90 * PipesPositionList.GetArrayElementAtIndex(i).intValue;
            }

            objPIVOT.localEulerAngles = new Vector3(0, 0, -newAngle);
        }
        #endregion
    }

    public void displayPipesInTheInspectorSectionLink(PipesPuzzle_Pc myScript, GUIStyle style_Blue, int WhichSection)
    {
        #region
        EditorGUILayout.LabelField("");

        int number = 0;

        int raw = Mathf.RoundToInt(_NumberOfKey.intValue / _Column.intValue);

        //Debug.Log("raw : " + raw +  " : _Column :" + _Column.intValue + " : _NumberOfKey :" + _NumberOfKey.intValue);
        for (var i = 0; i <= raw; i++)
        {
            EditorGUILayout.BeginHorizontal();

            for (var j = 0; j < _Column.intValue; j++)
            {

                tmpText = null;
                if (tilesList.arraySize > number)
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(SquareSize.intValue));

                    if(ap_checkIfPipesIsLinked(myScript, number))
                        GUI.backgroundColor = _cBlue;
                    else if (currentSelectedSprite.intValue == number)
                        GUI.backgroundColor = _cRed;
                    else
                        GUI.backgroundColor = _cGray;
                    if (GUILayout.Button("", GUILayout.Width(SquareSize.intValue), GUILayout.Height(SquareSize.intValue)))
                    {
                   
                        if (b_SelectPipessToLink.boolValue){            // Select linked Pipes
                            ap_linkPipes(myScript,number);
                        }
                        else{                                           // Select Master Pipes
                            currentSelectedSprite.intValue = number;
                        }
                    }
                    number++;

                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }

    private bool ap_checkIfPipesIsLinked(PipesPuzzle_Pc myScript, int number)
    {
        #region
        bool result = false;
        for (var i = 0; i < myScript.linkPipes[currentSelectedSprite.intValue]._PipesList.Count;i++){
            if(myScript.linkPipes[currentSelectedSprite.intValue]._PipesList[i] == number){
                result = true;
                break;
             }
        }

        return result;
        #endregion
    }

    private void ap_linkPipes(PipesPuzzle_Pc myScript,int number){
        #region
        if (currentSelectedSprite.intValue != number)
        {
            Undo.RegisterFullObjectHierarchyUndo(myScript, myScript.name);
            //Debug.Log("Add Linked Pipes : " + number);

            bool result = false;
            int listPosition = 0;
            for (var i = 0; i < myScript.linkPipes[currentSelectedSprite.intValue]._PipesList.Count; i++)
            {
                if (myScript.linkPipes[currentSelectedSprite.intValue]._PipesList[i] == number)
                {
                    result = true;
                    listPosition = i;
                    break;
                }
            }

            if (result)
                myScript.linkPipes[currentSelectedSprite.intValue]._PipesList.RemoveAt(listPosition);
            else
                myScript.linkPipes[currentSelectedSprite.intValue]._PipesList.Add(number); 
        }
        #endregion
    }

    private void ap_MoveLinkedPipes(PipesPuzzle_Pc myScript,float step,int WhichSection)
    {
        #region
        SerializedProperty PipesList = linkPipes.GetArrayElementAtIndex(currentSelectedSprite.intValue).FindPropertyRelative("_PipesList");
       
        for (var i = 0; i < PipesList.arraySize; i++){
           // Debug.Log("PipesList : " + PipesList.GetArrayElementAtIndex(i).intValue);

            Transform[] ts = myScript.gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in ts)
            {
                if (child != null && child.name.Contains("PIVOT") 
                    && child != myScript.transform 
                    && int.Parse(child.parent.parent.name) == PipesList.GetArrayElementAtIndex(i).intValue)
                {
                    objPIVOT = child;
                }
            }

            if(WhichSection == 2){      // Section : Solution 
                movePipes(myScript,
                     PipesSolutionList.GetArrayElementAtIndex(PipesList.GetArrayElementAtIndex(i).intValue),
                     null,
                     step,
                     true);
            }
            if (WhichSection == 3)      // Section :Pipes Init Position
            {
                movePipes(myScript,
                     PipesPositionList.GetArrayElementAtIndex(PipesList.GetArrayElementAtIndex(i).intValue),
                     null,
                     step,
                     true);
            }
        }
        #endregion
    }

    //--> display a list of methods call when the puzzle starts the first time
    private void displayFirstTimePuzzle(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        PipesPuzzle_Pc myScript = (PipesPuzzle_Pc)target;

        methodModule.displayMethodList("Actions when the puzzle starts the first time:",
                                       editorMethods,
                                       methodsList,
                                       myScript.methodsList,
                                       style_Blue,
                                       style_Yellow_01,
                                       "Read docmentation for more info the methods allowed.");

        #endregion
    }
    private void updatePuzlleDetectorPosition(int dragAndDropMode)
    {
        #region
        PipesPuzzle_Pc myScript = (PipesPuzzle_Pc)target;

        if (dragAndDropMode == 0)      // Focus
        {
            if (GUILayout.Button("Update Puzzle Detector Position (Focus)"))
            {
                if (FindGameObject("refFocus"))
                {
                    SerializedObject serializedObject3 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.transform);
                    SerializedProperty m_LocalScale = serializedObject3.FindProperty("m_LocalScale");
                    SerializedProperty m_LocalPosition = serializedObject3.FindProperty("m_LocalPosition");

                    serializedObject3.Update();

                    m_LocalScale.vector3Value = FindGameObject("refFocus").transform.localScale;
                    m_LocalPosition.vector3Value = FindGameObject("refFocus").transform.localPosition;

                    serializedObject3.ApplyModifiedProperties();

                    SerializedObject serializedObject4 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.GetComponent<MeshRenderer>());
                    SerializedProperty m_Enabled = serializedObject4.FindProperty("m_Enabled");
                    serializedObject4.Update();
                    m_Enabled.boolValue = false;
                    serializedObject4.ApplyModifiedProperties();
                }
            }
        }
        else                                    // Other Modes
        {
            if (GUILayout.Button("Update Puzzle Detector Position (No Focus)"))
            {
                if (FindGameObject("refNoFocus"))
                {
                    SerializedObject serializedObject3 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.transform);
                    SerializedProperty m_LocalScale = serializedObject3.FindProperty("m_LocalScale");
                    SerializedProperty m_LocalPosition = serializedObject3.FindProperty("m_LocalPosition");


                    serializedObject3.Update();

                    m_LocalScale.vector3Value = FindGameObject("refNoFocus").transform.localScale;
                    m_LocalPosition.vector3Value = FindGameObject("refNoFocus").transform.localPosition;

                    serializedObject3.ApplyModifiedProperties();


                    SerializedObject serializedObject4 = new UnityEditor.SerializedObject(myScript.aP_PuzzleDetector.GetComponent<MeshRenderer>());
                    SerializedProperty m_Enabled = serializedObject4.FindProperty("m_Enabled");
                    serializedObject4.Update();
                    m_Enabled.boolValue = true;
                    serializedObject4.ApplyModifiedProperties();
                }
            }
        }
        #endregion
    }

    private GameObject FindGameObject(string sName)
    {
        #region
        PipesPuzzle_Pc myScript = (PipesPuzzle_Pc)target;
        Transform[] children = myScript.gameObject.transform.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == sName)
                return child.gameObject;
        }

        return null;
        #endregion
    }

    void OnSceneGUI()
    {
    }

    public void _helpBox(int value)
    {
        #region
        string sType = "";
       // if (puzzleType.intValue == 1)
            sType = "Pipe";
        //else
          //  sType = "Cercle";

        if (helpBoxEditor.boolValue)
        {
            switch (value)
            {
                case 0:
                    EditorGUILayout.HelpBox("1-Choose the number of " + sType +"s." +
                                            "\n2- Choose the number of column." +
                                            "\n3-Press button 'Generate " + sType + "' to create the puzzle.", MessageType.Info);
                    break;
                case 1:
                    EditorGUILayout.HelpBox("1-Click on a Button below to access his parameters." +
                                            "\n2-Drag and drop a sprite in the slot next to the KEY thumbnail." +
                                            "\n3-Change his scale." +
                                            "\n4-Apply the same scale to all tiles by pressing button ''Apply to All''." + 
                                            "\n5-Choose the text displayed in the scene view inside the KEY." +
                                            "\n6-Choose the value displayed on the result screen in the scene view.", MessageType.Info);
                    break;
                case 2:
                    EditorGUILayout.HelpBox("1-Press button 'Activate Mode' next to Choose Parent " + sType + ". Then choose the " + sType + " you want to link." +
                                            "\n(Press one of the square button to select a " + sType + ")" +
                                            "\n\n2-Press button 'Activate Mode' next to Choose Linked " + sType + ". Then choose the " + sType + " you want to link to the parent." +
                                            "\n (Press one of the square button to select a " + sType + ")." , MessageType.Info);
                    break;
            }
        }
        #endregion
    }

   /* void OnInspectorUpdate()
    {
        Repaint();
    }
*/

}
#endif