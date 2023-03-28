using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    private Transform _player;
    private Rigidbody2D _rigidbody;
    private int _speed;
    private float _aggroRange;
    private BossBehavior _behavior;
    private bool _secondPhase;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = animator.GetComponent<BossBehavior>().Player;
        _rigidbody = animator.GetComponent<Rigidbody2D>();
        _speed = animator.GetComponent<BossBehavior>().BossMovingSpeed;
        _behavior = animator.transform.GetComponent<BossBehavior>();
        _aggroRange = animator.GetComponent<BossBehavior>().AttackRange;
        _secondPhase = animator.GetComponent<BossBehavior>().SecondPhase;
        if (_secondPhase)
        {
            _speed *= 2;
            animator.SetBool("FirstPhase", false);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _behavior.LookAtPlayer();

        Vector2 playerTarget = new Vector2(_player.position.x,_rigidbody.position.y);
        Vector2 newPos = Vector2.MoveTowards(_rigidbody.position, playerTarget, _speed * Time.fixedDeltaTime);
        
        if(Vector2.Distance(_player.position,_rigidbody.position) <= _aggroRange)
        {
            animator.SetTrigger("Aggroed");
        }
        _rigidbody.MovePosition(newPos);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Aggroed");
    }

}
