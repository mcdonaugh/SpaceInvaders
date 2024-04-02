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
        [SerializeField] private TMP_Text _playerScoreText;
        [SerializeField] private TMP_Text _playerHighScoreText;
        [SerializeField] private GameObject _playerController;
        [SerializeField] private float _playerBounds;
        [SerializeField] private float _playerSpeed;
        [SerializeField] private GameObject _invaderController;
        [SerializeField] private float _invaderBounds = .65f;
        [SerializeField] private int _invaderGridRows = 5;
        [SerializeField] private int _invaderGridCols = 8;
        [SerializeField] private float _invaderGridOffset = .3f;
        [SerializeField] private float _invaderMoveSpeed = .065f;
        [SerializeField] private float _invaderMoveInterval = .2f;
        [SerializeField] private float _invaderSpawnDelay = .025f;
        private GameObject[,] _invaderGridArray;
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
            _gameIsActive = true;

            UpdateLivesText();
            UpdateScoreText();
            StartCoroutine(ActivateGrid());
            ActivatePlayer();
        }

        private void EndGame()
        {
            _gameIsActive = false;
            _startView.gameObject.SetActive(false);
            _gameView.gameObject.SetActive(false);
            _gameEndView.gameObject.SetActive(true);
            ResetPlayer();
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
            _invaderGridArray = new GameObject[_invaderGridRows, _invaderGridCols];

            float rowWidth = (_invaderGridCols - 1) * _invaderGridOffset;
            rowWidth /= 2;

            float rowHeight = (_invaderGridRows - 1) * _invaderGridOffset;
            rowHeight /= 2;

            for (int i = 0; i < _invaderGridRows; i++)
            {
                for (int j = 0; j < _invaderGridCols; j++)
                {
                    GameObject newEnemy = Instantiate(_invaderController, new Vector3(-rowWidth, -rowHeight, 0) + new Vector3(j * _invaderGridOffset, i * _invaderGridOffset, 0), Quaternion.identity);
                    newEnemy.SetActive(false);
                    _invaderGridArray[i, j] = newEnemy;
                }
            }
            _topCenterEnemy = _invaderGridArray[_invaderGridRows - 1, _invaderGridCols/2];
        }
        
        private void SpawnPlayer()
        {
            GameObject newPlayer = Instantiate(_playerController, _playerOrigin, Quaternion.identity);
            newPlayer.SetActive(false);
            _playerController = newPlayer;
        }

        private IEnumerator ActivateGrid()
        {
            foreach (var enemy in _invaderGridArray)
            {
                yield return new WaitForSeconds(_invaderSpawnDelay);
                enemy.SetActive(true);
            }

            StartCoroutine(MoveTimer());
        }

        private void ActivatePlayer()
        {
           _playerController.SetActive(true); 
        }

        private void ResetPlayer()
        {
            _playerController.SetActive(false);
            _playerController.transform.position = _playerOrigin; 
        }

        private void ResetGrid()
        {
            foreach (var enemy in _invaderGridArray)
            {
                enemy.SetActive(false);
            }

            StopCoroutine(MoveTimer());
        }

        private void MoveX()
        {
            
            for (int i = 0; i < _invaderGridCols; i++)
            {
                _invaderGridArray[_currentRowIndex,i].transform.position += new Vector3(_directionX, 0 ,0) * _invaderMoveSpeed;
            }

            _currentRowIndex++;

            if (_currentRowIndex == _invaderGridRows)
            {
                _currentRowIndex = 0;
            }

            if (_topCenterEnemy.transform.position.x > _invaderBounds)
            {
                MoveY();
                _directionX = -1;
            }    
            else if (_topCenterEnemy.transform.position.x < -_invaderBounds)
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
                for (int i = 0; i < _invaderGridCols; i++)
                {
                    _invaderGridArray[_currentRowIndex,i].transform.position += new Vector3(0, -_directionY ,0) * _invaderMoveSpeed;
                }

                moveDown = false;
            }
        }

        private IEnumerator MoveTimer()
        {
            while(_gameIsActive)
            {
                yield return new WaitForSeconds(_invaderMoveInterval);
                MoveX();
                Debug.Log("tick");
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
                _playerHighScoreText.text = $"{_highScore}";
            }
  
        }
        
    }
}