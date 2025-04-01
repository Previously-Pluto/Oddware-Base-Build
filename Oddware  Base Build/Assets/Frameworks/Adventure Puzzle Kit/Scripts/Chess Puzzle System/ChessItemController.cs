using UnityEngine;

namespace ChessPuzzleSystem
{
    public class ChessItemController : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType = ItemType.None;
  
        private ChessSingleFuseController chessfuseController;
        private ChessFuseBoxController fuseboxController;

        private enum ItemType { None, ChessFuse, Fusebox }

        private void Awake()
        {
            switch (_itemType)
            {
                case ItemType.ChessFuse:
                    chessfuseController = GetComponent<ChessSingleFuseController>();
                    break;
                case ItemType.Fusebox:
                    fuseboxController = GetComponent<ChessFuseBoxController>();
                    break;
            }
        }

        public void ObjectInteract()
        {
            switch (_itemType)
            {
                case ItemType.ChessFuse:
                    chessfuseController.ChessPiecePickup();
                    break;
                case ItemType.Fusebox:
                    fuseboxController.WhatType();
                    break;
            }
        }
    }
}
