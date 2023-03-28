using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRate;
    [SerializeField] private int _attackStaminaUsage;
    [SerializeField] [Range(0.5f,2.0f)] private float _attackStaminaCooldown;

    private Animator _animator;
    private float _followingAttackTime = 0;
    public static bool IsAttacking = false;
    private bool _followUpAttack = false;
    private UIManager _UImanager;
    private PlayerControls _playerControls;

    public delegate void AttackMovementLock();
    public static AttackMovementLock onAttackMovementLocked;
    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
        AttackBehaviourQueu.onAttackQueued += OnAttackQueued;
    }


    private void Start()
    {
        _UImanager = UIManager.Instance;
    }
    private void Update()
    {
        if(Time.time > _followingAttackTime)
        {
            if(_playerControls.Player.Attack.WasPressedThisFrame())
            {
                if (!IsAttacking)
                {
                    Attack(1);
                }
                else
                {
                    Attack(2);
                }
                
                _followingAttackTime = Time.time +1f / _attackRate;
            }
        }
    }
    private void OnAttackQueued()
    {
        //IsAttacking = false;
    }

    private void Attack(int attack)
    {
        int damage = (int)_damage;
        if (_UImanager.UseStamina(_attackStaminaUsage, _attackStaminaCooldown))
        {
            onAttackMovementLocked();
            if (attack == 1)
            {
                _animator.SetTrigger("Attack");
                IsAttacking = true;
            }
            else if (attack == 2)
            {
                print("prepping 2nd attack");
                _animator.SetTrigger("SecondAttack");
                IsAttacking = false;
                damage *= 2;
            }
            
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
            //deal damage
            if(enemiesHit.Length > 0)
            {
                foreach (Collider2D enemy in enemiesHit)
                {
                    //Debug.Log("We hit" + enemy.name);
                    enemy.GetComponent<Entity>().TakeDamage(damage);
                }
            }
        }
        else
            print("Not Enough Stamina to attack");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
    private void OnDestroy()
    {
        AttackBehaviourQueu.onAttackQueued -= OnAttackQueued;
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
