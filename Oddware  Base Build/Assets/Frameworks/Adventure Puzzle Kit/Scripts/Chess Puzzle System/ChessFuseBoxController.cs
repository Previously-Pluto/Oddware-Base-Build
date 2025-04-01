using UnityEngine;
using AdventurePuzzleKit;

namespace ChessPuzzleSystem
{
    public class ChessFuseBoxController : MonoBehaviour
    {
        [Header("Fuse Box Properties")]
        [SerializeField] private FuseBoxType _fuseBoxType = FuseBoxType.None;
        [SerializeField] private enum FuseBoxType { None, PawnFuseBox, RookFuseBox, KnightFuseBox, BishopFuseBox, QueenFuseBox, KingFuseBox }

        [Header("Fuse placed by default?")]
        [SerializeField] private FuseType _fuseType;
        private enum FuseType { None, PawnFuse, RookFuse, KnightFuse, BishopFuse, QueenFuse, KingFuse }
        public bool fusePlaced;

        [HideInInspector] public string fuseBoxName;
        [HideInInspector] public string fuseName;

        [Header("Fuse Objects")]
        [SerializeField] private GameObject pawnObject = null;
        [SerializeField] private GameObject rookObject = null;
        [SerializeField] private GameObject knightObject = null;
        [SerializeField] private GameObject bishopObject = null;
        [SerializeField] private GameObject queenObject = null;
        [SerializeField] private GameObject kingObject = null;

        [Header("Power Manager Reference")]
        [SerializeField] private ChessPowerManager powerManager = null;

        [Header("Light Object")]
        [SerializeField] private Renderer fuseBoxLight = null;

        private ChessFuseBoxController fuseBoxController;

        private void Awake()
        {
            fuseBoxController = GetComponent<ChessFuseBoxController>();

            switch(_fuseBoxType)
            {
                case FuseBoxType.PawnFuseBox: fuseBoxName = "Pawn"; break;
                case FuseBoxType.RookFuseBox: fuseBoxName = "Rook"; break;
                case FuseBoxType.KnightFuseBox: fuseBoxName = "Knight"; break;
                case FuseBoxType.BishopFuseBox: fuseBoxName = "Bishop"; break;
                case FuseBoxType.QueenFuseBox: fuseBoxName = "Queen"; break;
                case FuseBoxType.KingFuseBox: fuseBoxName = "King"; break;
            }

            if (fusePlaced)
            {
                switch (_fuseType)
                {
                    case FuseType.PawnFuse: _fuseType = FuseType.PawnFuse; fuseName = "Pawn"; ModelSwitch();  pawnObject.SetActive(true); break;
                    case FuseType.RookFuse: _fuseType = FuseType.RookFuse; fuseName = "Rook"; ModelSwitch(); rookObject.SetActive(true); break;
                    case FuseType.KnightFuse: _fuseType = FuseType.KnightFuse; fuseName = "Knight"; ModelSwitch(); knightObject.SetActive(true); break;
                    case FuseType.BishopFuse: _fuseType = FuseType.BishopFuse; fuseName = "Bishop"; ModelSwitch(); bishopObject.SetActive(true); break;
                    case FuseType.QueenFuse: _fuseType = FuseType.QueenFuse; fuseName = "Queen"; ModelSwitch(); queenObject.SetActive(true); break;
                    case FuseType.KingFuse: _fuseType = FuseType.KingFuse;fuseName = "King"; ModelSwitch(); kingObject.SetActive(true); break;
                }
            }
        }

        public void WhatType()
        {
            ChessInventoryManager.instance.invfuseBoxController = fuseBoxController;
            ChessInventoryManager.instance.OpenInventory();
        }

        private void ModelSwitch()
        {
            fuseBoxLight.material.color = Color.green;
            pawnObject.SetActive(false);
            rookObject.SetActive(false);
            knightObject.SetActive(false);
            bishopObject.SetActive(false);
            queenObject.SetActive(false);
            kingObject.SetActive(false);        
        }

        private void RemoveModel()
        {
            pawnObject.SetActive(false);
            rookObject.SetActive(false);
            knightObject.SetActive(false);
            bishopObject.SetActive(false);
            queenObject.SetActive(false);
            kingObject.SetActive(false);

            fusePlaced = false;
            fuseName = null;
            fuseBoxLight.material.color = Color.red;
        }

        public void PlaceFuse(string fuseType)
        {
            switch (fuseType)
            {
                case "Pawn":
                    if (ChessInventoryManager.instance.hasPawnFuse && !fusePlaced)
                    {
                        _fuseType = FuseType.PawnFuse;
                        fuseName = "Pawn";
                        fusePlaced = true;
                        ModelSwitch();
                        pawnObject.SetActive(true);
                    }
                    break;
                case "Rook":
                    if (ChessInventoryManager.instance.hasRookFuse && !fusePlaced)
                    {
                        _fuseType = FuseType.RookFuse;
                        fuseName = "Rook";
                        fusePlaced = true;
                        ModelSwitch();
                        rookObject.SetActive(true);
                    }
                    break;
                case "Knight":
                    if (ChessInventoryManager.instance.hasKnightFuse && !fusePlaced)
                    {
                        _fuseType = FuseType.KnightFuse;
                        fuseName = "Knight";
                        fusePlaced = true;
                        ModelSwitch();
                        knightObject.SetActive(true);
                    }
                    break;
                case "Bishop":
                    if (ChessInventoryManager.instance.hasBishopFuse && !fusePlaced)
                    {
                        _fuseType = FuseType.BishopFuse;
                        fuseName = "Bishop";
                        fusePlaced = true;
                        ModelSwitch();
                        bishopObject.SetActive(true);
                    }
                    break;
                case "Queen":
                    if (ChessInventoryManager.instance.hasQueenFuse && !fusePlaced)
                    {
                        _fuseType = FuseType.QueenFuse;
                        fuseName = "Queen";
                        fusePlaced = true;
                        ModelSwitch();
                        queenObject.SetActive(true);
                    }
                    break;
                case "King":
                    if (ChessInventoryManager.instance.hasKingFuse && !fusePlaced)
                    {
                        _fuseType = FuseType.KingFuse;
                        fuseName = "King";
                        fusePlaced = true;
                        ModelSwitch();
                        kingObject.SetActive(true);
                    }
                    break;
                case "RemoveFuse":
                        RemoveModel();
                    break;
            }
            AKAudioManager.instance.Play("ChessInsert");
            powerManager.CheckFuses();
        }
    }
}