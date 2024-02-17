using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour, IInitializable
{
    [SerializeField] private List<GameObject> _levelPrefabs = new List<GameObject>();
    public void Initialize()
    {
        BuildLevel();
    }
    private void BuildLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", 0) >= _levelPrefabs.Count)
            PlayerPrefs.SetInt("CurrentLevel", 0);
        Instantiate(_levelPrefabs[PlayerPrefs.GetInt("CurrentLevel", 0)]);
    }
}
