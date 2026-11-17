using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public string[] dialogueLines;
    public bool[] endDialogueLines;
    public float typingSpeed = 0.05F;
    //public AudioClip voiceSound;
    //public float SoundPitch =1f;

    public DialogueChoice[] choices;



}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;
    public string[] choices;
    public int[] nextDialogueIndexes;

    public int giftChoiceIndex = -1;
}
