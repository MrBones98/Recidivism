using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Entity
{
    [SerializeField] private float _hitKnockback;

    public delegate void PlayerDied();
    public static event PlayerDied onPlayerDeath;

    private UIManager _UIManager;
    private SceneLoader _sceneLoader;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private bool _invulnerable = false;
    private bool _isDead = false;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _sceneLoader = SceneLoader.Instance;
        _UIManager = UIManager.Instance;
    }
    //when doing player gets hit, do a override of the take damage function k
    public override void TakeDamage(float damage)
    {


        if (!_invulnerable)
        {
            _animator.SetTrigger("PlayerHit");
            print("player ouchie");
            _rigidbody.AddForce(-transform.right * _hitKnockback, ForceMode2D.Impulse);
            base.TakeDamage(damage);
            _UIManager.SetPlayerHealth(Health);
            StartCoroutine(InvulnerabilityTimer());
        }
        if (_invulnerable)
        {
            return;
        }
        if (Health <= 0)
        {
            Die();
            _isDead = true;
        }

    }
    private IEnumerator InvulnerabilityTimer()
    {
        _invulnerable = true;
        yield return new WaitForSeconds(0.8f);
        _invulnerable = false;
    }
    protected override void Die()
    {
        if (_isDead == false)
        {
            _animator.SetBool("IsDead", true);
            _animator.Play("Death");

            Physics2D.IgnoreLayerCollision(6, 7, true);
            StartCoroutine(DeathCounter());
            //Destroy(gameObject);
        }
    }
    private IEnumerator DeathCounter()
    {
        onPlayerDeath();
        //yield return new WaitForSeconds(5.0f);
        yield return new WaitForSeconds(4.0f);
        _sceneLoader.ReloadScene();
    }
}
