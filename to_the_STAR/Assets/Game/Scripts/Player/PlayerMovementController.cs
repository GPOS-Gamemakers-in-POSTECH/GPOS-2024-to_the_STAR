using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;
using UnityEngine.Windows;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private int moveSpeed = 5;

        private InputActions.PlayerActions _playerActions;
        
        private Vector2 _moveVector;
        private InputAction _moveAction;
        private InputAction _interactAction;
        private Rigidbody2D _rb;

        public Vector2 getMoveVector()
        {
            return _moveVector;
        }
        public int getMoveSpeed()
        {
            return moveSpeed;
        }

        public Lever _lever;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveAction = GameInputSystem.Instance.PlayerActions.Move;
            _interactAction = GameInputSystem.Instance.PlayerActions.Interact;
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

            if (_interactAction.IsPressed() == true)
            {
                Interaction();
            } 
        }

        private void FixedUpdate()
        {
            Vector2 currPosition = transform.position;
            _rb.MovePosition(currPosition + _moveVector * (moveSpeed * Time.deltaTime));
        }

        private void Interaction()
        {
            if (_lever == null) return;

            _lever.Interaction();
        }

    }
}