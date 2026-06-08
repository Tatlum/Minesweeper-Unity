using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Minesweeper.GameFlow;
using System;

namespace Minesweeper.View
{
    public class GameUIView : MonoBehaviour
    {
        [Header("Windows/Panels")]
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _gamePanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _gameOverPanel;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _winResultText;
        [SerializeField] private TextMeshProUGUI _loseResultText;

        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button[] _restartButtons;
        [SerializeField] private Button[] _mainMenuButtons;

        public void TogglePanel(GameState state)
        {
            _menuPanel.SetActive(state == GameState.Menu);
            _gamePanel.SetActive(state is GameState.Playing or GameState.Paused or GameState.GameOver);
            _pausePanel.SetActive(state == GameState.Paused);
            _gameOverPanel.SetActive(state == GameState.GameOver);
        }

        public void UpdateTimerText(int seconds)
        {
            _timerText.text = seconds.ToString("D3"); // Формат 001, 015, 120
        }

        public void ShowWinResult()
        {
            _winResultText.gameObject.SetActive(true);
            _loseResultText.gameObject.SetActive(false);
        }

        public void ShowLoseResult()
        {
            _winResultText.gameObject.SetActive(false);
            _loseResultText.gameObject.SetActive(true);
        }
        
        public void BindStart(Action onStart) => _startButton.onClick.AddListener(onStart.Invoke);
        public void BindPause(Action onPause) => _pauseButton.onClick.AddListener(onPause.Invoke);
        public void BindResume(Action onResume) => _resumeButton.onClick.AddListener(onResume.Invoke);
        
        public void BindRestart(Action onRestart)
        {
            foreach (var button in _restartButtons)
            {
                button.onClick.AddListener(onRestart.Invoke);
            }
        }
        
        public void BindMainMenu(Action onMainMenu)
        {
            foreach (var button in _mainMenuButtons)
            {
                button.onClick.AddListener(onMainMenu.Invoke);
            }
        }
        
        public void UnbindAll()
        {
            _startButton?.onClick.RemoveAllListeners();
            _pauseButton?.onClick.RemoveAllListeners();
            _resumeButton?.onClick.RemoveAllListeners();

            UnbindArray(_restartButtons);
            UnbindArray(_mainMenuButtons);
        }

        private void UnbindArray(Button[] buttons)
        {
            if (buttons == null)
            {
                return;
            }

            foreach (var btn in buttons)
            {
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                }
            }
        }
    }
}
