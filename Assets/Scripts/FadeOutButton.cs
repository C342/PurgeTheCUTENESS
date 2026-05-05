using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonSceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string sceneToLoad;

    private bool isFading;

    private void Start()
    {
        SetAlpha(0f);
    }

    public void OnButtonPressed()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndLoad());
        }
    }

    private IEnumerator FadeOutAndLoad()
    {
        isFading = true;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f);

        yield return new WaitForEndOfFrame();

        SceneManager.LoadScene(sceneToLoad);
    }

    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}