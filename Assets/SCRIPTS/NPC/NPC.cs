using System.Collections;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
   private DialogueManager dialogueManager;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    //REGALOS

    public NPCGiftSystem giftSystem;

    private InventoryController inventoryController;
    private MenuController menuController;

    private DialogueChoice activeChoice;
    private GameObject selectedGift;

    private GiftReaction selectedReaction;
    private string selectedReactionText;

    private bool isGiftFlowActive;

    private void Start()
    {
        dialogueManager =  DialogueManager.Instance;

        inventoryController = FindAnyObjectByType<InventoryController>();
        menuController = FindAnyObjectByType<MenuController>();
    }
    public bool canInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (isGiftFlowActive)
            return;
        if (dialogueData == null || (PauseManager.isGamePaused && !isDialogueActive))
            return;
        if (isDialogueActive)
        {
            Nextline();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;
      
        dialogueManager.SetNPCInfo(dialogueData.name);
        dialogueManager.ShowDialogueUI(true);
        PauseManager.SetPause(true);

        DisplayCurrentLine();
    }

    void Nextline()
    {
        if(isTyping)
        {
            StopAllCoroutines();
            dialogueManager.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        dialogueManager.ClearChoices();

        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        foreach(DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if(dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueManager.SetDialogueText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {            
            dialogueManager.SetDialogueText(dialogueManager.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping=false;
       
    }


    void DisplayChoices(DialogueChoice choice)
    {
        activeChoice = choice;

        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];

            if (i == choice.giftChoiceIndex)
            {
                dialogueManager.CreateChoiceButton(
                    choice.choices[i],
                    StartGiftSelection
                );
            }
            else
            {
                dialogueManager.CreateChoiceButton(
                    choice.choices[i],
                    () => ChooseOption(nextIndex)
                );
            }
        }
    }


    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueManager.ClearChoices();
        DisplayCurrentLine();
       
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive=false;
        dialogueManager.SetDialogueText("");
        dialogueManager.ShowDialogueUI(false);
        PauseManager.SetPause(false);
    }

    void StartGiftSelection()
    {
        if (giftSystem == null || inventoryController == null || menuController == null)
        {
            Debug.LogWarning("Gift system references are missing!");
            return;
        }

        isGiftFlowActive = true;

        inventoryController.BeginGiftSelection(SelectGiftItem);
        menuController.OpenInventoryForGift();

        dialogueManager.ShowDialogueUI(false);
    }

    void SelectGiftItem(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();

        if (item == null)
            return;

        selectedGift = itemObject;

        selectedReaction = giftSystem.GetGiftReaction(
            item,
            out selectedReactionText
        );

        dialogueManager.ShowDialogueUI(true);
        dialogueManager.SetDialogueText("GIFT?");

        dialogueManager.ClearChoices();

        dialogueManager.CreateChoiceButton("YES", ConfirmGift);
        dialogueManager.CreateChoiceButton("NO", CancelGift);
    }

    void ConfirmGift()
    {
        inventoryController.EndGiftSelection();
        menuController.CloseInventoryForGift();

        if (!inventoryController.RemoveItem(selectedGift))
        {
            Debug.LogWarning("The selected gift could not be removed!");
            selectedGift = null;
            isGiftFlowActive = false;
            dialogueManager.SetDialogueText("No se pudo entregar el regalo.");
            dialogueManager.ClearChoices();
            dialogueManager.CreateChoiceButton("Continuar", FinishGiftReaction);
            return;
        }

        selectedGift = null;

        dialogueManager.SetDialogueText(selectedReactionText);
        dialogueManager.ClearChoices();

        dialogueManager.CreateChoiceButton("Continuar", FinishGiftReaction);
    }

    void CancelGift()
    {
        inventoryController.EndGiftSelection();
        menuController.CloseInventoryForGift();

        selectedGift = null;
        isGiftFlowActive = false;

        dialogueManager.ShowDialogueUI(true);
        dialogueManager.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);

        dialogueManager.ClearChoices();
        DisplayChoices(activeChoice);
    }

    void FinishGiftReaction()
    {
        isGiftFlowActive = false;

        EndDialogue();
    }

}
