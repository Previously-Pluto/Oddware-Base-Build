using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using NaninovelInventory;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ContextItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Serializable]
    public struct EventID
    {
        [FormerlySerializedAs("ID")]
        public string Name;

        public UnityEvent Event;
        public SpriteStyle SpriteStyle;
        public int EventHash;
        public bool SpecialEvent;
        public bool ConditionMet;
    }
    
    [SerializeField] private string itemName;
    [SerializeField] private List<EventID> events;
    [SerializeField] private List<PlayScript> conditionScripts;
    [SerializeField] private Transform menuPosition;
    
    public Transform MenuPosition => menuPosition;

    public List<EventID> Events => events;
    public List<PlayScript> ConditionScripts => conditionScripts;

    private void OnEnable()
    {
        ContextMenuManager.Instance.RegisterContextItem(this);
    }

    private void OnDisable()
    {
        ContextMenuManager.Instance.DeregisterContextItem(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ContextMenuManager.Instance.SelectedItem = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ContextMenuManager.Instance.SelectedItem = null;
    }
}