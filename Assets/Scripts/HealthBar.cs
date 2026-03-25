using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI barText;

    public int maxHealth;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        barText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

        healthBar.value = currentHealth;
        healthBar.maxValue = maxHealth;
    }
}
