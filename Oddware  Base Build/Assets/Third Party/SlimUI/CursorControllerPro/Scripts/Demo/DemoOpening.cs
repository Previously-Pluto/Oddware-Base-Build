// www.SlimUI.com
// Copyright (c) 2018 - 2020 SlimUI. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

// JOIN THE DISCORD FOR SUPPORT OR CONVERSATION: https://discord.gg/7cK4KBf

using System.Collections;
using UnityEngine;

namespace SlimUI.CursorControllerPro{
	public class DemoOpening : MonoBehaviour {
		public GameObject popUpWelcome;
		public Animator inputArrow;
		public float waitTime;

		// JUST FOR DEMO
		void Start () {
			popUpWelcome.SetActive(false);
			StartCoroutine(LoadPopUpWelcome());
		}
		
		IEnumerator LoadPopUpWelcome(){
			yield return new WaitForSeconds(waitTime);
			popUpWelcome.SetActive(true);
		}

		// Changes direction of Input Arrow
		public void ChangeArrowDirection(int x){
			inputArrow.SetInteger("direction", x);
		}
	}
}
