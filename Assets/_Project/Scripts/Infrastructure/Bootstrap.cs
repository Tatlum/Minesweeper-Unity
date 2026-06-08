using Minesweeper.Core;
using Minesweeper.GameFlow;
using Minesweeper.Presenters;
using Minesweeper.View;
using UnityEngine;
using Random = System.Random;

namespace Minesweeper.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;
        [SerializeField] private BoardPresenter _boardPresenter;
        [SerializeField] private GameUIPresenter _gameUIPresenter;
        [SerializeField] private GameUIView _uiView;
        
        [SerializeField] private Camera _gameCamera;

        private GameStateManager _stateManager;
        private MinesweeperGame _minesweeperGame;

        private void Start()
        {
            var mineGenerationService = new MineGenerationService(new Random());
            var boardTraversalService = new BoardTraversalService();
            
            var boardModel = new BoardModel(_config.Width, _config.Height, _config.MineCount);
            
            _minesweeperGame = new MinesweeperGame(boardModel, mineGenerationService, boardTraversalService);
            _stateManager = new GameStateManager();
            
            _boardPresenter.Initialize(boardModel, _minesweeperGame);
            _gameUIPresenter.Initialize(_minesweeperGame, _stateManager, _uiView);
            
            var cameraAdjuster = new CameraAdjuster();
            cameraAdjuster.AdjustByBoardSize(_gameCamera, _boardPresenter.BoardScreenSize);
            
            _minesweeperGame.OnGameWon += FinishGame;
            _minesweeperGame.OnGameLost += FinishGame;
        }
        
        private void FinishGame()
        {
            _stateManager.ChangeState(GameState.GameOver);
        }

        private void OnDestroy()
        {
            _minesweeperGame.OnGameWon -= FinishGame;
            _minesweeperGame.OnGameLost -= FinishGame;
        }
    }
}
