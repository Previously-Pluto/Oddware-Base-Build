using AdventurePuzzleKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FlashlightSystem
{
    public class FlashlightController : MonoBehaviour
    {
        [Header("Infinite Flashlight")]
        [SerializeField] private bool infiniteFlashlight = false;

        [Header("Battery Parameters")]
        [SerializeField] private float batteryDeg = 0.01f;
        [SerializeField] private int batteryCount = 1;

        [Header("Battery Reload Timers")]
        [SerializeField] private float replaceBatteryTimer = 1.0f;
        [SerializeField] private float maxReplaceBatteryTimer = 1.0f;

        [Header("Flashlight Parameters")]
        [Range(0, 10)][SerializeField] private float maxFlashlightIntensity = 1.0f;
        [Range(1, 10)][SerializeField] private int flashlightRotationSpeed = 2;
        private bool isFlashlightOn;

        [Header("UI References")]
        [SerializeField] private Image batteryLevel = null;
        [SerializeField] private Text batteryCountUI = null;
        [SerializeField] private Image flashlightIndicator = null;

        [Header("Sounds")]
        [SerializeField] private string flashlightPickupSound = "FlashlightPickup";
        [SerializeField] private string flashlightClickSound = "FlashlightClick";
        [SerializeField] private string flashlightReloadSound = "FlashlightReload";

        [Header("Main Flashlight References")]
        [SerializeField] private Light flashlightSpot = null;
        [SerializeField] private FlashlightMovement flashlightMovement = null;

        private bool showOnce = false;
        private bool shouldUpdate = false;

        public static FlashlightController instance;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        void Start()
        {
            flashlightSpot.intensity = maxFlashlightIntensity;
            batteryCountUI.text = batteryCount.ToString("0");
            flashlightMovement.speed = flashlightRotationSpeed;
        }

        public void EnableInventory()
        {
            enabled = true;
            AKUIManager.instance.hasFlashlight = true;
            AKAudioManager.instance.Play(flashlightPickupSound);
        }

        public void CollectBattery(int batteries)
        {
            batteryCount = batteryCount + batteries;
            batteryCountUI.text = batteryCount.ToString("0");
            AKAudioManager.instance.Play(flashlightPickupSound); 
        }

        void Update()
        {
            if(!AKUIManager.instance.isInventoryOpen)
            {
                if (Input.GetKeyDown(AKInputManager.instance.flashlightSwitch) && !showOnce) //TURNING FLASHLIGHT ON/OFF
                {
                    AKAudioManager.instance.Play(flashlightClickSound);

                    if (flashlightSpot.enabled == false)
                    {
                        isFlashlightOn = true;
                        flashlightSpot.enabled = true;
                        flashlightIndicator.color = Color.white;
                    }
                    else
                    {
                        isFlashlightOn = false;
                        flashlightSpot.enabled = false;
                        flashlightIndicator.color = Color.black;
                    }
                }

                if (!infiniteFlashlight)
                {
                    if (isFlashlightOn)
                    {
                        if (flashlightSpot.intensity <= maxFlashlightIntensity && flashlightSpot.intensity > 0)
                        {
                            flashlightSpot.intensity -= (batteryDeg * Time.deltaTime) * maxFlashlightIntensity;
                            batteryLevel.fillAmount -= batteryDeg * Time.deltaTime;
                        }

                        if (flashlightSpot.intensity >= maxFlashlightIntensity)
                        {
                            flashlightSpot.intensity = maxFlashlightIntensity;
                        }

                        else if (flashlightSpot.intensity <= 0)
                        {
                            flashlightSpot.intensity = 0;
                        }
                    }

                    if (Input.GetKey(AKInputManager.instance.reloadBattery) && batteryCount >= 1)
                    {
                        shouldUpdate = false;
                        replaceBatteryTimer -= Time.deltaTime;
                        AKUIManager.instance.radialIndicator.enabled = true;
                        AKUIManager.instance.radialIndicator.fillAmount = replaceBatteryTimer;

                        if (replaceBatteryTimer <= 0)
                        {
                            batteryCount--;
                            batteryCountUI.text = batteryCount.ToString("0");
                            flashlightSpot.intensity += maxFlashlightIntensity;
                            AKAudioManager.instance.Play(flashlightReloadSound);
                            batteryLevel.fillAmount = maxFlashlightIntensity;

                            replaceBatteryTimer = maxReplaceBatteryTimer;
                            AKUIManager.instance.radialIndicator.fillAmount = maxReplaceBatteryTimer;
                            AKUIManager.instance.radialIndicator.enabled = false;
                        }
                    }
                    else
                    {
                        if (shouldUpdate)
                        {
                            replaceBatteryTimer += Time.deltaTime;
                            AKUIManager.instance.radialIndicator.fillAmount = replaceBatteryTimer;

                            if (replaceBatteryTimer >= maxReplaceBatteryTimer)
                            {
                                replaceBatteryTimer = maxReplaceBatteryTimer;
                                AKUIManager.instance.radialIndicator.fillAmount = maxReplaceBatteryTimer;
                                AKUIManager.instance.radialIndicator.enabled = false;
                                shouldUpdate = false;
                            }
                        }
                    }

                    if (Input.GetKeyUp(AKInputManager.instance.reloadBattery))
                    {
                        shouldUpdate = true;
                    }
                }
            }
        }
    }
}