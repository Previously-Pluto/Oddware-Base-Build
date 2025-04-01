using AdventurePuzzleKit;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

namespace GasMaskSystem
{
    public class GasMaskController : MonoBehaviour
    {
        public enum GasMaskState { GasMaskOn, GasMaskOffInSmoke, GasMaskOffOutOfSmoke }
        public enum FilterState { FilterNumber, FilterAlarm, FilterNormal, FilterValue }
        public enum MaskUIState { MaskNormal, MaskEquipped }

        [Header("Gas Mask Features")]
        [Range(1, 10)][SerializeField] private float maxEquipMaskTimer = 1f; //The maximum time you want to wait before putting on or taking off the mask. Same as "maskTimer"

        [Header("Gas Mask Colours")]
        [SerializeField] private Color maskEquippedColor = Color.green;
        [SerializeField] private Color maskNormalColor = Color.white;
        private MaskUIState _maskUIState;

        private float maskBeforeTimer = 0.99f; //Just a millisecond before the max timer so we can stop this looping, it will autofill from the start function if edited
        [HideInInspector] public bool hasGasMask = false;
        [HideInInspector] public bool gasMaskOn = false;
        private float equipMaskTimer = 1f;
        [HideInInspector] public bool puttingOn = false, puttingOff = false;
        private GasMaskState currentState;

        [Header("Movement Speeds")]
        [SerializeField] private float walkNorm = 5;
        [SerializeField] private float walkGas = 2;
        [SerializeField] private float runNorm = 10;
        [SerializeField] private float runGas = 4;

        [Header("Filter Options")]
        [Range(0, 20)][SerializeField] private float filterFallRate = 2f; //Increase this to make the filter deplete faster
        [Range(0, 100)][SerializeField] private int warningPercentage = 20; //The percentage the system will give a warning

        private float maxFilterTimer = 100f; //Keep this at 100.
        [Range(0, 100)][SerializeField]private float filterTimer = 100f; //Set the same as your max value, do not change!
        private bool hasFilter = true; //Whether you have a filter or not
        private bool filterChanged = false; //Has the filter changed
        [Space(10)] public int maskFilters = 0; //How many filters does your player currently have? Increase this value at the start to give them more!
        private FilterState _filterState;
        private float replaceFilterTimer = 1.0f;
        private float maxReplaceFilterTimer = 1.0f;

        [Space(10)][SerializeField] private float defaultVignette = 0.28f; //Default value of the vignette image effect

        [Header("Filter Colours")]
        [SerializeField] private Color filterAlarmColor = Color.red;
        [SerializeField] private Color filterNormalColor = Color.white;

        [Header("UI References")]
        [SerializeField] private Image maskIconUI = null;
        [SerializeField] private Image filterIconUI = null;
        [SerializeField] private Text filterCountUI = null;
        [SerializeField] private Image filterSliderUI = null;

        private GameObject mainCamera;

        [Header("Effects References")]
        [SerializeField] private VignetteAndChromaticAberration vignetteEffect = null;
        [SerializeField] private BlurOptimized blurEffect = null;
        [SerializeField] private Image visorImageOverlay = null;

        [Header("Sounds")]
        [SerializeField] private string gasMaskWarning = "GasMaskWarning";
        [SerializeField] private string gasMaskChoking = "GasMaskChoking";
        [Space(5)] [SerializeField] private string gasMaskDeepBreath = "GasMaskDeepBreath";
        [SerializeField] private string gasMaskBreathIn = "GasMaskBreathIn";
        [SerializeField] private string gasMaskBreathOut = "GasMaskBreathOut";
        [SerializeField] private string gasMaskBreathing = "GasMaskBreathing";
        [Space(5)] [SerializeField] private string gasMaskReplaceFilter = "GasMaskReplaceFilter";
        [SerializeField] private string gasMaskPickup = "GasMaskPickup";

        private bool canBreath = true; //Can be made visible for debugging - Can the player breath at any time?
        [HideInInspector] private bool playOnce = false;
        private bool shouldUpdateEquip = false;
        private bool shouldUpdateFilter = false;

        public static GasMaskController instance;

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else {  instance = this; DontDestroyOnLoad(gameObject); }
        }

        void Start()
        {
            filterTimer = maxFilterTimer;
            equipMaskTimer = maxEquipMaskTimer;
            maskBeforeTimer = maxEquipMaskTimer - 0.01f;
            UpdateFilterUI(FilterState.FilterNumber);
        }

