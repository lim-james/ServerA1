using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

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
            Item item = inventory.items[i];
            if (item == null)
                slots[i].color = defaultColor;
            else 
                slots[i].color = item.color;
        }
    }

    public void UpdateIndicator()
    {
        indicator.position = slots[inventory.index].transform.position;
    }

}
