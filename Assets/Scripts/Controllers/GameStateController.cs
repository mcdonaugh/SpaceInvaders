using SpaceInvaders.Views;
using UnityEngine;

namespace SpaceInvaders.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        public int PlayerScore => _playerScore;
        public int PlayerLives => _playerLives;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private GameView _gameView;
        private int _playerScore;
        private int _playerLives;

        private void Awake()
        {
            _gameView.gameObject.SetActive(false);
        }

        private void Update()
        {
            StartGame();
            ChangeScore();
        }

        private void ChangeScore()
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                _playerScore++;
            }
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                _playerScore--;
            }
        }

        private void StartGame()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _gameView.gameObject.SetActive(true);
            }
        }
        
    }
}
