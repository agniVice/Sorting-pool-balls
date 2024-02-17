using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private TextMeshProUGUI _highScore;
    [SerializeField] private Image _progressBar;

    [SerializeField] private Image[] _stars;

    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _inactiveColor;

    private bool _isInitialized;

    private float _levelTime;
    private float _currentTime;

    private float _percentage;

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
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState == GameState.State.InGame)
        {
            _currentTime -= Time.fixedDeltaTime;

            if (_currentTime <= 0)
            {
                _currentTime = 0;
            }
            UpdateProgressBar();
        }
    }
    private void UpdateProgressBar()
    {
        _percentage = GetPercentage();

        _progressBar.fillAmount = _percentage;

        if (_percentage <= 0.01f)
            GameState.Instance.LoseGame();
        else if (_percentage <= 0.05f)
        {
            _stars[2].color = _inactiveColor;
            _stars[1].color = _inactiveColor;
            _stars[0].color = _inactiveColor;
        }
        else if (_percentage <= 0.35f)
        {
            _stars[2].color = _inactiveColor;
            _stars[1].color = _inactiveColor;
            _stars[0].color = _activeColor;
        }
        else if (_percentage <= 0.65f)
        {
            _stars[2].color = _inactiveColor;
            _stars[1].color = _activeColor;
            _stars[0].color = _activeColor;
        }
        else if (_percentage > 0.65f)
        {
            _stars[2].color = _activeColor;
            _stars[1].color = _activeColor;
            _stars[0].color = _activeColor;
        }
    }
    public void Initialize()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        Show();

        _levelNumberText.text = "LEVEL: " + (PlayerPrefs.GetInt("CurrentLevel", 0) + 1);

        _isInitialized = true;

        _currentTime = BottlesManager.Instance.LevelTime;
        _levelTime = BottlesManager.Instance.LevelTime;

        UpdateScore();
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameFinished += Hide;
        GameState.Instance.GamePaused += Hide;
        GameState.Instance.GameUnpaused += Show;

        GameState.Instance.ScoreAdded += UpdateScore;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameFinished -= Hide;
        GameState.Instance.GamePaused -= Hide;
        GameState.Instance.GameUnpaused -= Show;

        GameState.Instance.ScoreAdded -= UpdateScore;
    }
    private void UpdateScore()
    {
        _highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    private void Show()
    {
        _panel.SetActive(true);
    }
    private void Hide()
    {
        _panel.SetActive(false);
    }
    public void OnRestartButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnPauseButtonClicked()
    {
        GameState.Instance.PauseGame();
    }
    public float GetPercentage()
    {
        return _currentTime / _levelTime;
    }
}