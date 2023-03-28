using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : Entity
{
    [SerializeField] private BoxCollider2D _bossColider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public delegate void BossFightWon();
    public static event BossFightWon onBossFightWon;

    public delegate void BossHalfHealth();
    public static event BossHalfHealth onBossHalfHealth;
    public bool IsInvulnerable = false;

    private Animator _animator;
    private UIManager _UImanager;
    private bool _isDead = false;
    private void Start()
    {
        _UImanager = UIManager.Instance;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void Die()
    {
        //do cool death animation;
        if(_isDead == false)
        {
            _animator.SetTrigger("Death");
            _animator.Play("Hit");
            Physics2D.IgnoreLayerCollision(6, 7, true);
            onBossFightWon();
            Destroy(gameObject,10f);
        }
    }
    private IEnumerator DeathCounter()
    {
        _bossColider.enabled = !_bossColider.enabled;
        yield return new WaitForSeconds(150f);
    }

    //better than having hit animation that only changes color
    private IEnumerator DamagedFlashing()
    {
        for (int i = 0; i < 3; i++)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public override void TakeDamage(float damage)
    {
        if (IsInvulnerable)
            return;
        base.TakeDamage(damage);
        if (Health <= 0)
        {
            Die();
            _isDead = true;
        }
        if (Health > 0)
        {
            StartCoroutine(DamagedFlashing());
        }
        if (Health <= MaxHealth / 2)
        {
            onBossHalfHealth();
            GetComponent<Animator>().SetBool("SecondPhase", true);
        }

        Debug.Log($"{GetHealth()}  from max health: {MaxHealth}");
        _UImanager.SetBossHealth(Health);
    }
}
