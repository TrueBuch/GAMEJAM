using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Toggle _fullscreen;
    [SerializeField] private Slider _volumeSlider;

    public void Initialize()
    {
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        _fullscreen.onValueChanged.AddListener(OnFullsceenChanged);

        _volumeSlider.value = PlayerPrefs.GetFloat("volume");
        _fullscreen.isOn = PlayerPrefs.GetInt("fullscreen") == 1;
    }

    private void OnFullsceenChanged(bool isFull)
    {
        PlayerPrefs.SetInt("fullscreen", isFull ? 1 : 0);
        Main.ChangeScreen();
    }

    private void OnVolumeChanged(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
    }
}