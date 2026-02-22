using UnityEngine;

public class AttackState : IState
{
    private Enemy enemy;
    private float cooldown = 1f;
    private float timer;
    private Animator animator;

    public AttackState(Enemy enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        timer = 0f;
        enemy.Stop();
        animator.SetBool("setAttack", true);
    }

    public void Update()
    {
        timer += UnityEngine.Time.deltaTime;

        if (timer >= cooldown)
        {
            enemy.Attack();
            timer = 0f;
        }

        if (!enemy.IsInAttackRange())
        {
            enemy.stateMachine.ChangeState(new ChaseState(enemy, animator));
        }
    }

    public void Exit()
    {
        animator.SetBool("setAttack", false);
    }
}
