// www.SlimUI.com
// Copyright (c) 2018 - 2020 SlimUI. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

// JOIN THE DISCORD FOR SUPPORT OR CONVERSATION: https://discord.gg/7cK4KBf

using UnityEngine;
using UnityEngine.EventSystems;

namespace SlimUI.CursorControllerPro{
    public class ButtonReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
        GameObject controllerObj;
        CursorController cursorController;
        TooltipController tooltipController;

        [Header("TOOLTIP")]
        public bool hasTooltip = false;
        public string title = "Tooltip";
        public string body = "Tooltip information goes here.";

        void Start(){
            controllerObj = GameObject.Find("CursorControl");
            cursorController = controllerObj.GetComponent<CursorController>();
            tooltipController = controllerObj.GetComponent<TooltipController>();
        }

        public void OnPointerEnter(PointerEventData eventData){
            cursorController.FadeIn();
            cursorController.HoverSpeed();
            if(hasTooltip) cursorController.tooltipController.ShowTooltip(); tooltipController.UpdateTooltipText(title, body);
        }

        public void OnPointerExit(PointerEventData eventData){
            cursorController.FadeOut();
            cursorController.NormalSpeed();
            if(hasTooltip) cursorController.tooltipController.HideTooltip();
        }
    }
}