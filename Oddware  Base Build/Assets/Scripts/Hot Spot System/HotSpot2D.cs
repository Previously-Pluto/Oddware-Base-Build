using UnityEngine;
using UnityEngine.EventSystems;

namespace RoomManagement
{
    public class HotSpot2D : HotSpot
    {
        private SpriteRenderer spriteRenderer;
        private PolygonCollider2D _collider;
        
        public override void Awake()
        {
            base.Awake();
            spriteRenderer = renderer.GetComponent<SpriteRenderer>();
            _collider = GetComponent<PolygonCollider2D>();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            highlighter.enabled = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            highlighter.enabled = false;
        }
    }
}