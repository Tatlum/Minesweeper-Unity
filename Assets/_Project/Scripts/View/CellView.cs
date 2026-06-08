using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minesweeper.View
{
    [RequireComponent(typeof(Collider2D))]
    public class CellView : MonoBehaviour
    {
        public event Action<int, int> OnLeftClicked;
        public event Action<int, int> OnRightClicked;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private Sprite _closedSprite;
        [SerializeField] private Sprite _flagSprite;
        [SerializeField] private Sprite _mineSprite;
        
        [Tooltip("Спрайты для количества мин от 0 до 8.")]
        [SerializeField] private Sprite[] _numberSprites;

        public int X { get; private set; }
        public int Y { get; private set; }
        
        public void Initialize(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Draw(
            bool isRevealed, 
            bool isFlagged, 
            bool isMine, 
            int adjacentMines)
        {
            if (!isRevealed)
            {
                _spriteRenderer.sprite = isFlagged ? _flagSprite : _closedSprite;
                return;
            }

            if (isMine)
            {
                _spriteRenderer.sprite = _mineSprite;
                return;
            }

            if (adjacentMines >= 0 && adjacentMines < _numberSprites.Length)
            {
                _spriteRenderer.sprite = _numberSprites[adjacentMines];
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return; 
                }
                
                OnLeftClicked?.Invoke(X, Y);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return; 
                }
                
                OnRightClicked?.Invoke(X, Y);
            }
        }
    }
}