        void Update()
        {
            if (Input.GetKey(AKInputManager.instance.equipMaskKey) && hasFilter && hasGasMask && !gasMaskOn && !puttingOn)
            {
                shouldUpdateEquip = false;
                equipMaskTimer -= Time.deltaTime;
                AKUIManager.instance.radialIndicator.enabled = true;
                AKUIManager.instance.radialIndicator.fillAmount = equipMaskTimer;

                if (equipMaskTimer <= 0)
                {
                    equipMaskTimer = 1f;
                    AKUIManager.instance.radialIndicator.fillAmount = 1f;
                    AKUIManager.instance.radialIndicator.enabled = false;
                    StartCoroutine(MaskOn());
                    StartCoroutine(Wait());
                }
            }
            else if (Input.GetKey(AKInputManager.instance.equipMaskKey) && hasFilter && hasGasMask && gasMaskOn && !puttingOn)
            {
                shouldUpdateEquip = false;
                equipMaskTimer -= Time.deltaTime;
                AKUIManager.instance.radialIndicator.enabled = true;
                AKUIManager.instance.radialIndicator.fillAmount = equipMaskTimer;
                if (equipMaskTimer <= 0)
                {
                    equipMaskTimer = maxEquipMaskTimer;
                    AKUIManager.instance.radialIndicator.fillAmount = 1f;
                    AKUIManager.instance.radialIndicator.enabled = false;
                    MaskOff();
                    StartCoroutine(Wait());
                }
            }
            else
            {
                if (shouldUpdateEquip)
                {
                    equipMaskTimer += Time.deltaTime;
                    AKUIManager.instance.radialIndicator.fillAmount = equipMaskTimer;

                    if (equipMaskTimer >= maskBeforeTimer)
                    {
                        equipMaskTimer = maxEquipMaskTimer;
                        AKUIManager.instance.radialIndicator.enabled = false;
                        AKUIManager.instance.radialIndicator.fillAmount = maxEquipMaskTimer;
                        shouldUpdateEquip = false;
                        StartCoroutine(Wait());
                    }
                }
            }
            if (Input.GetKeyUp(AKInputManager.instance.equipMaskKey))
            {
                shouldUpdateEquip = true;
            }

            if (hasGasMask && gasMaskOn)
            {
                filterTimer -= Time.deltaTime * filterFallRate;
                UpdateFilterUI(FilterState.FilterValue);
                if (vignetteEffect.intensity <= 0.5)
                {
                    vignetteEffect.intensity += Time.deltaTime / (200 * 10);
                }

                if (filterTimer <= 1)
                {
                    if (maskFilters >= 1)
                    {
                        ReplaceFilter();
                    }
                    else
                    {
                        filterTimer = 0;
                        UpdateFilterUI(FilterState.FilterValue);
                        hasFilter = false;
                        MaskOff();
                    }
                }

                if (filterTimer <= ((maxFilterTimer / 100) * warningPercentage) && !filterChanged)
                {
                    UpdateFilterUI(FilterState.FilterAlarm);
                    AKAudioManager.instance.Play(gasMaskWarning);
                    filterChanged = true;
                }
            }

            if (hasGasMask)
            {
                if (Input.GetKey(AKInputManager.instance.replaceFilterKey) && maskFilters >= 1)
                {
                    shouldUpdateFilter = false;
                    replaceFilterTimer -= Time.deltaTime;
                    AKUIManager.instance.radialIndicator.enabled = true;
                    AKUIManager.instance.radialIndicator.fillAmount = replaceFilterTimer;
                    if (replaceFilterTimer <= 0)
                    {
                        replaceFilterTimer = maxReplaceFilterTimer;
                        AKUIManager.instance.radialIndicator.fillAmount = maxReplaceFilterTimer;
                        AKUIManager.instance.radialIndicator.enabled = false;
                        ReplaceFilter();
                    }
                }
                else
                {
                    if (shouldUpdateFilter)
                    {
                        Debug.Log("I'm ticking up");
                        replaceFilterTimer += Time.deltaTime;
                        AKUIManager.instance.radialIndicator.fillAmount = replaceFilterTimer;

                        if (replaceFilterTimer >= maxReplaceFilterTimer)
                        {
                            replaceFilterTimer = maxReplaceFilterTimer;
                            AKUIManager.instance.radialIndicator.fillAmount = maxReplaceFilterTimer;
                            AKUIManager.instance.radialIndicator.enabled = false;
                            shouldUpdateFilter = false;
                        }
                    }
                }
                if (Input.GetKeyUp(AKInputManager.instance.replaceFilterKey))
                {
                    Debug.Log("Lifted Key");
                    shouldUpdateFilter = true;
                }
            }

            if (!canBreath)
            {
                if (gasMaskOn) currentState = GasMaskState.GasMaskOn;
                else currentState = GasMaskState.GasMaskOffInSmoke;
            }
            else
            {
                if (gasMaskOn) currentState = GasMaskState.GasMaskOn;
                else currentState = GasMaskState.GasMaskOffOutOfSmoke;
            }

            switch (currentState)
            {
                case GasMaskState.GasMaskOffOutOfSmoke:
                    if (playOnce)
                    {
                        AKAudioManager.instance.StopPlaying(gasMaskChoking);
                        AKAudioManager.instance.Play(gasMaskDeepBreath);
                        playOnce = false;
                    }
                    break;
                case GasMaskState.GasMaskOffInSmoke:
                    if (!playOnce)
                    {
                        AKAudioManager.instance.StopPlaying(gasMaskDeepBreath);
                        AKAudioManager.instance.StopPlaying(gasMaskChoking);
                        AKAudioManager.instance.Play(gasMaskChoking);
                        playOnce = true;
                    }

                    GasMaskHealthController.instance.currentHealth -= GasMaskHealthController.instance.healthFall * Time.deltaTime;
                    GasMaskHealthController.instance.UpdateHealth();

                    if (GasMaskHealthController.instance.currentHealth <= 0) GasMaskHealthController.instance.Death();
                    break;
                case GasMaskState.GasMaskOn:
                    AKAudioManager.instance.StopPlaying(gasMaskChoking);
                    AKAudioManager.instance.StopPlaying(gasMaskDeepBreath);
                    break;
            }
            return;
        }

