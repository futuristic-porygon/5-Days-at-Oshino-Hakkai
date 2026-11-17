
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCGiftData", menuName = "NPC Gift Data")]
public class NPCGiftData : ScriptableObject
{
    public List<NPCGiftPreference> giftPreferences;
}

[System.Serializable]
public class NPCGiftPreference
{
    public Item item;
    public GiftReaction reaction;

    [TextArea]
    public string reactionText;
}

public enum GiftReaction
{
    Love,
    Like,
    Neutral,
    Dislike
}
