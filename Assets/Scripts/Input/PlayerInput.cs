using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterScript.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 directionMove;
        public bool isMousePressed;
        public bool isSpaceKeyPressed;
        private Input input;
        private Input.PlayerActions playerInput;

        public void Start()
        {
            input = new Input();
            input.Enable();

            playerInput = input.Player;
            playerInput.Pause.performed += PauseOnPerformed;
        }

        public void Update()
        {
            isMousePressed = playerInput.Attack.IsPressed();
            directionMove = playerInput.Move.ReadValue<Vector2>();
            isSpaceKeyPressed = playerInput.Slide.IsPressed();
        }

        public void OnDestroy()
        {
            playerInput.Pause.performed -= PauseOnPerformed;
        }


        private void PauseOnPerformed(InputAction.CallbackContext obj)
        {
            MainUIManager.Instance.TogglePauseUI();
        }
    }
}