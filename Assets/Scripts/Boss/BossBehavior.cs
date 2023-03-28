using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [SerializeField] private float _normalBossDamage;
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private LayerMask _playerLayer;

    private Animator _animator;
    private Vector3 _bossScale;

    public int BossMovingSpeed;
    public Transform Player;
    public float AttackRange;
    public bool SecondPhase = false;

    private void Awake()
    {
        BossTrigger.onBossFightTriggered += OnBossFightTriggered;
        BossBase.onBossHalfHealth += OnBossHalfHealth;
    }

    private void OnBossHalfHealth()
    {
        SecondPhase = true;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _bossScale = transform.localScale;
    }
    private void OnBossFightTriggered()
    {
        Player = PlayerMarker.Instance.gameObject.transform;
        _animator.SetTrigger("FightTriggered");
    }
    public void LookAtPlayer()
    {
        if(transform.position.x > Player.position.x)
        {
            transform.localScale = new Vector3(-_bossScale.x, _bossScale.y, _bossScale.z);
        }
        else if(transform.position.x < Player.position.x)
        {
            transform.localScale = new Vector3(_bossScale.x, _bossScale.y, _bossScale.z);
        }
    }
    public void Attack(float damage)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(_attackOrigin.position, new Vector2(2f, 3.5f), _playerLayer);
        foreach (Collider2D collider in collider2Ds)
        {
            if (SecondPhase)
                damage *= 1.5f;
            if(collider.TryGetComponent(out PlayerBase playerBase))
            {
                collider.GetComponent<PlayerBase>().TakeDamage(damage);
                Debug.Log(collider.gameObject.name);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackOrigin.position, new Vector2(2f, 4f));
    }
    private void OnDestroy()
    {
        BossTrigger.onBossFightTriggered -= OnBossFightTriggered;
        BossBase.onBossHalfHealth -= OnBossHalfHealth;
    }
}
