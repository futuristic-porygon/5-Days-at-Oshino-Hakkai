using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool isOpened { get; private set; }
    public string chestID { get; private set; }
    public GameObject itemPrefab;
    public Sprite openedSprite;

    void Start()
    {
        chestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public bool canInteract()
    {
        return !isOpened;
    }

    public void Interact()
    {
        if (!canInteract()) return;
        OpenChest();
    }

    private void OpenChest()
    {
        SetOpened(true);

        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab,transform.position + Vector3.down,Quaternion.identity);
          
        }
    }

    public void SetOpened(bool opened)
    {
        if (isOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}
