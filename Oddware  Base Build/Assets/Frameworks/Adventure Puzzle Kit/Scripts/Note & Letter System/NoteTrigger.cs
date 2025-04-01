using UnityEngine;
using AdventurePuzzleKit;

namespace NoteSystem
{
    public class NoteTrigger : MonoBehaviour
    {
        [Header("Keypad Object")]
        [SerializeField] private NoteController myNote = null;

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
                    myNote.DisplayNotes();
                    AKDisableManager.instance.DisablePlayer(true);
                }
            }
        }
    }
}
