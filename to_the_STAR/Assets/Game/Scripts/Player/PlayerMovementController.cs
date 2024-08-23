using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;
using UnityEngine.Windows;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private int moveSpeed = 3;
        [SerializeField] GameObject DashDetect;

        private InputActions.PlayerActions _playerActions;
        
        private Vector2 _moveVector;
        private Vector2 lastMove;
        private InputAction _moveAction;
        private InputAction _interactAction;
        private InputAction _dashAction;
        private Rigidbody2D _rb;
        private Animator _ani;

        private GameObject dashAble;

        private Weapon_Flamethrower _flameThrower;

        private float dash = 3.0f;
        private const float dashMax = 3.0f;
        private const float dashLength = 1.25f;

        private int dashCount = 0;

        public Vector2 getMoveVector()
        {
            return _moveVector;
        }
        public int getMoveSpeed()
        {
            return moveSpeed;
        }

        public float getDash()
        {
            return dash;
        }
        public void turn()
        {
            _ani.SetTrigger("TurningPoint");
        }

        public void damaged()
        {
            _ani.SetTrigger("Damaged");
        }

        public Lever _lever;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveAction = GameInputSystem.Instance.PlayerActions.Move;
            _interactAction = GameInputSystem.Instance.PlayerActions.Interact;
            _playerActions = GameInputSystem.Instance.PlayerActions;
            _dashAction = GameInputSystem.Instance.PlayerActions.Dash;
            _flameThrower = GetComponent<Weapon_Flamethrower>();
            _ani = GetComponent<Animator>();
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
            /*
            switch (rotateValue)
            {
                case > 0:
                    transform.Rotate(Vector3.forward, 90);
                    _flameThrower.DirVectorUpdate(90);
                    break;
                case < 0:
                    transform.Rotate(Vector3.forward, -90);
                    _flameThrower.DirVectorUpdate(-90);
                    break;
            }
            */
        }
        
        private void Update()
        {
            float angle = _rb.rotation * Mathf.Deg2Rad;
            Vector2 tmp = _moveAction.ReadValue<Vector2>();
            _moveVector = new Vector2(tmp.x * Mathf.Cos(angle) - tmp.y * Mathf.Sin(angle),
                                      tmp.x * Mathf.Sin(angle) + tmp.y * Mathf.Cos(angle));
            if (_moveVector.sqrMagnitude > 0)
            {
                GetComponent<SpriteRenderer>().flipX = (_moveVector.x + _moveVector.y > 0) ^ (transform.rotation.eulerAngles.z == 180 || transform.rotation.eulerAngles.z == 270);
                lastMove = _moveVector * dashLength / 10;
                _ani.SetBool("Move", true);
            }
            else
            {
                _ani.SetBool("Move", false);
            }
            if (_interactAction.triggered == true)
            {
                Interaction();
            }
            if (dashAble != null && dashCount < 0)
            {
                bool isEnable = dashAble.GetComponent<DashDetector>().isEnable();
                if (isEnable && !GetComponent<Weapon_Hammer>().isMouseInputted())
                {
                    transform.position = dashAble.transform.position;
                    dash -= 1;
                    _ani.SetTrigger("Dash");
                }
                Destroy(dashAble);
            }
            if (_dashAction.triggered == true && dash > 1)
            {
                dashAble = Instantiate(DashDetect, transform.position + new Vector3(lastMove.x, lastMove.y, 0), Quaternion.identity);
                dashAble.GetComponent<DashDetector>().setMove(new Vector2(lastMove.x, lastMove.y));
                dashCount = 10;
            }
            dashCount--;
        }

        private void FixedUpdate()
        {
            Vector2 currPosition = transform.position;
            int hammerRecoil = 1;
            if(GetComponent<PlayerData>().hammerCooldown() > 0.8f){
                hammerRecoil = 0;
            }
            _rb.MovePosition(currPosition + _moveVector * (moveSpeed * Time.deltaTime) 
                * (1 - Mathf.Max(GetComponent<PlayerData>().hammerCharge(), 0)) * (1 - Mathf.Max(GetComponent<PlayerData>().flamethrowerFever() / 3, 0)) * hammerRecoil);
            dash += Time.deltaTime;
            if (dash > dashMax) dash = dashMax;
        }

        private void Interaction()
        {
            if (_lever == null) return;

            _lever.Interaction();
        }

    }
}