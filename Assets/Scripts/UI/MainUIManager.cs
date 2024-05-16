using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public enum UIState
    {
        GamePlay,
        Pause,
        GameOver,
        GameFinished
    }

    public class MainUIManager : Singleton<MainUIManager>
    {
        public GameManager gameManager;
        public TMP_Text coinText;
        public Slider healthSlider;

        public UIState currentState;

        [Header("UI Panel")]
        public GameObject uiPause;
        public GameObject uiGameOver;
        public GameObject uiGameFinished;

        private void Start()
        {
            SwitchUIState(UIState.GamePlay);
        }

        public void Update()
        {
            healthSlider.value = gameManager.playerCharacter.health.CurrentHealthPercentage;
            coinText.SetText(gameManager.playerCharacter.coin.ToString());
        }

        public void TogglePauseUI()
        {
            if (currentState == UIState.GamePlay)
            {
                SwitchUIState(UIState.Pause);
            }
            else if (currentState == UIState.Pause)
            {
                SwitchUIState(UIState.GamePlay);
            }
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void GameOver()
        {
            SwitchUIState(UIState.GameOver);
        }

        public void GameFinished()
        {
            SwitchUIState(UIState.GameFinished);
        }

        public void SwitchUIState(UIState state)
        {
            uiPause.SetActive(false);
            uiGameOver.SetActive(false);
            uiGameFinished.SetActive(false);

            Time.timeScale = 1;

            switch (state)
            {
                case UIState.GamePlay:
                    break;
                case UIState.Pause:
                    Time.timeScale = 0;
                    uiPause.SetActive(true);
                    break;
                case UIState.GameOver:
                    uiGameOver.SetActive(true);
                    Time.timeScale = 0;
                    break;
                case UIState.GameFinished:
                    uiGameFinished.SetActive(true);
                    Time.timeScale = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            currentState = state;
        }
    }
}