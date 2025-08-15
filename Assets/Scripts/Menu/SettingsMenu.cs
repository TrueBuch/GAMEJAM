using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour, IMenu
{
    [SerializeField] protected Toggle _fullscreen;
    [SerializeField] protected Slider _volumeSlider;
    [SerializeField] protected Button _done;

    public void Initialize()
    {
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        _fullscreen.onValueChanged.AddListener(OnFullsceenChanged);

        _done.onClick.AddListener(() => Main.Get<MenuController>().HideAll());
        _volumeSlider.value = PlayerPrefs.GetFloat("volume");
        _fullscreen.isOn = PlayerPrefs.GetInt("fullscreen") == 1;
    }

    protected void OnFullsceenChanged(bool isFull)
    {
        PlayerPrefs.SetInt("fullscreen", isFull ? 1 : 0);
        Main.ChangeScreen();
    }

    protected void OnVolumeChanged(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
    }
}