using UnityEngine;

namespace CharacterScript.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 directionMove;
        private Input input;

        public void Start()
        {
            input = new Input();
            input.Enable();
        }

        public void Update()
        {
            directionMove = input.Player.Move.ReadValue<Vector2>();
        }
    }
}