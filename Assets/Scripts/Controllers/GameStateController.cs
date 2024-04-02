using System.Collections;
using SpaceInvaders.GameInput;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;

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
        [SerializeField] private TMP_Text _playerScoreText;
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private GameObject _playerController;
        [SerializeField] private float _playerBounds;
        [SerializeField] private float _playerSpeed;
        [SerializeField] private GameObject _enemyController;
        [SerializeField] private float _enemyBounds;
        [SerializeField] private int _gridRows;
        [SerializeField] private int _gridCols;
        [SerializeField] private float _gridOffset;
        [SerializeField] private float _moveSpeed = .3f;
        [SerializeField] private float _moveInterval = .2f;
        [SerializeField] private float _spawnDelay = .025f;
        private GameObject[,] _gridArray;
        private int _playerScore;
        private int _highScore;
        private Vector3 _playerOrigin;
        private GameObject _topCenterEnemy;
        private float _directionX = 1;
        private float _directionY = 1;
        private int _currentRowIndex;


        private bool _gameIsActive;

        private void Awake()
        {
            _startView.gameObject.SetActive(true);
            _gameView.gameObject.SetActive(false);
            _gameEndView.gameObject.SetActive(false);
            _gameIsActive = false;
            _playerOrigin = new Vector3(-_playerBounds,-2f,0f);
            SpawnGrid();
            SpawnPlayer();
        }

        private void OnEnable()
        {
            _userInput.OnFirePressed += OnFireActionHandler;
            _userInput.OnLeftPressed += OnLeftActionHandler;
            _userInput.OnRightPressed += OnRightActionHandler;
        }

        private void StartGame()
        {
            _startView.gameObject.SetActive(false);
            _gameView.gameObject.SetActive(true);
            _gameEndView.gameObject.SetActive(false);
            StartCoroutine(ActivateGrid());
            // UpdateLivesText();
            // UpdateScoreText();
            ActivatePlayer();
        }

        private void EndGame()
        {
            _startView.gameObject.SetActive(false);
            _gameView.gameObject.SetActive(false);
            _gameEndView.gameObject.SetActive(true);
            ResetGrid();
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

        private void OnFireActionHandler()
        {
            ChangeGameState();
        }

        private void OnLeftActionHandler()
        {
            _playerController.transform.position -= new Vector3(1,0,0) * _playerSpeed * Time.deltaTime;
        }

        private void OnRightActionHandler()
        {
            _playerController.transform.position += new Vector3(1,0,0) * _playerSpeed * Time.deltaTime;
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

        private void SpawnGrid()
        {
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
                    newEnemy.SetActive(false);
                    _gridArray[i, j] = newEnemy;
                }
            }
            _topCenterEnemy = _gridArray[_gridRows - 1, _gridCols/2];
        }
        
        private void SpawnPlayer()
        {
            GameObject newPlayer = Instantiate(_playerController, _playerOrigin, Quaternion.identity);
            newPlayer.SetActive(false);
            _playerController = newPlayer;
        }

        private IEnumerator ActivateGrid()
        {
            foreach (var enemy in _gridArray)
            {
                yield return new WaitForSeconds(_spawnDelay);
                enemy.SetActive(true);
            }

            StartCoroutine(MoveTimer());
        }

        private void ActivatePlayer()
        {
           _playerController.SetActive(true); 
        }

        private void DeActivatePlayer()
        {
            _playerController.SetActive(false);
            _playerController.transform.position = _playerOrigin; 
        }

        private void ResetGrid()
        {
            foreach (var enemy in _gridArray)
            {
                enemy.SetActive(false);
            }
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

            if (_topCenterEnemy.transform.position.x > _enemyBounds)
            {
                MoveY();
                _directionX = -1;
            }    
            else if (_topCenterEnemy.transform.position.x < -_enemyBounds)
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
            }
        }

        private IEnumerator MoveTimer()
        {
            while(true)
            {
                yield return new WaitForSeconds(_moveInterval);
                MoveX();
            }
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