using UnityEngine;

public class AttackState : IState
{
    private Enemy enemy;
    private Animator animator;

    private float attackTimer;
    private float postDelayTimer;

    private bool hasAttacked = false;
    private bool isPostDelay = false;

    private float postDelay = 1f; // ⭐ 공격 후 대기 시간

    public AttackState(Enemy enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        attackTimer = 0f;
        postDelayTimer = 0f;
        hasAttacked = false;
        isPostDelay = false;

        enemy.Stop(); // 이동 차단
        //animator.SetBool("setAttack", true);
    }

    public void Update()
    {
        // 1️⃣ 공격 전
        if (!hasAttacked)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= enemy.GetAttackCooldown())
            {
                enemy.Attack();
                hasAttacked = true;
                isPostDelay = true;
            }

            return;
        }

        // 2️⃣ 공격 후 대기
        if (isPostDelay)
        {
            postDelayTimer += Time.deltaTime;

            if (postDelayTimer >= postDelay)
            {
                isPostDelay = false;

                if (!enemy.IsInAttackRange())
                {
                    enemy.stateMachine.ChangeState(
                        new ChaseState(enemy, animator)
                    );
                }
                else
                {
                    // 다시 공격 상태 유지 (연속 공격)
                    enemy.stateMachine.ChangeState(
                        new AttackState(enemy, animator)
                    );
                }
            }
        }
    }

    public void Exit()
    {
        //animator.SetBool("setAttack", false);
        animator.ResetTrigger("attackTrigger");
        animator.Play("Idle", 0, 0f);
    }
}