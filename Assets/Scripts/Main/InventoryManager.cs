using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun
{
    public class InventoryManager : MonoBehaviour
    {

        [SerializeField]
        private ItemGroupObject itemGroup;
        [SerializeField]
        private Inventory inventory;
        [SerializeField]
        private Color defaultColor;

        [SerializeField]
        private Image[] slots;
        [SerializeField]
        private Transform indicator;

        public void ReloadData()
        {
            for (int i = 0; i < inventory.size; ++i)
            {
                int item = inventory.items[i];
                if (item < 0)
                    slots[i].color = defaultColor;
                else
                    slots[i].color = itemGroup.colors[item];
            }
        }

        public void UpdateIndicator()
        {
            indicator.position = slots[inventory.index].transform.position;
        }
    }
}
