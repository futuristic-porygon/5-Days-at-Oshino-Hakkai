
using UnityEngine;
using UnityEngine.EventSystems;

public class GiftItemSelector : MonoBehaviour, IPointerClickHandler
{
    private InventoryController inventoryController;

    private void Start()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        inventoryController.SelectGiftItem(gameObject);
    }
}
