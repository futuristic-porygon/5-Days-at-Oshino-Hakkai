using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;

    public int slotCount;
    public GameObject[] itemPrefabs;

    private bool isGiftSelectionActive;
    private System.Action<GameObject> onGiftItemSelected;


    void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();


        //for (int i = 0; i < slotCount; i++)
        //{
        //    Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        //    if (i < itemPrefabs.Length)
        //    {
        //        GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //        slot.currentItem = item;
        //    }
        //}
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in inventoryPanel.transform)//Sobreescribe los items que estan sin guardar
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)//Crea slots nuevos
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        //Crea los items en el savefile en los lugares en los que estaban guardados
        foreach (InventorySaveData data in inventorySaveData)
        {
            if(data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
           
        }

    }


    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.Log("Inventory full!!!!!!!!!");
        return false;
    }


    public void BeginGiftSelection(System.Action<GameObject> onItemSelected)
    {
        isGiftSelectionActive = true;
        onGiftItemSelected = onItemSelected;
    }

    public void EndGiftSelection()
    {
        isGiftSelectionActive = false;
        onGiftItemSelected = null;
    }

    public void SelectGiftItem(GameObject selectedItem)
    {
        if (!isGiftSelectionActive || selectedItem == null)
            return;

        onGiftItemSelected?.Invoke(selectedItem);
    }

    public bool RemoveItem(GameObject itemToRemove)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem == itemToRemove)
            {
                slot.currentItem = null;
                Destroy(itemToRemove);
                return true;
            }
        }

        return false;
    }


}
