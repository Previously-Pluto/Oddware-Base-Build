using UnityEngine;

namespace GasMaskSystem
{
    public class GasMaskItemController : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType = ItemType.None;

        private enum ItemType { None, GasMask, Filter }

        public void ObjectInteract()
        {
            if (_itemType == ItemType.GasMask)
            {
                GasMaskController.instance.PickupGasMask();
                gameObject.SetActive(false);
            }

            else if (_itemType == ItemType.Filter)
            {
                GasMaskController.instance.PickupFilter();
                gameObject.SetActive(false);
            }
        }
    }
}
