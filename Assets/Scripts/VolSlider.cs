using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VolSlider : MonoBehaviour
{
   private Slider slider;
   [SerializeField] private TextMeshProUGUI volumeText;
   [SerializeField] private GameObject muteIcon;

    void Start()
    {
        slider = GetComponent<Slider>();
        float savedVolume;

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            savedVolume = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            savedVolume = 0.5f;

            PlayerPrefs.SetFloat("MusicVolume", savedVolume);
            PlayerPrefs.Save();
        }

        slider.value = savedVolume;

        UpdateUI(savedVolume);

        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(value);
        }
        UpdateUI(value);
    }

    void UpdateUI(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);

        volumeText.text = percent + "%";
        muteIcon.SetActive(value <= 0.001f);
    }
}