using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState
    {
        Normal,Dodging,
    }

    [SerializeField] private int _speed;
    [SerializeField] private GameObject _playerSprite;
    [SerializeField] private float _rollingSpeed;
    [SerializeField] private int _rollingStaminaUsage;
    [SerializeField] [Range(0.5f,2.0f)] private float _rollingStaminaCooldown;

    //to change to rigidbody check out the dashing from neon runner and Ethereal
    private Animator _animator;
    private float _originalRollingSpeed;
    private PlayerState _state;
    private Vector3 _playerScale;
    private PlayerControls _playerControls;
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Vector2 _input;
    private bool _isTryingToJump;
    private bool _canMove = true;
    private UIManager _UIManager;

    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void Start()
    {
        _UIManager=UIManager.Instance;
    }
    private void Awake()
    {
        _state = PlayerState.Normal;
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _playerScale = _playerSprite.transform.localScale;
        _originalRollingSpeed = _rollingSpeed;
        PlayerCombat.onAttackMovementLocked += OnAttackMovementLocked;
        PlayerBase.onPlayerDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        _canMove = false;
    }

    private void OnAttackMovementLocked()
    {
        StartCoroutine(MovementBlockedTimer());   
    }

    private IEnumerator MovementBlockedTimer()
    {
        _canMove = false;
        yield return new WaitForSeconds(0.5f);
        _canMove = true;
    }
    private void Update()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_direction.x));
        switch(_state)
        {
            case PlayerState.Normal:
                UpdatePlayerInput();
                UpdateFacingDirection();
                DodgeRollInput();
                break;
            case PlayerState.Dodging:
                Roll();
                break;
        }
        print(_state);
    }

    private void Roll()
    {
        //switch up to riogidbody after etsting
        if (_canMove)
        {
            Vector2 direction = _direction;
            if(_direction == Vector2.zero)
            {
                direction = new Vector2(_playerSprite.transform.localScale.x, 0);
            }
            Physics2D.IgnoreLayerCollision(6, 7,true);
                transform.position += (Vector3)direction.normalized * _rollingSpeed * Time.deltaTime;
                _rollingSpeed -= _rollingSpeed * 10f * Time.deltaTime;
                if (_rollingSpeed < 5)
                {
                    Physics2D.IgnoreLayerCollision(6, 7,false);
                    _state = PlayerState.Normal;
                }
        }
    }

    private void DodgeRollInput()
    {
        if (_playerControls.Player.Roll.WasPerformedThisFrame())
        {
            if (_UIManager.UseStamina(_rollingStaminaUsage, _rollingStaminaCooldown))
            {
                _state = PlayerState.Dodging;
                _rollingSpeed = _originalRollingSpeed;
            }
            else
                print("not enough stamina to roll");
        }
    }

    private void UpdateFacingDirection()
    {
        if (_direction.x < 0)
        {
            _playerSprite.transform.localScale = new Vector3(-_playerScale.x, _playerScale.y, _playerScale.z);
        }
        else if (_direction.x > 0)
        {
            _playerSprite.transform.localScale = new Vector3(_playerScale.x, _playerScale.y, _playerScale.z);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed * Time.deltaTime, _rigidbody.velocity.y);
    }

    private void UpdatePlayerInput()
    {
        float moveX = 0;
        float moveY = 0;

        if (_canMove)
        {
            //if input enabled
            if (_input.x > 0)
            {
                moveX += 1f;
            }
            if (_input.x < 0)
            {
                moveX -= 1f;
            }
        }
        _input = _playerControls.Player.Move.ReadValue<Vector2>();


        if (_playerControls.Player.Jump.WasPerformedThisFrame())
        {
            _isTryingToJump = true;
        }
        else
        {
            _isTryingToJump = false;
        }

        _direction = new Vector2(moveX, moveY).normalized;
    }
    private void OnDestroy()
    {
        PlayerCombat.onAttackMovementLocked-=OnAttackMovementLocked;
        PlayerBase.onPlayerDeath -= OnPlayerDeath;
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
