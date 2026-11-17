
using UnityEngine;

public class NPCGiftSystem : MonoBehaviour
{
    public NPCGiftData giftData;

    public GiftReaction GetGiftReaction(Item givenItem, out string reactionText)
    {
        foreach (NPCGiftPreference preference in giftData.giftPreferences)
        {
            if (preference.item == null)
                continue;

            if (preference.item.ID == givenItem.ID)
            {
                reactionText = preference.reactionText;

                if (string.IsNullOrEmpty(reactionText))
                {
                    reactionText = GetDefaultReaction(preference.reaction);
                }

                return preference.reaction;
            }
        }

        reactionText = "Gracias por el regalo.";
        return GiftReaction.Neutral;
    }

    private string GetDefaultReaction(GiftReaction reaction)
    {
        switch (reaction)
        {
            case GiftReaction.Love:
                return "¡Me encanta!";

            case GiftReaction.Like:
                return "¡Muchas gracias!";

            case GiftReaction.Dislike:
                return "No es algo que me guste mucho...";

            default:
                return "Gracias por el detalle.";
        }
    }
}
