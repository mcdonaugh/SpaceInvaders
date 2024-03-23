using UnityEngine;
using TMPro;
using SpaceInvaders.Controllers;

namespace SpaceInvaders.Views
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private GameStateController _gameStateController;
        [SerializeField] private TMP_Text _playerLives;

        public void UpdateLivesText(int lives)
        {
            _playerLives.text = $"{lives}";
        }

    }
}