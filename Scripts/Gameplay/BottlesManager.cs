using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottlesManager : MonoBehaviour
{
    public static BottlesManager Instance;

    public float LevelTime;

    [SerializeField] private List<Bottle> _bottles = new List<Bottle>();

    [SerializeField] private Bottle _selectedBottle;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void SelectBottle(Bottle bottle)
    {
        if(_selectedBottle == null)
        {
            if(!bottle.IsEmpty)
            {
                AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.BallSelected, 1f);

                bottle.GetLastBall();
                _selectedBottle = bottle;
            }
        }
        else
        {
            if (_selectedBottle != bottle)
            {
                if (!CheckBottleFill(bottle) && !CheckBottleEmpty(_selectedBottle))
                {
                    if (_selectedBottle != bottle)
                    {
                        PutBallInBottle(_selectedBottle, bottle);
                        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.BallSet, 1f);
                    }
                }
                else
                    AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Wrong, 1f);
            }
            else
            {
                bottle.UndoLastBall();
                _selectedBottle = null;
                AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.BallSet, 1f);
            }
        }
    }
    public void CheckForLevelComplete()
    {
        bool isCompleted = true;
        foreach(Bottle bottle in _bottles)
        {
            if (!bottle.CheckForComplete())
                isCompleted = false;
        }
        if (isCompleted)
        {
            var hud = FindObjectOfType<HUD>();
            int stars = 0;
            if (hud.GetPercentage() <= 0.05f)
            {
                stars = 0;
            }
            else if (hud.GetPercentage() <= 0.35f)
            {
                stars = 1;
            }
            else if (hud.GetPercentage() <= 0.65f)
            {
                stars = 2;
            }
            else if (hud.GetPercentage() > 0.65f)
            {
                stars = 3;
            }
            GameState.Instance.FinishGame(stars);
        }
    }
    private bool CheckBottleEmpty(Bottle bottle)
    {
        return bottle.IsEmpty;
    }
    private bool CheckBottleFill(Bottle bottle)
    {
        return bottle.IsFull;
    }
    private void PutBallInBottle(Bottle ballFrom, Bottle ballIn)
    {
        var ball = ballFrom.RemoveLastBall();
        ballIn.AddBall(ball);
        _selectedBottle = null;
    }
}
