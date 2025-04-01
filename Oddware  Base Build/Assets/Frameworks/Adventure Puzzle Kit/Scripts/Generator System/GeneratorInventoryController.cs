using AdventurePuzzleKit;
using UnityEngine;
using UnityEngine.UI;

namespace GeneratorSystem
{
    public class GeneratorInventoryController : MonoBehaviour
    {
        [Header("Fuel Levels")]
        [Range(0, 100)] public float currentFuel;
        [Range(0, 100)] public float maximumFuel;
        public bool hasJerrycan;

        [HideInInspector] public bool isInventoryOpen;

        [Header("References")]
        [SerializeField] private Image fuelFillUI = null;
        [SerializeField] private Text currentFuelText = null;
        [SerializeField] private Text maximumFuelText = null;

        public static GeneratorInventoryController instance;

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }

            UpdateUI(true, 0);
        }

        public void CollectedJerrycan()
        {
            hasJerrycan = true;
            AKUIManager.instance.hasJerrycan = true;
        }

        public void UpdateUI(bool shouldAdd, float fuelAmount)
        {
            if (shouldAdd)
            {
                if (currentFuel <= maximumFuel)
                {
                    currentFuel += fuelAmount;
                }

                if (currentFuel >= maximumFuel)
                {
                    currentFuel = maximumFuel;
                }
            }

            else
            {
                currentFuel = fuelAmount;
            }

            fuelFillUI.fillAmount = (currentFuel / maximumFuel);
            currentFuelText.text = currentFuel.ToString("0");
            maximumFuelText.text = maximumFuel.ToString("0");
        }
    }
}
