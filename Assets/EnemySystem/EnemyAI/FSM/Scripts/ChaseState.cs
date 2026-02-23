using UnityEngine;

public class ChaseState : IState
{
    private Enemy enemy;
    private Animator animator;

    public ChaseState(Enemy enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        animator.SetBool("IsRunning", true);
    }

    public void Update()
    {
        enemy.MoveToTarget();

        if (enemy.IsInAttackRange())
        {
            enemy.stateMachine.ChangeState(
                new AttackState(enemy, animator)
            );
        }
    }

    public void Exit()
    {
        enemy.Stop();
        animator.SetBool("IsRunning", false);
    }
}