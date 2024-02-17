using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FinishWindow : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _homeButton;

    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _inactiveColor;

    [SerializeField] private Image[] _stars;

    private bool _isInitialized;

    private void OnEnable()
    {
        if (!_isInitialized)
            return;

        SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    public void Initialize()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        Hide();

        _isInitialized = true;
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameFinished += Show;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameFinished -= Show;
    }
    private void Show()
    {
        _panel.SetActive(true);
        _panel.GetComponent<CanvasGroup>().alpha = 0f;
        _panel.GetComponent<CanvasGroup>().DOFade(1f, 0.6f).SetLink(_panel.gameObject);

        foreach (Image star in _stars)
            star.color = _inactiveColor;

        for (int i = 0; i <= GameState.Instance.Stars - 1; i++)
            _stars[i].color = _activeColor;

        if (PlayerPrefs.GetInt("CurrentLevel", 0) == GameState.Instance.MaxLevels)
        {
            _nextButton.gameObject.SetActive(false);
            _homeButton.gameObject.SetActive(true);
        }
    }
    private void Hide()
    {
        _panel.SetActive(false);
    }
    public void OnRestartButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnMenuButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Menu");
    }
    public void OnNextButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
}