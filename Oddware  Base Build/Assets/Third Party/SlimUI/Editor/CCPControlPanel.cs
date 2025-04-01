using UnityEditor;
using UnityEngine;

namespace SlimUI.CursorControllerPro
{
    public class CCPControlPanel : EditorWindow
    {
        private static CCPControlPanel s_instance;
        private GUISkin guiSkin;
        private GUIStyle redButton, setupButton, moreButton;
        private bool inSetupWindow = false;
        private bool showWarning = false;

        private string cursorRendererName = "CursorCamera";
        private string[] renderModeOptions = { "Screen Space - Camera", "World Space" };
        public int renderIndex = 0;

        [MenuItem("Window/SlimUI/Cursor Controller Pro/Control Panel", false, 0)]
        public static void Open()
        {
            GetWindow<CCPControlPanel>(true, "CCP Control Panel");
            Instance.InitWindow();
        }

        private void OnEnable()
        {
            InitWindow();
            EditorUtility.ClearProgressBar();
        }

        private void InitWindow()
        {
            titleContent = new GUIContent("CCP Control Panel");
            minSize = new Vector2(336, 429);
            maxSize = minSize;

            InitStyles();
        }

        private void InitStyles()
        {
            guiSkin = EditorGUIUtility.Load("Assets/Resources/SlimUI/ControlPanelSkin.guiskin") as GUISkin;

            if (guiSkin == null)
            {
                Debug.LogError("GUISkin not found. Make sure it's in the correct location.");
                return;
            }

            redButton = guiSkin.GetStyle("RedButton");
            moreButton = guiSkin.GetStyle("MoreButton");
            setupButton = guiSkin.GetStyle("SetupButton");
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            // Use the "WindowBackground" style to render the background
            GUIStyle windowBackgroundStyle = guiSkin.GetStyle("WindowBackground");
            if (windowBackgroundStyle != null)
            {
                // Calculate the background dimensions while maintaining the aspect ratio
                float bgWidth = position.width;
                float bgHeight = position.height;

                // Calculate the position to center the background
                float xOffset = (position.width - bgWidth) / 2;
                float yOffset = (position.height - bgHeight) / 2;

                // Set the background color (change Color.white to your desired color)
                GUI.backgroundColor = Color.white;

                // Draw the background using the calculated size and position
                GUI.Box(new Rect(xOffset, yOffset, bgWidth, bgHeight), GUIContent.none, windowBackgroundStyle);

                // Reset the background color to its original state
                GUI.backgroundColor = Color.white;
            }
            else
            {
                // Fallback if "WindowBackground" style is not found
                GUI.Label(new Rect(0, 0, position.width, position.height), GUIContent.none);
            }

            GUILayout.BeginArea(new Rect(1, 172, position.width, position.height - 172));
            DrawContent();
            GUILayout.EndArea();
        }



