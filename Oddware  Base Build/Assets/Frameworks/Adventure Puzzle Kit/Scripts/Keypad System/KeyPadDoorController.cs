using AdventurePuzzleKit;
using UnityEngine;

namespace KeypadSystem
{
    public class KeyPadDoorController : MonoBehaviour
    {
        private Animator doorAnim;

        [Header("Animation Name")]
        [SerializeField] private string animationName = "OpenDoor";

        [Header("Sound Clip Names")]
        [SerializeField] private string soundClipName = "ThemedKeyDoorOpen";

        private void Awake()
        {
            doorAnim = gameObject.GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            AKAudioManager.instance.Play(soundClipName);
            doorAnim.Play(animationName, 0, 0.0f);
        }
    }
}
