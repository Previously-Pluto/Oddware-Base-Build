// www.SlimUI.com
// Copyright (c) 2018 - 2020 SlimUI. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

// JOIN THE DISCORD FOR SUPPORT OR CONVERSATION: https://discord.gg/7cK4KBf

using UnityEngine;

namespace SlimUI.CursorControllerPro{
	public class AutomaticAnimating : MonoBehaviour {
		bool calledYet = false;

		void Start()
		{
			InvokeRepeating("Animating", 1.0f, 1f);
		}

		void Animating(){
			if(GetComponent<Animator>().GetBool("Fade") == false && calledYet == false){
				calledYet = true;
				GetComponent<Animator>().SetBool("Fade",true);
			}else if(GetComponent<Animator>().GetBool("Fade") == true && calledYet == false){
				calledYet = true;
				GetComponent<Animator>().SetBool("Fade",false);
			}
			calledYet = false;
		}
	}
}