using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public void TriggerDialogue()
    {
        List<string> lines = new List<string>()
        {
            "THESE ARE YOUR THOUGHTS SPEAKING.",
            "WHO ARE YOU?",
            "NOT SURE, YOUR MEMORY DOESN'T GO BACK THAT FAR.",
            "WHERE ARE YOU?",
            "NOT SURE EITHER.",
            "YOU FEEL DIZZY, LIKE YOUR MIND IS IN THE GUTTER.",
            "YOUR VISION IS GETTING SHAKIER BY THE SECOND.",
            "GET OUT OF THIS PLACE."
        };

        dialogueManager.StartDialogue(lines);
    }

    void OnMouseDown()
    {
        UnityEngine.Debug.Log("Dialogue Triggered");
        TriggerDialogue();
    }
}