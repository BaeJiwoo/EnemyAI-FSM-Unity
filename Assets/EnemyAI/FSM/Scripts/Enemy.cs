using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rb;
    public float moveSpeed = 3f;
    public float attackRange = 2f;

    public StateMachine stateMachine;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stateMachine.ChangeState(new ChaseState(this, this.animator));
    }

    public void MoveToTarget()
    {
        Vector2 dir = (target.position - transform.position).normalized;

        // 이동
        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        // ⭐ 방향에 따른 flip 처리
        if (dir.x > 0.05f)
        {
            spriteRenderer.flipX = false; // 오른쪽 바라봄
        }
        else if (dir.x < -0.05f)
        {
            spriteRenderer.flipX = true;  // 왼쪽 바라봄
        }
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void Attack()
    {
        Debug.Log("Attack!");
    }

    public bool IsInAttackRange()
    {
        return Vector2.Distance(transform.position, target.position) <= attackRange;
    }
}
