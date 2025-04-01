using UnityEngine;

namespace LeverPuzzleSystem
{
    public class LeverController : MonoBehaviour
    {
        [Header("Lever Number")]
        public int leverNumber;

        [Header("Animation Name")]
        [SerializeField] private string animationName = "Handle_Pull";

        [Header("Add Lever System Controller Script")]
        [SerializeField] private LeverSystemController _leverSystemController = null;

        private Animator handleAnimation;

        private void Start()
        {
            handleAnimation = GetComponentInChildren<Animator>();
        }

        public void LeverNumber()
        {
            if (_leverSystemController.canPull)
            {
                if (_leverSystemController.pulls <= _leverSystemController.pullLimit - 1)
                {
                    handleAnimation.Play(animationName, 0, 0.0f);
                    _leverSystemController.playerOrder = _leverSystemController.playerOrder + leverNumber;
                    _leverSystemController.pulls++;
                    _leverSystemController.LeverPull();
                }    
            }
        }

        public void LeverReset()
        {
            _leverSystemController.LeverReset();
        }

        public void LeverCheck()
        {
            _leverSystemController.LeverCheck();
        }
    }
}