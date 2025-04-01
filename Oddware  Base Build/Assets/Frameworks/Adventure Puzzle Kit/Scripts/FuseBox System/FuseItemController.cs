using UnityEngine;
using AdventurePuzzleKit;

namespace FuseboxSystem
{
    public class FuseItemController : MonoBehaviour
    {
        [Space(10)] [SerializeField] private ObjectType _objectType = ObjectType.None;

        private enum ObjectType { None, Fusebox, Fuse }

        public void ObjectInteract()
        {
            if (_objectType == ObjectType.Fusebox)
            {
                gameObject.GetComponent<FuseboxController>().CheckFuseBox();
            }

            else if (_objectType == ObjectType.Fuse)
            {
                FuseboxInventoryController.instance.UpdateFuseUI();
                AKAudioManager.instance.Play("FuseBoxPickup");
                gameObject.SetActive(false);
            }
        }
    }
}
