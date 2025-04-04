﻿// Description : SaveData_Pc : Use for the save system : THis script to all the objects that need to be saved in a scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData_Pc : MonoBehaviour {
	public 	bool 								SeeInspector = false;
	public 	bool 								moreOptions = false;
	public List<EditorMethodsList_Pc.MethodsList> 	methodsList = new List<EditorMethodsList_Pc.MethodsList>(1);
	private CallMethods_Pc callMethods = new CallMethods_Pc();

	public bool b_isObjectActivated = false;		// Modify by w_Inventory to init the selected Monobehaviour isObjectActivated.cs
	public int isObjectActivatedIndex = 0;			// Modify by w_Inventory to init the selected Monobehaviour isObjectActivated.cs

    // Return a string with the values to save
	public string R_SaveData () {
		return callMethods.Call_A_Method_Only_String_SaveData (methodsList,"");
	}

    // Load the values for this object (string)
	public void LoadData (string s_ObjectDatas) {
        if(gameObject.activeInHierarchy)callMethods.Call_A_Method_ObjectLoadData (methodsList,s_ObjectDatas);
	}

    // Call by the button 'btn_ResetPuzzle' to reset the current puzzle
    public void ResetPuzzle(string s_ObjectDatas)
    {
        callMethods.Call_A_Method_F_ResetPuzzle(methodsList, s_ObjectDatas);
    }

  
}
