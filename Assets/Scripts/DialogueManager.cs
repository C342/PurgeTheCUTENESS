using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    public float fadeDuration = 0.5f;
    public float nextSceneDelay = 1.5f;

    private Queue<string> lines = new Queue<string>();
    private bool isDialogueActive = false;
    private bool isTyping = false;

    private Coroutine typingCoroutine;
    private Coroutine fadeCoroutine;

    private string currentLine;

    void Start()
    {        
        Color c = dialogueText.color;
        c.a = 1f;
        dialogueText.color = c;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentLine;
                isTyping = false;
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(List<string> dialogueLines)
    {
        if (isDialogueActive)
            return;

        isDialogueActive = true;

        lines.Clear();

        foreach (string line in dialogueLines)
        {
            lines.Enqueue(line);
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
    }

    void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(currentLine));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            SetAlpha(alpha);
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f);
    }

    void SetAlpha(float alpha)
    {
        Color color = dialogueText.color;
        color.a = alpha;
        dialogueText.color = color;
    }

    void EndDialogue()
    {
        dialogueText.text = "";
        isDialogueActive = false;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut());

        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(nextSceneDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    IEnumerator FadeOut()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            SetAlpha(alpha);
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
    }
}