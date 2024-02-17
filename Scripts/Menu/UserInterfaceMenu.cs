using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterfaceMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _optionsPanel;

    [SerializeField] private TextMeshProUGUI _recordText;

    [SerializeField] private Slider _audioSlider;
    [SerializeField] private Slider _musicSlider;

    [SerializeField] private Button _vibrationButton;
    [SerializeField] private Sprite _vibrationEnabled;
    [SerializeField] private Sprite _vibrationDisabled;
    
    private void Start()
    {
        OpenMenu();
        _recordText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        UpdateVibrationImage();
    }
    private void UpdateVibrationImage()
    {
        if(AudioVibrationManager.Instance.IsVibrationEnabled)
            _vibrationButton.GetComponent<Image>().sprite = _vibrationEnabled;
        else
            _vibrationButton.GetComponent<Image>().sprite = _vibrationDisabled;
    }
    public void OpenMenu()
    {
        _menuPanel.SetActive(true);
        _optionsPanel.SetActive(false);

        AudioVibrationManager.Instance.Save();
    }
    public void OpenOptions()
    {
        _menuPanel.SetActive(false);
        _optionsPanel.SetActive(true);

        _audioSlider.value = AudioVibrationManager.Instance.Audio;
        _musicSlider.value = AudioVibrationManager.Instance.Music;
    }
    public void ChangeAudio()
    {
        AudioVibrationManager.Instance.ChangeAudio(_audioSlider.value);
    }
    public void ChangeMusic()
    {
        AudioVibrationManager.Instance.ChangeMusic(_musicSlider.value);
    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
    public void OnPlayButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnSettingsButtonClicked()
    {
        OpenOptions();
    }
    public void OnReturnToMenuButtonCliked()
    {
        OpenMenu();
    }
    public void OnVibrationButtonClicked()
    {
        AudioVibrationManager.Instance.ToggleVibration();
        UpdateVibrationImage();
    }
}
