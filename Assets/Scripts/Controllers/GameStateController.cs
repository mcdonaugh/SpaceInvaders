using System.Collections;
using SpaceInvaders.GameInput;
using UnityEngine;
using TMPro;

namespace SpaceInvaders.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private UserInput _userInput;
        [SerializeField] private GameObject _scoreView;
        [SerializeField] private GameObject _startView;
        [SerializeField] private GameObject _gameView;
        [SerializeField] private GameObject _gameEndView;
        [SerializeField] private int _playerLives;
        [SerializeField] private TMP_Text _playerLivesText;
        [SerializeField] private int _playerScore;
        [SerializeField] private TMP_Text _playerScoreText;
        private int _highScore;
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private GameObject _enemyController;
        [SerializeField] private int _gridRows;
        [SerializeField] private int _gridCols;
        [SerializeField] private float _gridOffset;
        [SerializeField] private float _enemyBoundOffset = 3;
        [SerializeField] private float _moveSpeed = .3f;
        [SerializeField] private float _moveInterval = .2f;
        private GameObject _topCenterEnemy;
        private float _directionX = 1;
        private float _directionY = 1;
        private float _rowsMoved = 1;
        private int _currentRowIndex;
        private GameObject[,] _gridArray;


        private bool _gameIsActive;

        private void Awake()
        {
            _startView.gameObject.SetActive(true);
            _gameView.gameObject.SetActive(false);
            _gameEndView.gameObject.SetActive(false);
            _gameIsActive = false;
        }

        private void OnEnable()
        {
            _userInput.OnFirePressed += OnFireActionHandler;
            _userInput.OnLeftPressed += OnLeftActionHandler;
            _userInput.OnRightPressed += OnRightActionHandler;
        }

        private void OnFireActionHandler()
        {
            ChangeGameState();
        }

        private void OnLeftActionHandler()
        {
            Debug.Log("Left");
        }

        private void OnRightActionHandler()
        {
            Debug.Log("Right");
        }

        private void UpdateScoreText()
        {
            _playerScoreText.text = $"{_playerScore}";
        }

        private void ChangeGameState()
        {
            if(!_gameIsActive)
            {
                StartGame();
                _gameIsActive = true;
            }
            else if (_gameIsActive)
            {
                EndGame();
                _gameIsActive = false;
            }
        }

        private void StartGame()
        {
            _startView.gameObject.SetActive(false);
            _gameView.gameObject.SetActive(true);
            _gameEndView.gameObject.SetActive(false);
            UpdateLivesText();
            UpdateScoreText();

             _gridArray = new GameObject[_gridRows, _gridCols];
            float rowWidth = (_gridCols - 1) * _gridOffset;
            rowWidth /= 2;

            float rowHeight = (_gridRows - 1) * _gridOffset;
            rowHeight /= 2;

            for (int i = 0; i < _gridRows; i++)
            {
                for (int j = 0; j < _gridCols; j++)
                {
                    GameObject newEnemy = Instantiate(_enemyController, new Vector3(-rowWidth, -rowHeight,0) + new Vector3(j * _gridOffset, i * _gridOffset, 0), Quaternion.identity);
                    _gridArray[i, j] = newEnemy;
                }
            }

            _topCenterEnemy = _gridArray[_gridRows - 1, _gridCols/2];
            StartCoroutine(MoveTimer(_moveInterval));
        }

        private void MoveX()
        {
            
            for (int i = 0; i < _gridCols; i++)
            {
                _gridArray[_currentRowIndex,i].transform.position += new Vector3(_directionX, 0 ,0) * _moveSpeed;
            }

            _currentRowIndex++;

            if (_currentRowIndex == _gridRows)
            {
                _currentRowIndex = 0;
            }

            if (_topCenterEnemy.transform.position.x > _enemyBoundOffset)
            {
                MoveY();
                _directionX = -1;
            }    
            else if (_topCenterEnemy.transform.position.x < -_enemyBoundOffset)
            {
                MoveY();
                _directionX = 1;  
            }        
        }

        private void MoveY()
        {
            bool moveDown = true;

            if(moveDown == true)
            {
                for (int i = 0; i < _gridCols; i++)
                {
                    _gridArray[_currentRowIndex,i].transform.position += new Vector3(0, -_directionY ,0) * _moveSpeed;
                }
                moveDown = false;
                _rowsMoved++;
            }
        }

        private IEnumerator MoveTimer(float interval)
        {
            while(true)
            {
                yield return new WaitForSeconds(interval);
                MoveX();
            }
        }

        private void EndGame()
        {
            _startView.gameObject.SetActive(false);
            _gameView.gameObject.SetActive(false);
            _gameEndView.gameObject.SetActive(true);
            UpdateScoreText();
            CacheHighScore();
            StartCoroutine(RestartGame());
        }

        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(3);
            _startView.gameObject.SetActive(true);
            _gameView.gameObject.SetActive(false);
            _gameEndView.gameObject.SetActive(false);
            _playerScore = 0;
            UpdateScoreText();
        }

        

        public void UpdateLivesText()
        {
            _playerLivesText.text = $"{_playerLives}";
        }


        private void CacheHighScore()
        {
            if (_playerScore >= _highScore)
            {
                _highScore = _playerScore;
                _highScoreText.text = $"{_highScore}";
            }
  
        }
        
    }
}