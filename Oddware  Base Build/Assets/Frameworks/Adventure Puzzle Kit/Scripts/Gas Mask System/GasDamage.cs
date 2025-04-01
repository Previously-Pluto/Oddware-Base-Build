using UnityEngine;

namespace GasMaskSystem
{
    public class GasDamage : MonoBehaviour
    {
        private const string playerTag = "Player";

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(playerTag) && !GasMaskController.instance.gasMaskOn)
            {
                GasMaskController.instance.DamageGas();
            }

            else if (other.CompareTag(playerTag) && GasMaskController.instance.gasMaskOn)
            {
                GasMaskController.instance.CanBreath();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag) && !GasMaskController.instance.gasMaskOn)
            {
                GasMaskController.instance.CanBreath();
            }
        }
    }
}
