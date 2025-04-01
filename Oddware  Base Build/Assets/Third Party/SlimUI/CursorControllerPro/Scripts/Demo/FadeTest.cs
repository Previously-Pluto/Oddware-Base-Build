// www.SlimUI.com
// Copyright (c) 2018 - 2020 SlimUI. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

// JOIN THE DISCORD FOR SUPPORT OR CONVERSATION: https://discord.gg/7cK4KBf

using System.Collections;
using UnityEngine;

namespace SlimUI.CursorControllerPro{
	public class FadeTest : MonoBehaviour {
		bool canFade;

		void Start(){
			canFade = true;
		}
		
		void Update () {
			if(Input.GetKeyDown("r") && canFade){
				StartCoroutine(FadeQuick());
			}
		}
		
		IEnumerator FadeQuick(){
			canFade = false;
			GetComponent<Animator>().SetBool("Fade",true);
			yield return new WaitForSeconds(1.5f);
			GetComponent<Animator>().SetBool("Fade",false);
			yield return new WaitForSeconds(0.15f);
			canFade = true;
		}
	}
}
