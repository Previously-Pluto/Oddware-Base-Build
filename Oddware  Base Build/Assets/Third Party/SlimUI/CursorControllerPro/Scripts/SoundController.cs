// www.SlimUI.com
// Copyright (c) 2018 - 2020 SlimUI. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

// JOIN THE DISCORD FOR SUPPORT OR CONVERSATION: https://discord.gg/7cK4KBf

using UnityEngine;
using UnityEngine.Audio;

namespace SlimUI.CursorControllerPro{
    public class SoundController : MonoBehaviour{
        [Header("UI AUDIO MIXER")]
        public AudioMixerGroup audioMixer;

        Object[] SoundFiles;

        [Header("SOUND SETTINGS")]
        [Range(0,1.0f)]
        public float vol = 1.0f;
        [Space]
        public AudioClip hoverSound;
        [Range(0.5f,2.0f)]
        public float hoverPitch = 1.0f;
        [Space]
        public AudioClip clickSound;
        [Range(0.5f,2.0f)]
        public float clickPitch = 1.0f;
        [Space]
        [Range(0.5f,2.0f)]
        public float exitPitch = 1.0f;
        public AudioClip exitSound;

        public void PlaySound(int soundType){
            SoundFiles = Resources.LoadAll("SlimUI/Sound", typeof(Object));
        }
    }
}
