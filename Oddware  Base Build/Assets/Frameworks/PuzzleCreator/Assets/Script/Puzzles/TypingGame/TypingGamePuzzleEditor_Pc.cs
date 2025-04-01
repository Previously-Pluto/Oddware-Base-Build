//Description : TypingGamePuzzle_Pc : Manage the typing game behaviour
#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using TypingGameKit;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TypingGamePuzzle_Pc))]
public class TypingGamePuzzleEditor_Pc : Editor
{

    SerializedProperty SeeInspector;
    SerializedProperty helpBoxEditor;
    SerializedProperty toolbarCurrentValue;
    SerializedProperty tilesList;

    SerializedProperty initialSequenceCount;
    SerializedProperty anchorObject;
    SerializedProperty texts;
    SerializedProperty positionRange;
    SerializedProperty inputFailedClip;
    SerializedProperty inputSucceededClip;
    SerializedProperty demoManager;
    SerializedProperty rotationSpeed;
    SerializedProperty locks;


    SerializedProperty distanceScaling;
    SerializedProperty overlapAvoidance;
    SerializedProperty restrictToFrame;
    SerializedProperty softMovement;

    SerializedProperty a_KeyPressed;
    SerializedProperty a_KeyPressedVolume;
    SerializedProperty a_Reset;
    SerializedProperty a_ResetVolume;
    SerializedProperty a_WrongCode;
    SerializedProperty a_WrongCodeVolume;



    public string[] toolbarStrings = new string[] { "Puzzle Creation", "Game Options", "Naninovel" };

    SerializedProperty cameraUseForFocus;
    SerializedProperty methodsList;
    public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
    public AP_MethodModule_Pc methodModule;

    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab", "Reticule Mode" };
    //public AP_PuzzleDetector objPuzzleDetetor;
    SerializedProperty aP_PuzzleDetector;

    //Naninovel components
    SerializedProperty scriptName;

    SerializedProperty label;


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

    public Color _cBlue = new Color(0f, .9f, 1f, .5f);
    public Color _cRed = new Color(1f, .5f, 0f, .5f);
    public Color _cGray = new Color(.9f, .9f, .9f, 1);
    public Color _cGreen = Color.green;

    void OnEnable()
    {
        #region
        // Setup the SerializedProperties.
        SeeInspector = serializedObject.FindProperty("SeeInspector");
        helpBoxEditor = serializedObject.FindProperty("helpBoxEditor");
        toolbarCurrentValue = serializedObject.FindProperty("toolbarCurrentValue");
        TypingGamePuzzle_Pc myScript = (TypingGamePuzzle_Pc)target;


        initialSequenceCount = serializedObject.FindProperty("initualSequenceCount");
        anchorObject = serializedObject.FindProperty("anchorObject");
        texts = serializedObject.FindProperty("texts");
        positionRange = serializedObject.FindProperty("positionRange");
        inputFailedClip = serializedObject.FindProperty("inputFailedClip");
        inputSucceededClip = serializedObject.FindProperty("inputSucceededClip");

        distanceScaling = serializedObject.FindProperty("distanceScaling");
        overlapAvoidance = serializedObject.FindProperty("overlapAvoidance");
        restrictToFrame = serializedObject.FindProperty("restrictToFrame");
        softMovement = serializedObject.FindProperty("softMovement");
        demoManager = serializedObject.FindProperty("demoManager");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        locks = serializedObject.FindProperty("locks");

        tilesList = serializedObject.FindProperty("tilesList");
        cameraUseForFocus = serializedObject.FindProperty("cameraUseForFocus");


        methodsList = serializedObject.FindProperty("methodsList");
        editorMethods = new EditorMethods_Pc();
        methodModule = new AP_MethodModule_Pc();

        aP_PuzzleDetector = serializedObject.FindProperty("aP_PuzzleDetector");

        a_KeyPressed = serializedObject.FindProperty("a_KeyPressed");
        a_KeyPressedVolume = serializedObject.FindProperty("a_KeyPressedVolume");
        a_Reset = serializedObject.FindProperty("a_Reset");
        a_ResetVolume = serializedObject.FindProperty("a_ResetVolume");
        a_WrongCode = serializedObject.FindProperty("a_WrongCode");
        a_WrongCodeVolume = serializedObject.FindProperty("a_WrongCodeVolume");


        scriptName = serializedObject.FindProperty("scriptName"); ;

        label = serializedObject.FindProperty("label");

        Tex_01 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_02 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_03 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_04 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        Tex_05 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        #endregion
    }

