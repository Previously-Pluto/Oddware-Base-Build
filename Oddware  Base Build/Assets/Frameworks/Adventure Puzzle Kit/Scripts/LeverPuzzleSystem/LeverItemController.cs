using UnityEngine;

namespace LeverPuzzleSystem
{
    public class LeverItemController : MonoBehaviour
    {
        [Space(10)] [SerializeField] private ObjectType _objectType = ObjectType.None;

        private enum ObjectType { None, Lever, TestButton, ResetButton }

        private LeverController _leverController = null;

        private void Start()
        {
            _leverController = GetComponent<LeverController>();
        }

        public void ObjectInteract()
        {
            if (_objectType == ObjectType.Lever)
            {
                _leverController.LeverNumber();
            }

            else if (_objectType == ObjectType.TestButton)
            {
                _leverController.LeverCheck();
            }

            else if (_objectType == ObjectType.ResetButton)
            {
                _leverController.LeverReset();
            }
        }
    }
}
