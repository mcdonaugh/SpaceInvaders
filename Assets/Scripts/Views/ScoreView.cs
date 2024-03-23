using UnityEngine;
using TMPro;
using SpaceInvaders.Controllers;

namespace SpaceInvaders.Views
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private GameStateController _gameStateController;
        [SerializeField] private TMP_Text _playerScore;
        [SerializeField] private TMP_Text _highScore;

        private void Update()
        {
            OnScoreTextUpdate(_gameStateController.PlayerScore);
            OnHighScoreTextUpdate(_gameStateController.HighScore);
        }

        private void OnScoreTextUpdate(int playerScore)
        {
             _playerScore.text = $"{playerScore}";
        }

        private void OnHighScoreTextUpdate(int highScore)
        {
            _highScore.text = $"{highScore}";
        }


    }
}
