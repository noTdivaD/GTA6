using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsManager : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text sliderText = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject settingsUI = null;

    [Header("Post-Processing")]
    [SerializeField] private Volume pauseVolume;

    public void Start()
    {
        settingsUI.SetActive(false);
        pauseVolume.weight = 0f;
    }

    // Open Settings Menu
    public void OpenSettings()
    {
        Debug.Log("[SettingsManager] Opening settings menu...");
        settingsUI.SetActive(true);
        pauseVolume.weight = 1f;
    }

    // Close Settings Menu
    public void CloseSettings()
    {
        Debug.Log("[SettingsManager] Closing settings menu...");
        settingsUI.SetActive(false);
        pauseVolume.weight = 0f;
    }

    // Volume Slider
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        volumeSlider.value = value;
        if (sliderText != null)
        {
            Debug.Log("Setting volume text to: " + value.ToString("0.0"));
            sliderText.text = value.ToString("0.0");
        }
    }

    // Apply Button
    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        PlayerPrefs.Save();
    }
}
