using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private Vector2 _moveVector;
        private Rigidbody2D _rb;
        
        private InputAction _moveAction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveAction = GameInputSystem.Instance.InputActions.Player.Move;
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
    }
}