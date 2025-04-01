using UnityEngine;
using AdventurePuzzleKit;

namespace ThemedKeySystem
{
    public class ThemedKeyInventoryController : MonoBehaviour
    {
        [HideInInspector] public bool hasHeartKey;
        [HideInInspector] public bool hasDiamondKey;
        [HideInInspector] public bool hasSpadeKey;
        [HideInInspector] public bool hasClubKey;

        [Header("Key Icon UI")]
        [SerializeField] private GameObject heartFullUI = null;
        [SerializeField] private GameObject diamondFullUI = null;
        [SerializeField] private GameObject spadeFullUI = null;
        [SerializeField] private GameObject clubFullUI = null;

        private bool isOpen;

        public static ThemedKeyInventoryController instance;

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void UpdateInventory(string keyName)
        {
            if (keyName == "Heart")
            {
                hasHeartKey = true;
                AKUIManager.instance.hasThemedKey = true;
                heartFullUI.SetActive(true);
            }

            else if (keyName == "Diamond")
            {
                hasDiamondKey = true;
                AKUIManager.instance.hasThemedKey = true;
                diamondFullUI.SetActive(true);
            }

            else if (keyName == "Club")
            {
                hasClubKey = true;
                AKUIManager.instance.hasThemedKey = true;
                clubFullUI.SetActive(true);
            }

            else if (keyName == "Spade")
            {
                hasSpadeKey = true;
                AKUIManager.instance.hasThemedKey = true;
                spadeFullUI.SetActive(true);
            }
        }
    }
}
