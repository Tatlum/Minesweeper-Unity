using Minesweeper.Core;
using Minesweeper.View;
using UnityEngine;

namespace Minesweeper.Presenters
{
    public class BoardPresenter : MonoBehaviour
    {
        [SerializeField] private CellView _cellPrefab;
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private float _cellSpacing = 1.0f;
        
        public Vector2 BoardScreenSize { get; private set; }
        
        private BoardModel _boardModel;
        private MinesweeperGame _minesweeperGame;
        
        private CellView[,] _cellViews;
        private bool _isSubscribed;

        public void Initialize(BoardModel boardModel, MinesweeperGame minesweeperGame)
        {
            _boardModel = boardModel;
            _minesweeperGame = minesweeperGame;

            InitializeViews();
            RefreshAllViews();
            Subscribe();
        }

        private void InitializeViews()
        {
            BoardScreenSize = new Vector2(
                (_boardModel.Width - 1) * _cellSpacing, 
                (_boardModel.Height - 1) * _cellSpacing);
            var offset = new Vector3(BoardScreenSize.x / 2f, BoardScreenSize.y / 2f, 0);

            _cellViews = new CellView[_boardModel.Width, _boardModel.Height];

            for (int x = 0; x < _boardModel.Width; x++)
            {
                for (int y = 0; y < _boardModel.Height; y++)
                {
                    var spawnPos = new Vector3(x * _cellSpacing, y * _cellSpacing, 0) - offset;
                    var cellView = Instantiate(_cellPrefab, spawnPos, Quaternion.identity, _gridContainer);
                    
                    cellView.Initialize(x, y);
                    _cellViews[x, y] = cellView;
                }
            }
        }

        private void RefreshAllViews()
        {
            foreach (var view in _cellViews)
            {
                RefreshViewAt(view.X, view.Y);
            }
        }

        private void RefreshViewAt(int x, int y)
        {
            var view = _cellViews[x, y];
            var cellModel = _boardModel.GetCell(x, y);
            
            view?.Draw(
                cellModel.IsRevealed, 
                cellModel.IsFlagged, 
                cellModel.IsMine, 
                cellModel.AdjacentMines);
        }

        private void Subscribe()
        {
            if (_isSubscribed)
            {
                return;
            }
            
            _isSubscribed = true;
            
            _minesweeperGame.OnBoardChanged += RefreshAllViews;
            
            int width = _cellViews.GetLength(0);
            int height = _cellViews.GetLength(1);
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var view = _cellViews[x, y];

                    if (view == null)
                    {
                        continue;
                    }

                    view.OnLeftClicked += _minesweeperGame.RevealCell;
                    view.OnRightClicked += _minesweeperGame.ToggleFlag;
                }
            }
        }

        private void Unsubscribe()
        {
            _minesweeperGame.OnBoardChanged -= RefreshAllViews;
            
            int width = _cellViews.GetLength(0);
            int height = _cellViews.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var view = _cellViews[x, y];

                    if (view == null)
                    {
                        continue;
                    }

                    view.OnLeftClicked -= _minesweeperGame.RevealCell;
                    view.OnRightClicked -= _minesweeperGame.ToggleFlag;
                }
            }
            
            _isSubscribed = false;
        }
        
        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}