    private GameObject FindGameObject(string sName)
    {
        #region
        TypingGamePuzzle_Pc myScript = (TypingGamePuzzle_Pc)target;
        Transform[] children = myScript.gameObject.transform.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == sName)
                return child.gameObject;
        }

        return null;
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

        TypingGamePuzzle_Pc myScript = (TypingGamePuzzle_Pc)target;


        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Puzzle could not be edited in play mode", MessageType.Info);
        }
        else
        {
            // --> Display Tab sections in the Inspector

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

            // --> Display GeneratePuzzleSection
            if (toolbarCurrentValue.intValue == 0)
                DisplayPuzzleCreationSection(myScript, style_Orange);

            // --> Display Other Section
            if (toolbarCurrentValue.intValue == 1)
                otherSection(myScript, style_Orange);

            if (toolbarCurrentValue.intValue == 2)
                DisplayNaniNovelSection(myScript, style_Blue);


            if (tilesList.arraySize > 0)
            {

                if (b_TilesExist)
                {
                    // --> Display Select Sprites
                    if (toolbarCurrentValue.intValue == 0)
                        DisplayPuzzleCreationSection(myScript, style_Blue);
                    // --> Display Mix Section
                    if (toolbarCurrentValue.intValue == 1)
                        otherSection(myScript, style_Yellow_01);
                }
            }
        }


        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.LabelField("");
        #endregion
    }

    private void DisplayPuzzleCreationSection(TypingGamePuzzle_Pc myScript, GUIStyle style_blue)
    {
        #region
        EditorGUILayout.BeginVertical(style_blue);
        EditorGUILayout.HelpBox("Section : Generate Keys. (Minimum : 1)", MessageType.Info);
        _helpBox(0);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Demo manager :", GUILayout.Width(150));
        EditorGUILayout.PropertyField(demoManager, new GUIContent(""), GUILayout.Width(350));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Locks (timer must be in seconds) :", GUILayout.Width(220));
        EditorGUILayout.PropertyField(locks, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Rotation speed :", GUILayout.Width(130));
        EditorGUILayout.PropertyField(rotationSpeed, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Anchor object :", GUILayout.Width(100));
        EditorGUILayout.PropertyField(anchorObject, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Texts :", GUILayout.Width(100));
        EditorGUILayout.PropertyField(texts, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Position range :", GUILayout.Width(100));
        EditorGUILayout.PropertyField(positionRange, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Distance scaling :", GUILayout.Width(120));
        EditorGUILayout.PropertyField(distanceScaling, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Overlap avoidance :", GUILayout.Width(120));
        EditorGUILayout.PropertyField(overlapAvoidance, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Restrict to frame :", GUILayout.Width(120));
        EditorGUILayout.PropertyField(restrictToFrame, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Soft movement :", GUILayout.Width(120));
        EditorGUILayout.PropertyField(softMovement, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Succeeded clip :", GUILayout.Width(120));
        EditorGUILayout.PropertyField(inputSucceededClip, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Failed clip :", GUILayout.Width(120));
        EditorGUILayout.PropertyField(inputFailedClip, new GUIContent(""), GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Generate"))
        {
            GenerateKeys(myScript);
        }
        EditorGUILayout.EndVertical();
        #endregion
    }

    private void DisplayNaniNovelSection(TypingGamePuzzle_Pc myScript, GUIStyle style_blue)
    {
        #region
        EditorGUILayout.BeginVertical(style_blue);
        _helpBox(3);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Script name :", GUILayout.Width(150));
        EditorGUILayout.PropertyField(scriptName, new GUIContent(""), GUILayout.Width(350));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Label :", GUILayout.Width(150));
        EditorGUILayout.PropertyField(label, new GUIContent(""), GUILayout.Width(350));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        #endregion
    }


    private void GenerateKeys(TypingGamePuzzle_Pc myScript)
    {
        #region
        myScript.UpdateGameOptions();
        #endregion
    }

    public void _helpBox(int value)
    {
        #region
        if (helpBoxEditor.boolValue)
        {
            switch (value)
            {
                case 0:
                    EditorGUILayout.HelpBox("1-Choose the number of Column." +
                                            "\n2-Choose the number of keys." +
                                            "\n3-Press button 'Generate' to create the puzzle.", MessageType.Info);
                    break;
                case 1:
                    EditorGUILayout.HelpBox("1-Click on a Button below to access its parameters." +
                                            "\n2-Drag and drop a sprite in the slot next to the KEY thumbnail." +
                                            "\n3-Change its scale." +
                                            "\n4-Apply the same scale to all tiles by pressing button ''Apply to All''." +
                                            "\n5-Choose the text displayed in the scene view inside the KEY." +
                                            "\n6-Choose the value displayed on the result screen in the scene view.", MessageType.Info);
                    break;
                case 2:
                    EditorGUILayout.HelpBox("1-Create the code by pressing the buttons below." +
                                            "\n" +
                                            "\nNote : Reset the solution by pressing button 'Reset Solution'.", MessageType.Info);
                    break;
                case 3:
                    EditorGUILayout.HelpBox("1- Insert script name in order to indicate to Naninovel which script to play." +
                                            "\n2- Add label .", MessageType.Info);
                    break;

            }
        }
        #endregion
    }



    private void otherSection(TypingGamePuzzle_Pc myScript, GUIStyle style_Orange)
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
            //cameraUseForFocus.
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Puzzle Detetcor : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(aP_PuzzleDetector, new GUIContent(""));
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Play Audio when Key is pressed : ", GUILayout.Width(180));
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

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Play Audio when code is wrong : ", GUILayout.Width(180));
        EditorGUILayout.PropertyField(a_WrongCode, new GUIContent(""), GUILayout.Width(100));
        GUILayout.Label("Volume : ", GUILayout.Width(60));
        a_WrongCodeVolume.floatValue = EditorGUILayout.Slider(a_WrongCodeVolume.floatValue, 0, 1);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("");
        PuzzleDetection(style_Orange, style_Orange);
        GUILayout.Label("");
        displayFirstTimePuzzle(style_Orange, style_Orange);

        EditorGUILayout.EndVertical();
        #endregion
    }

    //--> display a list of methods call when the puzzle starts the first time
    private void displayFirstTimePuzzle(GUIStyle style_Yellow_01, GUIStyle style_Blue)
    {
        #region
        //--> Display feedback
        TypingGamePuzzle_Pc myScript = (TypingGamePuzzle_Pc)target;

        methodModule.displayMethodList("Actions when the puzzle starts the first time:",
                                       editorMethods,
                                       methodsList,
                                       myScript.methodsList,
                                       style_Blue,
                                       style_Yellow_01,
                                       "Read docmentation for more info the methods allowed.");
        //started the methods

        #endregion
    }


    private void PuzzleDetection(GUIStyle _color_01, GUIStyle _color_02)
    {
        #region
        EditorGUILayout.BeginVertical(_color_01);

        TypingGamePuzzle_Pc myScript = (TypingGamePuzzle_Pc)target;

        SerializedObject serializedObject3 = new UnityEditor.SerializedObject(myScript.gameObject.GetComponent<AP_PuzzleMoveType_Pc>());

        SerializedProperty dragAndDropMode = serializedObject3.FindProperty("puzzleMoveMode");






        serializedObject3.Update();

        GUILayout.Label("Puzzle Detection Options:", EditorStyles.boldLabel);
        /* if (helpBoxEditor.boolValue)
         {
             EditorGUILayout.HelpBox("Desktop: (0) Focus Mode and (3) Free Mode: Reticule" +
                                     "\nVR: Free Mode: (1) VR Raycast and Free Mode: (2) VR Hand", MessageType.Info);

             EditorGUILayout.HelpBox("VR: Free Mode: (1) VR Raycast and Free Mode: (2) VR Hand doesn't work on Mobile", MessageType.Warning);
         }*/


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Mode: ", GUILayout.Width(170));
        dragAndDropMode.intValue = EditorGUILayout.Popup(dragAndDropMode.intValue, listDragAndDropMode);
        EditorGUILayout.EndHorizontal();

        if (dragAndDropMode.intValue == 3)
        {
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

        //GUILayout.Label("");
        updatePuzlleDetectorPosition(dragAndDropMode.intValue);

        serializedObject3.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();
        #endregion
    }

    private void updatePuzlleDetectorPosition(int dragAndDropMode)
    {
        #region
        TypingGamePuzzle_Pc myScript = (TypingGamePuzzle_Pc)target;

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
}
#endif