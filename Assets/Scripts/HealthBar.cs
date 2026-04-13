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
    }

    void Update()
    {
        int currentHealth = player.health;
        int maxHealth = player.maxHealth;

        healthBar.value = currentHealth;
        barText.text = currentHealth + "/" + maxHealth;
    }
}