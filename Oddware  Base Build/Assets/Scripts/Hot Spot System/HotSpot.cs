using System.Collections.Generic;
using Highlighters;
using SlimUI.CursorControllerPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RoomManagement
{
    public abstract class HotSpot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Color defaultColor = Color.white;
        [SerializeField] protected Color highlightColor = Color.blue;

        protected Renderer renderer;
        protected Material[] materials;
        protected Highlighter highlighter;
        protected ButtonReceiver _receiver;
        protected SoundReceiver _soundReceiver;
        protected Collider[] _colliders;
        
        public virtual void Awake()
        {
            _colliders = GetComponentsInChildren<Collider>();
            _receiver = GetComponent<ButtonReceiver>();
            _soundReceiver = GetComponent<SoundReceiver>();

            renderer = GetComponent<Renderer>();
            materials = renderer.materials;
            highlighter = GetComponent<Highlighter>();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            highlighter.enabled = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            highlighter.enabled = false;
        }

        public void ToggleHotSpot(bool toggle)
        {
            if (toggle == false)
                highlighter.enabled = toggle;
            enabled = toggle;
            _receiver.enabled = toggle;
            _soundReceiver.enabled = toggle;
            foreach (var collider1 in _colliders)
            {
                collider1.enabled = toggle;
            }
        }
    }
}