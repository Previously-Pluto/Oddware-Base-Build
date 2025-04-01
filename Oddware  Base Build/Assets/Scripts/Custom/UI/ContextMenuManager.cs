using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Highlighters_BuiltIn;
using Naninovel;
using Naninovel.UI;
using NaninovelInventory;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ContextMenuManager : MonoBehaviour
{
    [SerializeField] private UltimateRadialMenu defaultMenu;
    [SerializeField] private InputActionReference contextMenuToggle;

    public static ContextMenuManager Instance;
    public ContextItem SelectedItem { get; set; }
    private List<ContextItem> activeContextItems = new List<ContextItem>();

    public List<ContextItem> ActiveContextItems
    {
        get { return activeContextItems; }
        set { activeContextItems = value; }
    }

    public InventoryUI Inventory { get; set; }
    private List<ContextMenuInputBlocker> inputBlockers = new List<ContextMenuInputBlocker>();
    public InputActionReference ContextMenuToggleAction => contextMenuToggle;

    private PositionController menuPositionController;
    private Camera refCam;
    private RectTransform menuRect;

    private void Awake()
    {
        Instance = this;
        inputBlockers = FindObjectsOfType<ContextMenuInputBlocker>(true).ToList();
        menuPositionController = GetComponent<PositionController>();

        Engine.OnInitializationFinished += () =>
        {
            refCam = Engine.GetService<ICameraManager>().Camera;
            refCam.gameObject.AddComponent<HighlightsManager>();
        };

        menuRect = defaultMenu.GetComponent<RectTransform>();
        foreach (ContextMenuInputBlocker inputBlocker in inputBlockers)
        {
            inputBlocker.ContextMenuManager = this;
        }
    }

    private void OnEnable()
    {
        contextMenuToggle.action.Enable();
        contextMenuToggle.action.started += ToggleContextMenu;
    }

    private void OnDisable()
    {
        contextMenuToggle.action.Disable();
        contextMenuToggle.action.started -= ToggleContextMenu;
    }

    public void OpenContextMenu(ContextItem contextItem)
    {
        defaultMenu.ClearMenu(1);
        defaultMenu.gameObject.SetActive(true);
        
        //Check if context item special event buttons should appear
        foreach (PlayScript script in contextItem.ConditionScripts)
        {
            script.Play();
        }

        foreach (ContextItem.EventID eventID in contextItem.Events)
        {
            if (eventID.SpecialEvent && !eventID.ConditionMet) continue;
            UltimateRadialButtonInfo btnInfo = new UltimateRadialButtonInfo();
            defaultMenu.RegisterButton(() =>
            {
                eventID.Event.Invoke();
                CloseContextMenu();
            }, btnInfo);

            btnInfo.UpdateIcon(eventID.SpriteStyle.NormalSprite);
            defaultMenu.OnButtonEnter += btnIndex =>
            {
                if (btnIndex == btnInfo.GetButtonIndex)
                {
                    defaultMenu.iconHighlightedColor = eventID.SpriteStyle.HighlightedColor;
                    btnInfo.UpdateIcon(eventID.SpriteStyle.HightlightedSprite != null
                        ? eventID.SpriteStyle.HightlightedSprite
                        : eventID.SpriteStyle.NormalSprite);
                }
            };

            defaultMenu.OnButtonExit += btnIndex =>
            {
                if (btnIndex == btnInfo.GetButtonIndex)
                {
                    defaultMenu.iconHighlightedColor = eventID.SpriteStyle.NormalColor;
                    btnInfo.UpdateIcon(eventID.SpriteStyle.NormalSprite);
                }
            };

            defaultMenu.OnButtonSelected += btnIndex =>
            {
                if (btnIndex == btnInfo.GetButtonIndex)
                {
                    defaultMenu.iconSelectedColor = eventID.SpriteStyle.SelectedColor;
                    btnInfo.UpdateIcon(eventID.SpriteStyle.SelectedSprite != null
                        ? eventID.SpriteStyle.SelectedSprite
                        : eventID.SpriteStyle.NormalSprite);
                }
            };
        }

        if (contextItem.MenuPosition != null)
            menuRect.anchoredPosition = refCam.WorldToScreenPoint(contextItem.MenuPosition.position);
        else
            menuRect.anchoredPosition = menuPositionController.GetPosition(menuRect, Input.mousePosition);

    }

    public void CloseContextMenu()
    {
        defaultMenu.gameObject.SetActive(false);
        ToggleInput(true);
    }

    public void ToggleContextMenu(InputAction.CallbackContext callbackContext)
    {
        if (SelectedItem != null)
            OpenContextMenu(SelectedItem);
        else
            CloseContextMenu();
    }

    public void ClearContextItem()
    {
        SelectedItem = null;
    }
    
    public void ToggleInput(bool toggle)
    {
        if (toggle)
            contextMenuToggle.action.Enable();
        else
            contextMenuToggle.action.Disable();
    }

    public void RegisterContextItem(ContextItem contextItem)
    {
        if (!ActiveContextItems.Contains(contextItem))
            ActiveContextItems.Add(contextItem);
    }

    public void DeregisterContextItem(ContextItem contextItem)
    {
        if (ActiveContextItems.Contains(contextItem))
            ActiveContextItems.Remove(contextItem);
    }

    public void SetEventStatus(int eventHash, bool toggle)
    {
        if (SelectedItem != null)
        {
            int index = SelectedItem.Events.FindIndex(eventStruct => eventStruct.EventHash == eventHash);
            if (SelectedItem.Events[index].ConditionMet != toggle)
            {
                ContextItem.EventID newIDData = SelectedItem.Events[index];
                newIDData.ConditionMet = toggle;
                SelectedItem.Events[index] = newIDData;
            }
        }
    }
}