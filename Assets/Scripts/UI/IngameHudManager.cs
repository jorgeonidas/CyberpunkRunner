using System;
using UnityEngine;

public class IngameHudManager : MonoBehaviour
{
    [SerializeField] GameObject _restartScreen;

    private void Start()
    {
        ShowGameOverScreen(false);
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    void OnEnable()
    {
       
    }

    void OnDisable()
    {
       GameManager.Instance.OnGameOver -= GameManager_OnGameOver; 
    }

    private void GameManager_OnGameOver()
    {
        ShowGameOverScreen(true);
    }

    public void ShowGameOverScreen(bool show)
    {
        _restartScreen.SetActive(show);
    }
}
