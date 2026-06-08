using Minesweeper.Core;
using Minesweeper.GameFlow;
using Minesweeper.View;
using UnityEngine;

namespace Minesweeper.Presenters
{
    public class GameUIPresenter : MonoBehaviour
    {
        private MinesweeperGame _game;
        private GameStateManager _stateManager;
        private GameUIView _view;
        
        private GameState _state;
        private float _time;

        public void Initialize(MinesweeperGame game, GameStateManager stateManager, GameUIView view)
        {
            _game = game;
            _view = view;
            _stateManager = stateManager;

            _stateManager.OnStateChanged += OnGameStateChanged;
            _game.OnGameWon += _view.ShowWinResult;
            _game.OnGameLost += _view.ShowLoseResult;

            BindButtons();
            OnGameStateChanged(_stateManager.CurrentState);
        }

        private void BindButtons()
        {
            _view.BindStart(StartNewGame);
            _view.BindRestart(StartNewGame);

            _view.BindPause(() => _stateManager.ChangeState(GameState.Paused));
            _view.BindResume(() => _stateManager.ChangeState(GameState.Playing));
            _view.BindMainMenu(() => _stateManager.ChangeState(GameState.Menu));
        }

        private void StartNewGame()
        {
            _game.RestartGame();
            _stateManager.ChangeState(GameState.Playing);
        }

        private void OnGameStateChanged(GameState state)
        {
            _state = state;
            _view.TogglePanel(state);
        }

        private void Update()
        {
            ProcessRestartInput();
            UpdateTimer();
        }

        private void ProcessRestartInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartNewGame();
            }
        }

        private void UpdateTimer()
        {
            if (_state != GameState.Playing)
                return;

            _time = _game.IsFirstMoveDone ? _time + Time.deltaTime : 0;
            _view.UpdateTimerText(Mathf.FloorToInt(_time));
        }
        
        private void OnDestroy()
        {
            if (_stateManager != null)
            {
                _stateManager.OnStateChanged -= OnGameStateChanged;
            }

            if (_game != null && _view != null)
            {
                _game.OnGameWon -= _view.ShowWinResult;
                _game.OnGameLost -= _view.ShowLoseResult;
            }

            if (_view != null)
            {
                _view.UnbindAll();
            }
        }
    }
}
