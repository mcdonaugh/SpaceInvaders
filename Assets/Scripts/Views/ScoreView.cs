using UnityEngine;
using TMPro;
using SpaceInvaders.Controllers;

namespace SpaceInvaders.Views
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private GameStateController _gameStateController;
        [SerializeField] private TMP_Text _playerScore;

        private void Update()
        {
            OnScoreTextUpdate(_gameStateController.PlayerScore);
        }

        private void OnScoreTextUpdate(int playerScore)
        {
             _playerScore.text = $"{playerScore}";
        }


    }
}
