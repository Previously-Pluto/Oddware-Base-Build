using UnityEngine;

namespace PadlockSystem
{
    public class PadlockDoorAnimation : MonoBehaviour
    {
        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            anim.Play("DoorOpen", 0, 0.0f);
        }
    }
}
