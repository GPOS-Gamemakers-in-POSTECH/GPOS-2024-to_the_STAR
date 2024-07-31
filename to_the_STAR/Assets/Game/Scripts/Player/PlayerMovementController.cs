using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private int moveSpeed = 5;

        private InputActions.PlayerActions _playerActions;
        
        private Vector2 _moveVector;
        private InputAction _moveAction;
        
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerActions = GameInputSystem.Instance.PlayerActions;
        }

        private void OnEnable()
        {
            _playerActions.Rotate.performed += OnRotatePerformed;
        }
        
        private void OnDisable()
        {
            _playerActions.Rotate.performed -= OnRotatePerformed;
        }
        
        private void Start()
        {
            _moveAction = _playerActions.Move;
        }
        
        private void OnRotatePerformed(InputAction.CallbackContext context)
        {
            var rotateValue = context.ReadValue<float>();

            switch (rotateValue)
            {
                case > 0:
                    transform.Rotate(Vector3.forward, 90);
                    break;
                case < 0:
                    transform.Rotate(Vector3.forward, -90);
                    break;
            }
        }
        
        private void Update()
        {
            _moveVector = _moveAction.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            Vector2 currPosition = transform.position;
            _rb.MovePosition(currPosition + _moveVector * (moveSpeed * Time.deltaTime));
        }
    }
}