        private void DrawContent()
        {
            if (!inSetupWindow)
            {
                GUILayout.BeginVertical();
                DrawSetup();
                DrawDocumentation();
                DrawSupport();
                DrawWatchVideos();
                DrawMoreSlimUI();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginVertical();
                if (GUILayout.Button("MENU", moreButton))
                {
                    inSetupWindow = false;
                    showWarning = false;
                }
                GUILayout.Label("Camera Render Mode", guiSkin.label);
                renderIndex = EditorGUILayout.Popup(renderIndex, renderModeOptions, "Dropdown");
                GUILayout.Space(5);
                GUILayout.Label("Name of existing Camera object to use as Renderer. (1) Leaving the name empty will generate a camera. (2) The name cannot contain spaces.", guiSkin.label);
                GUILayout.Space(2);
                cursorRendererName = EditorGUILayout.TextField(cursorRendererName, "TextField");
                GUILayout.Space(11);
                if (GUILayout.Button("GENERATE CONTROLLER", redButton))
                {
                    Object controlPrefab = AssetDatabase.LoadAssetAtPath("Assets/Resources/SlimUI/Prefabs/CursorControl.prefab", typeof(GameObject));
                    if (controlPrefab != null && FindObjectsOfType<CursorController>().Length < 1){
                        GameObject spawnedPrefab = PrefabUtility.InstantiatePrefab(controlPrefab) as GameObject;
                        spawnedPrefab.GetComponent<CursorController>();

                        if(renderIndex==0){
                            spawnedPrefab.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                            if(GameObject.Find(cursorRendererName) == null){
                                GameObject spawnedCamera = new GameObject("Cursor Camera");
                                spawnedCamera.AddComponent<Camera>();
                                //spawnedCamera.tag = cursorRendererName;
                                spawnedPrefab.GetComponent<CursorController>().cameraMain = spawnedCamera.GetComponent<Camera>();
                                spawnedPrefab.GetComponent<Canvas>().worldCamera = spawnedCamera.GetComponent<Camera>();
                                spawnedCamera.GetComponent<Camera>().orthographic = true;
                                DestroyImmediate(spawnedPrefab.GetComponent<AudioListener>());
                            }else{
                                spawnedPrefab.GetComponent<Canvas>().worldCamera = GameObject.Find(cursorRendererName).GetComponent<Camera>();
                                spawnedPrefab.GetComponent<CursorController>().cameraMain = GameObject.Find(cursorRendererName).GetComponent<Camera>();
                                spawnedPrefab.GetComponent<CursorController>().cameraMain.GetComponent<Camera>().GetComponent<Camera>().orthographic = true;
                                DestroyImmediate(spawnedPrefab.GetComponent<AudioListener>());
                                //spawnedPrefab.GetComponent<CursorController>().cameraMain.GetComponent<Camera>().name = "Cursor Camera";
                            }
                        }else if(renderIndex==1){
                            spawnedPrefab.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                            if(GameObject.Find(cursorRendererName) == null){
                                GameObject spawnedCamera = new GameObject("Cursor Camera");
                                spawnedCamera.AddComponent<Camera>();
                                //spawnedCamera.tag = cursorRendererName;
                                spawnedPrefab.GetComponent<CursorController>().cameraMain = spawnedCamera.GetComponent<Camera>();
                                spawnedPrefab.GetComponent<Canvas>().worldCamera = spawnedCamera.GetComponent<Camera>();
                                spawnedCamera.GetComponent<Camera>().orthographic = true;
                                DestroyImmediate(spawnedCamera.GetComponent<AudioListener>());
                            }else{
                                spawnedPrefab.GetComponent<Canvas>().worldCamera = GameObject.Find(cursorRendererName).GetComponent<Camera>();
                                spawnedPrefab.GetComponent<CursorController>().cameraMain = GameObject.Find(cursorRendererName).GetComponent<Camera>();
                                spawnedPrefab.GetComponent<CursorController>().cameraMain.GetComponent<Camera>().orthographic = true;
                                DestroyImmediate(spawnedPrefab.GetComponent<AudioListener>());
                                //spawnedPrefab.GetComponent<CursorController>().cameraMain.name = "Cursor Camera";
                            }
                        }

                        showWarning = false;
                        Debug.Log("Scene Successfully Configured!");
                        }else if (controlPrefab == null){
                            showWarning = true;
                            Debug.LogError("Could not find CursorControl.prefab. Make sure it's in the directory: Assets/Resources/SlimUI/Prefabs/CursorControl.prefab");
                        }else if(FindObjectsOfType<CursorController>().Length >= 1){
                            showWarning = true;
                            Debug.LogWarning("There is already a CursorControl prefab in your scene. You can only have 1.");
                        }
                    
                }
                GUILayout.Space(20);
                if (showWarning)
                {
                    GUIStyle warning = new GUIStyle("Label");
                    warning.fontSize = 12;
                    warning.wordWrap = true;
                    warning.normal.textColor = Color.red;
                    GUILayout.Label("                                 !! Check Console !!", "Warning");
                }
                GUILayout.EndVertical();
            }
        }

        private void DrawSetup()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("SET UP", setupButton)) inSetupWindow = true;
            GUILayout.EndVertical();
        }

        private void DrawDocumentation()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            if (GUILayout.Button("DOCUMENTATION", redButton)) Application.OpenURL("http://cursorcontrollerpro.slimui.com/documentation/");
            GUILayout.EndVertical();
        }

        private void DrawSupport()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            if (GUILayout.Button("GET SUPPORT", redButton)) Application.OpenURL("http://cursorcontrollerpro.slimui.com/support/");
            GUILayout.EndVertical();
        }

        private void DrawWatchVideos()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            if (GUILayout.Button("WATCH VIDEOS", redButton)) Application.OpenURL("https://www.youtube.com/channel/UCvUS-7AVIBYOgvQZ7ltxfVg/");
            GUILayout.EndVertical();
        }

        private void DrawMoreSlimUI()
        {
            GUILayout.BeginVertical(GUILayout.Height(45));
            GUILayout.Space(5);
            if (GUILayout.Button("MORE CONTENT", moreButton)) Application.OpenURL("https://assetstore.unity.com/publishers/35968");
            GUILayout.EndVertical();
        }

        public static CCPControlPanel Instance
        {
            get
            {
                if (s_instance != null) return s_instance;
                s_instance = (CCPControlPanel)GetWindow(typeof(CCPControlPanel), true, "CCP Control Panel");
                return s_instance;
            }
        }
    }
}
