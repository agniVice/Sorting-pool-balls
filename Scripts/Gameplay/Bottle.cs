using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bottle : MonoBehaviour
{
    [SerializeField] private List<Ball> _ballsInBottle = new List<Ball>();

    [SerializeField] private Transform[] _ballSpawnPoints;

    public Transform _ballUpper;

    public bool IsFull => _ballsInBottle.Count >= 4;
    public bool IsEmpty => _ballsInBottle.Count == 0;

    public void AddBall(Ball ball)
    {
        if (!IsFull)
        {
            _ballsInBottle.Add(ball);
            ball.transform.DOMove(_ballUpper.position, 0.3f).SetLink(ball.gameObject).OnKill(() => { 
                ball.transform.DOMove(_ballSpawnPoints[_ballsInBottle.Count - 1].position, 0.3f).SetLink(ball.gameObject); });

            BottlesManager.Instance.CheckForLevelComplete();
        }
    }
    public Ball RemoveLastBall()
    {
        if (_ballsInBottle.Count > 0)
        {
            int lastIndex = _ballsInBottle.Count - 1;
            Ball ballToRemove = _ballsInBottle[lastIndex];
            _ballsInBottle.RemoveAt(lastIndex);
            return ballToRemove;
        }
        return null;
    }
    public void GetLastBall()
    {
        if (!IsEmpty)
        {
            _ballsInBottle[_ballsInBottle.Count - 1].transform.DOMove(_ballUpper.position, 0.3f).SetLink(_ballsInBottle[_ballsInBottle.Count - 1].gameObject);
        }
    }
    public void UndoLastBall()
    {
        _ballsInBottle[_ballsInBottle.Count - 1].transform.DOMove(_ballSpawnPoints[_ballsInBottle.Count-1].position, 0.3f).SetLink(_ballsInBottle[_ballsInBottle.Count - 1].gameObject);
    }
    private void OnMouseDown()
    {
        if (GameState.Instance.CurrentState == GameState.State.Ready)
        {
            GameState.Instance.StartGame();
        }
        else
        {
            if (GameState.Instance.CurrentState != GameState.State.InGame)
                return;
        }

        BottlesManager.Instance.SelectBottle(this);
    }
    public bool CheckForComplete()
    {
        if (_ballsInBottle.Count == 0)
            return true;

        if (!IsFull)
            return false;

        BallType firstBallType = _ballsInBottle[0].Type;

        foreach (Ball ball in _ballsInBottle)
        {
            if (ball.Type != firstBallType)
                return false;
        }
        return true;
    }
}