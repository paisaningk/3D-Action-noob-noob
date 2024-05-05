using UnityEngine;

namespace CharacterScript.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 directionMove;
        public bool isMousePressed;
        private Input input;
        private Input.PlayerActions playerInput;

        
        
        public void Start()
        {
            input = new Input();
            input.Enable();

            playerInput = input.Player;
        }

        public void Update()
        {
            isMousePressed = playerInput.Attack.IsPressed();
            directionMove = playerInput.Move.ReadValue<Vector2>();
        }
    }
}