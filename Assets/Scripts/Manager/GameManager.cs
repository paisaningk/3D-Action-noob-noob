using CharacterComponent;
using CharacterComponent.Player;
using UI;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public PlayerCharacter playerCharacter;
        public bool isGameOver;

        public void Update()
        {
            if (isGameOver)
            {
                return;
            }

            if (playerCharacter.currentState != CharacterState.Dead) return;

            isGameOver = true;
            GameOver();
        }

        public void OnValidate()
        {
            if (!playerCharacter)
            {
                playerCharacter = FindObjectOfType<PlayerCharacter>();
            }
        }

        public void GameOver()
        {
            MainUIManager.Instance.GameOver();
            Debug.Log("Game over");
        }

        public void GameIsFinished()
        {
            MainUIManager.Instance.GameFinished();
            Debug.Log("Game Is Finished");
        }
    }
}