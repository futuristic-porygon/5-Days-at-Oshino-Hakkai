
using System.Collections.Generic;
using UnityEngine;

public class ItemDragger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private List<ItemDropper> validSlots;
    private Vector3 startPosition;
    private bool isDragging;
    private bool isPlaced;

    private PuzzleManager puzzleManager;
    public void Init(List<ItemDropper> slots, PuzzleManager manager)
    {
        validSlots = slots;
        puzzleManager = manager;

        spriteRenderer.sprite = validSlots[0].GetSprite();
        startPosition = transform.position;
    }

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;

            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    private void OnMouseDown()
    {
        if (isPlaced)
        {
            return;
        }

        isDragging = true;
    }

    private void OnMouseUp()
    {
        if (!isDragging)
        {
            return;
        }

        isDragging = false;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (ItemDropper slot in validSlots)
        {
            if (slot.ContainsPoint(mousePosition) && slot.TryPlace())
            {
                transform.position = slot.transform.position;
                isPlaced = true;

                puzzleManager.PiecePlaced();
                return;
            }
        }

        transform.position = startPosition;
    }
}
