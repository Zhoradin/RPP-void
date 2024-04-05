using UnityEngine;

public class KarlManager : MonoBehaviour
{
    private Animator karlAnimator;
    private DialogueManager dialogueManager;

    private bool endDialogueTagDetected = false;
    private bool dialogueEnded = false;

    private void Start()
    {
        karlAnimator = GameObject.Find("Karl").GetComponent<Animator>();
        dialogueManager = DialogueManager.GetInstance();
    }

    private void Update()
    {
        // Check if the dialogue has ended and #end_dialogue tag is detected
        if (dialogueEnded && endDialogueTagDetected)
        {
            // Play the Karl_Vanish animation
            karlAnimator.SetTrigger("EndDialogue");
        }
    }

    public void HandleEndDialogue()
    {
        // This function will be called by DialogueManager when #end_dialogue tag is detected
        // Set the flag to indicate that the #end_dialogue tag is detected
        endDialogueTagDetected = true;
    }

    public void OnDialogueEnd()
    {
        // This function will be called by DialogueManager when the dialogue is completely ended
        // Set the flag to indicate that the dialogue has ended
        dialogueEnded = true;

        // Optionally, you can also perform additional actions here after the dialogue is completely ended
        Debug.Log("Diyalog Bitti");
    }
}
