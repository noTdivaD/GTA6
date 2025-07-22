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
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private GameObject settingsUI = null;

    public void Start()
    {
        settingsUI.SetActive(false);
    }

    // Open Settings Menu
    public void OpenSettings()
    {
        settingsUI.SetActive(true);
        Time.timeScale = 0f;
    }

    // Close Settings Menu
    public void CloseSettings()
    {
        Time.timeScale = 1f;
        settingsUI.SetActive(false);
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

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        confirmationPrompt.SetActive(false);
    }
}
