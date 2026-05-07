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
            "CREATURES OF BONE AND FLESH HUNT YOUR..",
            "<color=#FF69B4>CUTENESS.</color>",
            "...",
            "M.",
            "...",
            "GET OUT OF THIS HELL."
        };

        dialogueManager.StartDialogue(lines);
    }

    public void TriggerSecondDialogue()
    {
        List<string> lines = new List<string>()
        {
            "YOUR HEAD REALLY HURTS.",
            "IT REALLY HURTS TO TALK TO YOU RIGHT NOW.",
            "THEY WANT TO TAKE YOUR <color=#FF69B4>CUTENESS</color> AWAY.",
            "USE YOUR <color=#DFD0B7>RIBS</color> TO FIGHT BACK.",
            "...",
            "A.",
            "...",
            "GET OUT OF THIS HELL..",
            "BEFORE THEY <color=#000000>PURGE</color> YOU."
        };

        dialogueManager.StartDialogue(lines);
    }

    void OnMouseDown()
    {
        TriggerDialogue();
    }
}