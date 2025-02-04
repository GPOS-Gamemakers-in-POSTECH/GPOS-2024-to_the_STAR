using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;
using UnityEngine.Windows;
using System.Collections;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float speed = 5;
        [SerializeField] GameObject DashDetect;
        [SerializeField] AudioClip playerHitSound;
        [SerializeField] AudioClip playerDashSound;
        [SerializeField] AudioClip itemObtainSound;

        private InputActions.PlayerActions _playerActions;
        
        private Vector2 _moveVector;
        private Vector2 lastMove;
        private InputAction _moveAction;
        private InputAction _interactAction;
        private InputAction _dashAction;
        private InputAction _sprintAction;
        private InputAction _weaponChange;
        private InputAction _selectHammer;
        private InputAction _selectFlamethrower;
        private Rigidbody2D _rb;
        private Animator _ani;

        private GameObject dashAble;

        private WeaponAdministrator _wa;

        private float stamina = 3.0f;
        private const float staminaMax = 3.0f;
        private const float dashLength = 1.25f;

        private const float sprintStamina = 0.7f;
        private const float sprintSpeed = 1.5f;
        private const float moveSpeed = 5;

        private int dashCount = 0;
        private int dashCooldownCounter = 0;
        private const int dashCooldownSet = 4;

        private bool usingStamina = false;
        private bool staminaCool = false;

        public Vector2 GetMoveVector()
        {
            return _moveVector;
        }
        public float getMoveSpeed()
        {
            return speed;
        }

        public float GetStamina()
        {
            return stamina;
        }

        public void SetStamina(float value)
        {
            stamina = value > 0 ? value : 0;
        }

        public void SetStaminaCool(bool tf)
        {
            staminaCool = tf;
        }

        public bool IsStaminaCool()
        {
            return staminaCool;
        }

        public void turn()
        {
            _ani.SetTrigger("TurningPoint");
        }

        public void damaged()
        {
            GetComponent<AudioSource>().PlayOneShot(playerHitSound, 1.0f);
            _ani.SetTrigger("Damaged");
        }

        public Lever _lever;
        public Item _item;

        IEnumerator invincible()
        {
            GetComponent<PlayerData>().setInvincibility(true);
            yield return new WaitForSeconds(0.6f);
            GetComponent<PlayerData>().setInvincibility(false);
        }
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveAction = GameInputSystem.Instance.PlayerActions.Move;
            _interactAction = GameInputSystem.Instance.PlayerActions.Interact;
            _playerActions = GameInputSystem.Instance.PlayerActions;
            _dashAction = GameInputSystem.Instance.PlayerActions.Dash;
            _sprintAction = GameInputSystem.Instance.PlayerActions.Sprint;
            _weaponChange = GameInputSystem.Instance.PlayerActions.WeaponChange;
            _selectHammer = GameInputSystem.Instance.PlayerActions.SelectHammer;
            _selectFlamethrower = GameInputSystem.Instance.PlayerActions.SelectFlamethrower;
            _wa = GetComponent<WeaponAdministrator>();
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

            if(GamePause.isGamePaused) return; // If the game is Paused

            if (_interactAction.triggered == true)
            {
                Interaction();
            }

            if(_selectHammer.triggered == true)
            {
                _wa.SelectHammer();
            }

            if (_selectFlamethrower.triggered == true)
            {
                _wa.SelectFlamethrower();
            }

            if (dashAble != null)
            {
                bool isEnable = dashAble.GetComponent<DashDetector>().isEnable();
                if (dashCount < 0)
                {
                    if (isEnable && !GetComponent<Weapon_Hammer>().isMouseInputted())
                    {
                        GetComponent<AudioSource>().PlayOneShot(playerDashSound, 1.0f);
                        transform.position = dashAble.transform.position;
                        stamina -= 1;
                        _ani.SetTrigger("Dash");
                        StartCoroutine(invincible());
                    }
                    Destroy(dashAble);
                }
                else if(dashCount == 4 && !isEnable)
                {
                    Destroy(dashAble);
                }
                else if (!isEnable)
                {
                    if (!GetComponent<Weapon_Hammer>().isMouseInputted())
                    {
                        GetComponent<AudioSource>().PlayOneShot(playerDashSound, 1.0f);
                        transform.position = dashAble.transform.position;
                        stamina -= 1;
                        _ani.SetTrigger("Dash");
                        StartCoroutine(invincible());
                    }
                    Destroy(dashAble);
                }

            }

            if (_weaponChange.ReadValue<float>() != 0) _wa.WeaponChange();

            float angle = _rb.rotation * Mathf.Deg2Rad;
            Vector2 tmp = _moveAction.ReadValue<Vector2>();
            if(tmp.x < 0.1 && tmp.x > -0.1)
            {
                bool left = UnityEngine.Input.GetKey("a");
                bool right = UnityEngine.Input.GetKey("d");
                if (!left && right) tmp.x = 1;
                else if (left && !right) tmp.x = -1;
            }
            _moveVector = new Vector2(tmp.x * Mathf.Cos(angle) - tmp.y * Mathf.Sin(angle),
                                      tmp.x * Mathf.Sin(angle) + tmp.y * Mathf.Cos(angle));

            if (staminaCool == false && stamina > 0 && _sprintAction.inProgress && _moveVector.sqrMagnitude > 0 && GetComponent<Weapon_Hammer>().isCharging() == false)
            {
                usingStamina = true;
                stamina -= Time.deltaTime * sprintStamina;
                if (stamina < 0)
                {
                    speed = moveSpeed;
                    stamina = 0;
                    usingStamina = false;
                    staminaCool = true;
                }
                else speed = moveSpeed * sprintSpeed;
            }
            else
            {
                speed = moveSpeed;
                if (staminaCool == false && (GetComponent<Weapon_Hammer>().isCharging() || GetComponent<Weapon_Flamethrower>().isTurnOn())) usingStamina = true;
                else usingStamina = false;
            }
            
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

            if (_dashAction.triggered == true && stamina >= 1.0f && dashCooldownCounter < 0 && staminaCool == false)
            {
                dashAble = Instantiate(DashDetect, transform.position + new Vector3(lastMove.x, lastMove.y, 0), Quaternion.identity);
                dashAble.transform.rotation = transform.rotation;
                dashAble.GetComponent<DashDetector>().setMove(new Vector2(lastMove.x, lastMove.y));
                dashCount = 5;
                dashCooldownCounter = dashCooldownSet;
            }
        }

        private void FixedUpdate()
        {
            Vector2 currPosition = transform.position;
            int hammerRecoil = 1;
            if(GetComponent<PlayerData>().hammerCooldown() > 0.8f){
                hammerRecoil = 0;
            }
            if(GetComponent<Weapon_Hammer>().isCharging() == false) _rb.MovePosition(currPosition + _moveVector * (speed * Time.fixedDeltaTime) 
                * (1 - Mathf.Max(GetComponent<PlayerData>().flamethrowerFever() / 3, 0)) * hammerRecoil);
            if (usingStamina == false) stamina += Time.fixedDeltaTime;
            if (stamina > staminaMax)
            {
                stamina = staminaMax;
                staminaCool = false;
            }
            dashCount--;
            dashCooldownCounter--;
        }

        private void Interaction()
        {
            if (_lever != null) _lever.Interaction();
            if (_item != null)
            {
                GetComponent<AudioSource>().PlayOneShot(itemObtainSound, 1.0f);
                _item.CollectItem();
            }
        }

    }
}