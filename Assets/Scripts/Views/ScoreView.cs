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


        public void OnScoreTextUpdate(int playerScore)
        {
             _playerScore.text = $"{playerScore}";
        }

        public void OnHighScoreTextUpdate(int highScore)
        {
            _highScore.text = $"{highScore}";
        }
    }
}
