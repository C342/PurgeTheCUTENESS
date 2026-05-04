using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI barText;
    [SerializeField] private PlayerController player;

    void Start()
    {
        healthBar.maxValue = player.maxHealth;
        healthBar.minValue = 0;
    }

    void Update()
    {
        int currentHealth = player.health;
        int maxHealth = player.maxHealth;

        healthBar.value = Mathf.Lerp(healthBar.value, currentHealth, Time.deltaTime * 10f);
        barText.text = $"{currentHealth} / {maxHealth}";

        if (currentHealth < maxHealth * 0.3f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.red;
        }
    }
}