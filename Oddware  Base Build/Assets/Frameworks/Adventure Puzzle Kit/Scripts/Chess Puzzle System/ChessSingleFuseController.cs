using UnityEngine;
using AdventurePuzzleKit;

namespace ChessPuzzleSystem
{
    public class ChessSingleFuseController : MonoBehaviour
    {
        [Header("Type of Key")]
        [SerializeField] private ChessPieceTheme chessType = ChessPieceTheme.None;

        public enum ChessPieceTheme { None, Pawn, Rook, Knight, Bishop, Queen, King }

        public void ChessPiecePickup()
        {
            switch (chessType)
            {
                case ChessPieceTheme.Pawn:
                    ChessInventoryManager.instance.UpdateInventory(ChessInventoryManager.InventoryPiece.Pawn);
                    break;
                case ChessPieceTheme.Rook:
                    ChessInventoryManager.instance.UpdateInventory(ChessInventoryManager.InventoryPiece.Rook);
                    break;
                case ChessPieceTheme.Knight:
                    ChessInventoryManager.instance.UpdateInventory(ChessInventoryManager.InventoryPiece.Knight);
                    break;
                case ChessPieceTheme.Bishop:
                    ChessInventoryManager.instance.UpdateInventory(ChessInventoryManager.InventoryPiece.Bishop);
                    break;
                case ChessPieceTheme.Queen:
                    ChessInventoryManager.instance.UpdateInventory(ChessInventoryManager.InventoryPiece.Queen);
                    break;
                case ChessPieceTheme.King:
                    ChessInventoryManager.instance.UpdateInventory(ChessInventoryManager.InventoryPiece.King);
                    break;
            }
            AKAudioManager.instance.Play("ChessPiecePickup");
            gameObject.SetActive(false);
        }
    }
}
