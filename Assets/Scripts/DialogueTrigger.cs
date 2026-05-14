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
            "YOU FEEL DIZZY, LIKE YOUR MIND IS IN THE GUTTER.",
            "YOUR VISION IS GETTING SHAKIER BY THE SECOND.",
            "IT APPEARS YOU'RE IN SOME KIND OF PURGATORY.",
            "THERE ARE CREATURES OF BONE AND FLESH THAT HUNT YOUR..",
            "<color=#FF69B4>CUTENESS.</color>",
            "USE YOUR <color=#DFD0B7>RIBS</color> TO FIGHT BACK.",
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
            "...",
            "A.",
            "...",
            "GET OUT OF THIS HELL..",
            "BEFORE THEY <color=#000000>PURGE</color> YOU."
        };

        dialogueManager.StartDialogue(lines);
    }

    public void TriggerFinalDialogue()
    {
        List<string> lines = new List<string>()
        {
            "IT'S ALMOST LIKE YOUR MISSING HALF YOUR HEAD.",
            "YOUR <color=#1338BE>VISION</color> IS STARTING TO FAIL YOU.",
            "BEWARE OF <color=#3b8132>SPITTERS.</color>",
            "...",
            "Y.",
            "...",
            "YOU REMEMBER YOUR NAME IS <color=#f1ee8e>MAY.</color>",
            "GET OUT OF THIS HELL."
        };

        dialogueManager.StartDialogue(lines);
    }

    void OnMouseDown()
    {
        TriggerDialogue();
    }
}