        public void PickupGasMask()
        {
            AKUIManager.instance.hasGasMask = true;
            AKAudioManager.instance.Play(gasMaskPickup);
            hasGasMask = true;
            UpdateMaskUI(MaskUIState.MaskNormal);
        }

        public void PickupFilter()
        {
            AKAudioManager.instance.Play(gasMaskPickup);
            maskFilters++;
            UpdateFilterUI(FilterState.FilterNumber);
        }

        void ReplaceFilter()
        {
            AKAudioManager.instance.Play(gasMaskReplaceFilter);
            filterTimer = maxFilterTimer;
            maskFilters--;
            vignetteEffect.intensity = defaultVignette;
            hasFilter = true;
            UpdateFilterUI(FilterState.FilterNormal);
            UpdateFilterUI(FilterState.FilterNumber);
            UpdateFilterUI(FilterState.FilterValue);
            filterChanged = false;
        }

        public void DamageGas()
        {
            canBreath = false;
            GasMaskHealthController.instance.UpdateHealth();
            GasMaskHealthController.instance.regenHealth = false;
            AKDisableManager.instance.player.m_WalkSpeed = walkGas;
            AKDisableManager.instance.player.m_RunSpeed = runGas;
            blurEffect.enabled = true;
        }

        public void CanBreath()
        {
            canBreath = true;
            GasMaskHealthController.instance.regenHealth = true;
            AKDisableManager.instance.player.m_WalkSpeed = walkNorm;
            AKDisableManager.instance.player.m_RunSpeed = runNorm;
            blurEffect.enabled = false;
        }

        public void UpdateFilterUI(FilterState _filterState)
        {
            switch (_filterState)
            {
                case FilterState.FilterNumber: filterCountUI.text = maskFilters.ToString("0");
                    break;
                case FilterState.FilterAlarm: filterIconUI.color = filterAlarmColor;
                    break;
                case FilterState.FilterNormal: filterIconUI.color = filterNormalColor;
                    break;
                case FilterState.FilterValue: filterSliderUI.fillAmount = (filterTimer / maxFilterTimer);
                    break;
            }
        }

        public void UpdateMaskUI(MaskUIState _maskUIState)
        {
            switch(_maskUIState)
            {
                case MaskUIState.MaskNormal: maskIconUI.color = maskNormalColor;
                    break;
                case MaskUIState.MaskEquipped: maskIconUI.color = maskEquippedColor;
                    break;
            }
        }

        IEnumerator Wait()
        {
            if (!gasMaskOn) puttingOff = true;
            else puttingOn = true;

            const float waitDuration = 2.5f;
            yield return new WaitForSeconds(waitDuration);
            puttingOn = puttingOff = false;
        }

        IEnumerator MaskOn()
        {
            gasMaskOn = true;
            AKAudioManager.instance.Play(gasMaskBreathIn);

            const float waitDuration = 1.5f;
            yield return new WaitForSeconds(waitDuration);
            UpdateMaskUI(MaskUIState.MaskEquipped);
            AKAudioManager.instance.Play(gasMaskBreathing);
            visorImageOverlay.enabled = true;
            vignetteEffect.enabled = true;
        }

        void MaskOff()
        {
            gasMaskOn = false;
            UpdateMaskUI(MaskUIState.MaskNormal);
            AKAudioManager.instance.Play(gasMaskBreathOut);
            AKAudioManager.instance.StopPlaying(gasMaskBreathing);
            AKAudioManager.instance.StopPlaying(gasMaskDeepBreath);
            visorImageOverlay.enabled = false;
            vignetteEffect.enabled = false;
        }
    }
}
