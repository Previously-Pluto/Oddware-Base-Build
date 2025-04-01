using UnityEngine;
using AdventurePuzzleKit;

namespace SafeUnlockSystem
{
    public class SafeTrigger : MonoBehaviour
    {
        [Header("Safe Controller Object")]
        [SerializeField] private SafeItemController _safeItemController = null;

        private bool canUse;
        private const string playerTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = true;
                AKUIManager.instance.triggerInteractPrompt.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = false;
                AKUIManager.instance.triggerInteractPrompt.SetActive(false);
            }
        }

        private void Update()
        {
            if (canUse)
            {
                if (Input.GetKeyDown(AKInputManager.instance.triggerInteractKey))
                {
                    AKUIManager.instance.triggerInteractPrompt.SetActive(false);
                    _safeItemController.ShowSafeLock();
                }
            }
        }
    }
}
