using SpaceInvaders.Views;
using UnityEngine;

namespace SpaceInvaders.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private ScoreView _scoreView;
        private int _playerScore;

        private void Awake()
        {
            _playerScore = 0;
        }
        private void Update()
        {
            IncreaseScore();
        }

        private void IncreaseScore()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _playerScore++;
                Debug.Log($"{_playerScore}");
            }
        }
        
    }
}
