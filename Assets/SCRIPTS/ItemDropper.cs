
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private string slotType;
    [SerializeField, Range(0f, 1f)] private float slotAlpha = 0.3f;

    private Collider2D slotCollider;
    private bool isOccupied;

    public string SlotType => slotType;

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        slotCollider = GetComponent<Collider2D>();

        Color color = spriteRenderer.color;
        color.a = slotAlpha;
        spriteRenderer.color = color;
    }

    public bool ContainsPoint(Vector2 position)
    {
        return slotCollider != null && slotCollider.OverlapPoint(position);
    }

    public bool TryPlace()
    {
        if (isOccupied)
        {
            return false;
        }

        isOccupied = true;
        return true;
    }
}
