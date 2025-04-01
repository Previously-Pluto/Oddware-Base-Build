using UnityEngine;
using UnityEngine.UI;
using AdventurePuzzleKit;

namespace FuseboxSystem
{
    public class FuseboxInventoryController : MonoBehaviour
    {
        [Header("Fuse UI")]
        [SerializeField] private Text fuseUI = null;

        [Header("Fuses in Inventory")]
        public int inventoryFuses;

        public static FuseboxInventoryController instance;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void UpdateFuseUI()
        {
            inventoryFuses++;
            fuseUI.text = inventoryFuses.ToString("0");
            AKUIManager.instance.hasFuse = true;
        }

        public void MinusFuseUI()
        {
            inventoryFuses--;
            fuseUI.text = inventoryFuses.ToString("0");
        }
    }
}
