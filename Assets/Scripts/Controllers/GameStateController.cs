using System.Collections;
using SpaceInvaders.Views;
using UnityEngine;

namespace SpaceInvaders.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private StartView _startView;
        [SerializeField] private GameView _gameView;
        [SerializeField] private GameEndView _gameEndView;
        [SerializeField] private int _playerLives;
        [SerializeField] private int _playerScore;
        private int _highScore;
        private bool _gameIsActive;

        

        private void Awake()
        {
            _startView.gameObject.SetActive(true);
            _gameView.gameObject.SetActive(false);
            _gameEndView.gameObject.SetActive(false);
            _gameIsActive = false;
        }

        private void Update()
        {
            ChangeGameState();
            UpdateScore();
        }

        private void UpdateScore()
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                _playerScore++;
                _scoreView.OnScoreTextUpdate(_playerScore); 
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                _playerScore--;
                _scoreView.OnScoreTextUpdate(_playerScore); 
            }
        }
        private void ChangeGameState()
        {
            if(Input.GetKeyDown(KeyCode.Space))
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
        }

        private void StartGame()
        {
            
                _startView.gameObject.SetActive(false);
                _gameView.gameObject.SetActive(true);
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
            _playerScore = 0;
            UpdateScore();
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
