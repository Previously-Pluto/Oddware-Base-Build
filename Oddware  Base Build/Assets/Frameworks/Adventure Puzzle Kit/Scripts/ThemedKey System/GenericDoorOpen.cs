using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThemedKeySystem
{
    public class GenericDoorOpen : MonoBehaviour
    {
        private Animator doorAnim;

        [SerializeField] private string animationName = null;

        private void Start()
        {
            doorAnim = GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            doorAnim.Play(animationName, 0, 0.0f);         
        }
    }
}
