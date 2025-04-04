﻿//Description : SaveDataEditor_Pc : SaveData_Pc custom editor
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(SaveData_Pc))]
public class SaveDataEditor_Pc : Editor {
	SerializedProperty			SeeInspector;											// use to draw default Inspector
	SerializedProperty 			methodsList;
	SerializedProperty 			moreOptions;
	private bool 				b_addMethods = false;									// use you press button +

	public EditorMethods_Pc editorMethods;
	public CallMethods_Pc 			callMethods;



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
		SeeInspector 		= serializedObject.FindProperty ("SeeInspector");

		methodsList			= serializedObject.FindProperty ("methodsList");
		moreOptions			= serializedObject.FindProperty ("moreOptions");
		editorMethods 		= new EditorMethods_Pc();
		callMethods 		= new CallMethods_Pc();

		Tex_01 = MakeTex(2, 2, new Color(.9f,.9f,0.9F,1f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
		Tex_04 = MakeTex(2, 2, new Color(1,.3f,1,.3f)); 
		Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f)); 

		SaveData_Pc myScript = (SaveData_Pc)target; 

		if(methodsList.arraySize  ==0){
			serializedObject.Update ();
			editorMethods.AddMethodsToList (methodsList);


			methodsList.GetArrayElementAtIndex(0).FindPropertyRelative("obj").objectReferenceValue = myScript.gameObject;
			methodsList.GetArrayElementAtIndex(0).FindPropertyRelative("methodInfoName").stringValue = "ReturnSaveData";

			serializedObject.ApplyModifiedProperties ();

			myScript.methodsList[0].indexScript = 
				myScript.methodsList[0].obj.GetComponents<MonoBehaviour> ().Length;
		}

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


		SaveData_Pc myScript = (SaveData_Pc)target; 

		// --> Display methods in the Inspector
		editorMethods.DisplayMethodsOnEditorSaveSection (myScript.methodsList,methodsList,style_Yellow_01);

		if (b_addMethods) {
			editorMethods.AddMethodsToList (methodsList);
			b_addMethods = false;
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField ("Show more Options :", GUILayout.Width (120));
		EditorGUILayout.PropertyField(moreOptions, new GUIContent (""), GUILayout.Width (30));
		EditorGUILayout.EndHorizontal ();


		if (moreOptions.boolValue) {
			EditorGUILayout.BeginVertical (style_Orange);
			EditorGUILayout.HelpBox ("The next button allow to check in the Console Tab the string saved with this script", MessageType.Info); 
		
			if (GUILayout.Button ("Check Save Value")) {
				Debug.Log (callMethods.Call_A_Method_Only_String_SaveData (myScript.methodsList, "Info : SaveData.cs does not return any value. Check if the selected script has a method named ReturnSaveData"));
			}

			EditorGUILayout.EndVertical ();
			EditorGUILayout.LabelField ("");
		}

		EditorGUILayout.LabelField ("");

		//-> If needed : Auto select isObjectActivated.cs when script is created.
		if (myScript.b_isObjectActivated ) {
			if (myScript.methodsList [0].indexScript != editorMethods.find_iSObjectActivatedScript (myScript.methodsList,methodsList,style_Yellow_01))
				myScript.methodsList [0].indexScript = editorMethods.find_iSObjectActivatedScript (myScript.methodsList,methodsList,style_Yellow_01);
		}

		serializedObject.ApplyModifiedProperties ();
	}

	void OnSceneGUI( )
	{
	}
}
#endif