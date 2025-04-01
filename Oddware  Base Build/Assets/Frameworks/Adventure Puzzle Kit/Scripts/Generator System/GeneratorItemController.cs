using AdventurePuzzleKit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GeneratorSystem
{
    public class GeneratorItemController : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType = ItemType.None;
        private enum ItemType { None, Jerrycan, Generator, FuelBarrel }

        [Header("Item Parameters")]
        [SerializeField] private string itemName = null;
        [SerializeField] private Sprite iconImage = null;
        [Range(0, 1000)] [SerializeField] private float itemFuelAmount = 100;
        [Range(0, 1000)] [SerializeField] private float itemMaxFuelAmount = 500;

        [Header("UI On/Off")]
        [SerializeField] private bool showUI = true;

        [Header("UI Elements")]
        [SerializeField] private GameObject itemCanvas = null;
        [SerializeField] private Text itemNameUI = null;
        [SerializeField] private Image iconImageUI = null;
        [SerializeField] private Text fuelAmountUI = null;
        [SerializeField] private Text maxFuelAmountUI = null;
        [SerializeField] private Image fuelGaugeUI = null;

        [Header("Generator Light Material - ONLY needed if this script is on a generator 3D object")]
        [SerializeField] private Renderer generatorLightMat = null;
        private bool isGenFull;
        private float fuelRequired;
        private bool initCheck = true;

        [Header("Generator System Sounds")]
        [SerializeField] private string pickupJerrycan = "FuelSwish";
        [SerializeField] private string pourIntoGenerator = "WaterPour";

        [Space(10)][SerializeField] private UnityEvent activateGenerator = null;

        private void Awake()
        {
            UpdateItemStats();
        }

        void ActivateGenerator()
        {
            activateGenerator.Invoke();     
        }

        public void ObjectInteract()
        {
            if (_itemType == ItemType.Jerrycan)
            {
                GeneratorInventoryController.instance.UpdateUI(true, itemFuelAmount);
                GeneratorInventoryController.instance.CollectedJerrycan();
                AKAudioManager.instance.Play(pickupJerrycan);
                gameObject.SetActive(false);
            }

            else if (_itemType == ItemType.Generator)
            {
                if (GeneratorInventoryController.instance.hasJerrycan && !isGenFull)
                {
                    if (initCheck)
                    {
                        fuelRequired = itemMaxFuelAmount - itemFuelAmount;
                        initCheck = false;
                    }

                    float inventoryFuel = GeneratorInventoryController.instance.currentFuel;

                    if (inventoryFuel > 0 && itemFuelAmount <= itemMaxFuelAmount)
                    {
                        if (inventoryFuel <= fuelRequired)
                        {
                            itemFuelAmount += inventoryFuel;
                            inventoryFuel = 0;
                        }
                        else
                        {
                            itemFuelAmount = itemMaxFuelAmount;
                            inventoryFuel -= fuelRequired;
                        }

                        fuelRequired = itemMaxFuelAmount - itemFuelAmount;

                        GeneratorInventoryController.instance.UpdateUI(false, inventoryFuel);
                        AKAudioManager.instance.Play(pourIntoGenerator);
                        UpdateItemStats();

                        if (itemFuelAmount >= itemMaxFuelAmount)
                        {
                            itemFuelAmount = itemMaxFuelAmount;
                            UpdateItemStats();
                        }
                    }
                    if(itemFuelAmount >= itemMaxFuelAmount)
                    {
                        isGenFull = true;
                        generatorLightMat.material.color = Color.green;
                        ActivateGenerator();
                    }
                }
            }

            else if (_itemType == ItemType.FuelBarrel)
            {
                if (GeneratorInventoryController.instance.hasJerrycan)
                {
                    if (itemFuelAmount > 0)
                    {
                        float _localCurrentInventory = GeneratorInventoryController.instance.currentFuel;
                        float _localMaxInventory = GeneratorInventoryController.instance.maximumFuel;

                        float _localLeftToFill = _localMaxInventory - _localCurrentInventory;

                        if (itemFuelAmount >= _localLeftToFill)
                        {
                            GeneratorInventoryController.instance.UpdateUI(true, _localLeftToFill);
                            itemFuelAmount -= _localLeftToFill;
                            AKAudioManager.instance.Play(pickupJerrycan);
                            UpdateItemStats();
                        }
                        else
                        {
                            GeneratorInventoryController.instance.UpdateUI(true, itemFuelAmount);
                            itemFuelAmount -= itemFuelAmount;
                            AKAudioManager.instance.Play(pickupJerrycan);
                            UpdateItemStats();
                        }

                    }
                }
            }
        }

        void UpdateItemStats()
        {
            itemNameUI.text = itemName;
            iconImageUI.sprite = iconImage;
            fuelAmountUI.text = itemFuelAmount.ToString("0");
            maxFuelAmountUI.text = itemMaxFuelAmount.ToString("0");
            fuelGaugeUI.fillAmount = (itemFuelAmount / itemMaxFuelAmount);
        }

        public void ShowObjectStats()
        {
            if (showUI)
            {
                itemCanvas.SetActive(true);
            }
        }

        public void HideObjectStats()
        {
            if (showUI)
            {
                itemCanvas.SetActive(false);
            }
        }
	}
}
