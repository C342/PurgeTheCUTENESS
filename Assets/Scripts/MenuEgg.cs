using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuEgg : MonoBehaviour
{
    public Image targetImage;
    public TextMeshProUGUI countdownText;

    public float startDelay = 5f;
    public float countdownTime = 240f;
    public Color targetColor = Color.red;

    private float delayTimer;
    private float currentTime;
    private bool countdownStarted;

    private Color startColor;

    void Start()
    {
        if (targetImage == null)
        {
            Debug.LogError("ASSIGN IMG");
            enabled = false;
            return;
        }

        startColor = targetImage.color;
        currentTime = countdownTime;
    }

    void Update()
    {
        if (!countdownStarted)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= startDelay)
                countdownStarted = true;

            return;
        }

        currentTime -= Time.deltaTime;

        float t = 1f - (currentTime / countdownTime);

        targetImage.color = Color.Lerp(startColor, targetColor, t);

        if (countdownText != null)
            countdownText.text = Mathf.CeilToInt(Mathf.Max(currentTime, 0f)).ToString();

        if (currentTime <= 0f)
        {
            targetImage.color = targetColor;
            enabled = false;
        }
    }
}