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

        public Lever lever;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveAction = GameInputSystem.Instance.PlayerActions.Move;
        }
        
        private void Update()
        {
            _moveVector = _moveAction.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            Vector2 currPosition = transform.position;
            _rb.MovePosition(currPosition + _moveVector * Time.deltaTime);
        }

        private void Interaction()
        {
            if (lever == null) return;

            lever.Interaction();
        }
    }
}