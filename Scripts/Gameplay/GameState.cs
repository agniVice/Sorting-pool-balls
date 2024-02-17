using System;
using UnityEngine;

public class GameState : MonoBehaviour, IInitializable
{
    public static GameState Instance;

    public Action GameStarted;
    public Action GamePaused;
    public Action GameUnpaused;
    public Action GameFinished;
    public Action GameLosed;

    public Action ScoreAdded;

    public enum State
    { 
        Ready,
        InGame,
        Paused,
        Finished
    }
    public State CurrentState { get; private set; }

    public int MaxLevels;
    public int Stars;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void Initialize()
    {
        CurrentState = State.Ready;
    }
    public void ChangeState(State state)
    {
        switch (state)
        { 
            case State.InGame:
                    {
                        break;
                    }
            case State.Paused:
                {
                    break;
                }
            case State.Finished:
                {
                    break;
                }
        }
    }
    public void StartGame()
    {
        GameStarted?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void PauseGame()
    {
        GamePaused?.Invoke();
        CurrentState = State.Paused;
        Time.timeScale = 0.0f;
    }
    public void UnpauseGame()
    {
        GameUnpaused?.Invoke();
        CurrentState = State.InGame;
        Time.timeScale = 1.0f;
    }
    public void FinishGame(int stars)
    {
        Stars = stars;

        PlayerScore.Instance.AddScore(Stars);
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 0) + 1);

        GameFinished?.Invoke();
        CurrentState = State.Finished;

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);

        //if(AudioVibrationManager.Instance.IsVibrationEnabled)
            //Handheld.Vibrate();
    }
    public void LoseGame()
    {
        GameLosed?.Invoke();
        CurrentState = State.Finished;

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Wrong, 1f);

        //if (AudioVibrationManager.Instance.IsVibrationEnabled)
            //Handheld.Vibrate();
    }
}