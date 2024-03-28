using System.Collections;
using SpaceInvaders.GameInput;
using SpaceInvaders.Views;
using UnityEngine;

namespace SpaceInvaders.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private UserInput _userInput;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private StartView _startView;
        [SerializeField] private GameView _gameView;
        [SerializeField] private GameEndView _gameEndView;
        [SerializeField] private InvadersController _invadersController;
        [SerializeField] private int _playerLives;
        [SerializeField] private int _playerScore;
        private int _highScore;
        private bool _gameIsActive;

        private void Awake()
        {
            _startView.gameObject.SetActive(true);
            _gameView.gameObject.SetActive(false);
            _invadersController.gameObject.SetActive(false);
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
            _scoreView.OnScoreTextUpdate(_playerScore); 
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
            _invadersController.gameObject.SetActive(true);
            _gameView.UpdateLivesText(_playerLives);
        }

        private void EndGame()
        {
            _gameEndView.gameObject.SetActive(true);
            CacheHighScore(_highScore);
            StartCoroutine(RestartGame());
        }

        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(3);
            _gameEndView.gameObject.SetActive(false);
            _startView.gameObject.SetActive(true);
            _invadersController.gameObject.SetActive(false);
            _playerScore = 0;
            UpdateScoreText();
        }

        private void CacheHighScore(int highScore)
        {
            if (highScore <= _playerScore)
            {
                _highScore = _playerScore;
                _scoreView.OnHighScoreTextUpdate(_highScore);
            }
  
        }
        
    }
}