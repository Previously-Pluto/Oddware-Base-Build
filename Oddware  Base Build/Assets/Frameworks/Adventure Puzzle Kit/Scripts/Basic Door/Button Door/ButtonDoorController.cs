using UnityEngine;
using System.Collections;

namespace AdventurePuzzleKit
{
    public class ButtonDoorController : MonoBehaviour
    {
        [Header("Door Object")]
        [SerializeField] private Animator doorAnim = null;

        [SerializeField] private bool doorOpen = false;

        [Header("Door Animation Names")]
        [SerializeField] private string openAnimationName = "DoorOpen";
        [SerializeField] private string closeAnimationName = "DoorClose";

        [Header("Sounds")]
        [SerializeField] private string doorOpenSound = "OpenDoorSound";
        [SerializeField] private string doorCloseSound = "CloseDoorSound";

        [Header("Pause Timer")]
        [SerializeField] private int waitTimer = 1;
        private bool pauseInteraction = false;

        private IEnumerator PauseDoorInteraction()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;

        }

        public void PlayAnimation()
        {
            if (!doorOpen && !pauseInteraction)
            {
                doorAnim.Play(openAnimationName, 0, 0.0f);
                doorOpen = true;
                AKAudioManager.instance.Play(doorOpenSound);
                StartCoroutine(PauseDoorInteraction());
            }

            else if (doorOpen && !pauseInteraction)
            {
                doorAnim.Play(closeAnimationName, 0, 0.0f);
                doorOpen = false;
                AKAudioManager.instance.Play(doorCloseSound);
                StartCoroutine(PauseDoorInteraction());
            }
        }
    }
}
