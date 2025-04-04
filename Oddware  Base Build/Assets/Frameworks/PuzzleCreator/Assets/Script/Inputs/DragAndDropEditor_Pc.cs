﻿//Description : DragAndDropEditor_Pc : DragAndDrop_Pc custom editor
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(AP_.DragAndDrop_Pc))]
public class DragAndDropEditor_Pc : Editor {
	SerializedProperty			SeeInspector;											// use to draw default Inspector
    SerializedProperty          a_TakeObject;
    SerializedProperty          distanceFromTheCamera;
    SerializedProperty          dragAndDropMode;


    public string[] listDragAndDropMode = new string[4] { "Focus Mode", "VR Raycast", "VR Grab","Reticule Mode" };


    public GameObject objCanvasInput;

	private Texture2D MakeTex(int width, int height, Color col) {						// use to change the GUIStyle
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	private Texture2D 		Tex_01;
	private Texture2D 		Tex_02;
	private Texture2D 		Tex_03;
	private Texture2D 		Tex_04;
	private Texture2D 		Tex_05;

	void OnEnable () {
		// Setup the SerializedProperties.
		SeeInspector 		        = serializedObject.FindProperty ("SeeInspector");

        a_TakeObject                = serializedObject.FindProperty("a_TakeObject");
        distanceFromTheCamera       = serializedObject.FindProperty("distanceFromTheCamera");
        dragAndDropMode             = serializedObject.FindProperty("dragAndDropMode");

        AP_.DragAndDrop_Pc myScript = (AP_.DragAndDrop_Pc)target; 

	

		Tex_01 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
		Tex_04 = MakeTex(2, 2, new Color(1,.3f,1,.3f)); 
		Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f)); 
	}


	public override void OnInspectorGUI()
	{
		if(SeeInspector.boolValue)							// If true Default Inspector is drawn on screen
			DrawDefaultInspector();

		serializedObject.Update ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("See Inspector :", GUILayout.Width (85));
		EditorGUILayout.PropertyField(SeeInspector, new GUIContent (""), GUILayout.Width (30));
		EditorGUILayout.EndHorizontal ();

		GUIStyle style_Yellow_01 		= new GUIStyle(GUI.skin.box);	style_Yellow_01.normal.background 		= Tex_01; 
		GUIStyle style_Blue 			= new GUIStyle(GUI.skin.box);	style_Blue.normal.background 			= Tex_03;
		GUIStyle style_Purple 			= new GUIStyle(GUI.skin.box);	style_Purple.normal.background 			= Tex_04;
		GUIStyle style_Orange 			= new GUIStyle(GUI.skin.box);	style_Orange.normal.background 			= Tex_05; 
		GUIStyle style_Yellow_Strong 	= new GUIStyle(GUI.skin.box);	style_Yellow_Strong.normal.background 	= Tex_02;

        AP_.DragAndDrop_Pc myScript = (AP_.DragAndDrop_Pc)target; 

        EditorGUILayout.BeginVertical(style_Orange);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Drag and Drop Mode: ", GUILayout.Width(170));
        dragAndDropMode.intValue = EditorGUILayout.Popup(dragAndDropMode.intValue, listDragAndDropMode);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        if(dragAndDropMode.intValue == 0){
            EditorGUILayout.BeginVertical(style_Orange);
            EditorGUILayout.HelpBox("Select local Z distance between the selected object and the camera.", MessageType.Info);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Distance from the camera: ", GUILayout.Width(170));
            EditorGUILayout.PropertyField(distanceFromTheCamera, new GUIContent(""));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();


            EditorGUILayout.HelpBox("Play a sound when an Object is selected or deselected.", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("AudioSource : ", GUILayout.Width(170));
            EditorGUILayout.PropertyField(a_TakeObject, new GUIContent(""));
            EditorGUILayout.EndHorizontal(); 
            EditorGUILayout.LabelField("");
        }

        EditorGUILayout.LabelField("");

		serializedObject.ApplyModifiedProperties ();
	}

	

	void OnSceneGUI( )
	{
	}
}
#endif