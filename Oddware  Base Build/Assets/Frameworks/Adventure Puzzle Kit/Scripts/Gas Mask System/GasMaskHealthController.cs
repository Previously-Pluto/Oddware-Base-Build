using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GasMaskSystem
{
    public class GasMaskHealthController : MonoBehaviour
    {
        [Header("Health Variables")]
        [Range(0, 100)] public float currentHealth = 100.0f;
        [Range(0, 100)] public float maxHealth = 100.0f;
        public float healthFall = 2;

        [Header("Health Regeneration Variables")]
        [SerializeField] private float currentHealthTimer = 1.0f; //How long it takes before regeneration health
        [SerializeField] private float maxHealthTimer = 1.0f; //Make sure this is the default start time of the regeneration
        [HideInInspector] public bool regenHealth = false; //Whether we should regenerate health or not

        [Header("Health UI Reference")]
        [SerializeField] private Text healthTextUI = null;
        [SerializeField] private Image healthSliderUI = null;

        [Header("Death Event")]
        [SerializeField] private UnityEvent onDeath = null;

        public static GasMaskHealthController instance;

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void UpdateHealth()
        {
            healthTextUI.text = currentHealth.ToString("0");
            healthSliderUI.fillAmount = (currentHealth / maxHealth);
        }

        private void Update()
        {
            if (regenHealth)
            {
                if (currentHealth <= maxHealth)
                {
                    currentHealthTimer -= Time.deltaTime;

                    if (currentHealthTimer <= 0)
                    {
                        currentHealth += Time.deltaTime * 10;
                        UpdateHealth();
                        currentHealthTimer = 0;

                        if (currentHealth >= maxHealth)
                        {
                            currentHealthTimer = maxHealthTimer;
                            regenHealth = false;
                        }
                    }
                }
                else
                {
                    regenHealth = false;
                    currentHealthTimer = maxHealthTimer;
                }
            }
        }

        public void Death()
        {
            onDeath.Invoke();
        }
    }
}
