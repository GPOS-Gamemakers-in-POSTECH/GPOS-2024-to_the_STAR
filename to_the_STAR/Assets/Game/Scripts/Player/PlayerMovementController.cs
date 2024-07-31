using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;
using UnityEngine.Windows;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public Vector2 _moveVector;
        private Rigidbody2D _rb;

        private InputAction _moveAction;
        private InputAction _interactAction;

        public Lever _lever;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveAction = GameInputSystem.Instance.PlayerActions.Move;
            _interactAction = GameInputSystem.Instance.PlayerActions.Interact;
        }
        
        private void Update()
        {
            _moveVector = _moveAction.ReadValue<Vector2>();

            if (_interactAction.IsPressed() == true)
            {
                Interaction();
            } 
        }

        private void FixedUpdate()
        {
            Vector2 currPosition = transform.position;
            _rb.MovePosition(currPosition + _moveVector * Time.deltaTime);
        }

        private void Interaction()
        {
            if (_lever == null) return;

            _lever.Interaction();
        }

    }
}