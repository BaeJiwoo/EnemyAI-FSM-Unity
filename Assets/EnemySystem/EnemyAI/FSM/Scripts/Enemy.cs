using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rb;
    public float moveSpeed = 3f;
    public float attackRange = 2f;

    [Header("Obstacle Check")]
    public float obstacleCheckDistance = 0.5f;
    public LayerMask obstacleLayer;

    public StateMachine stateMachine;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        stateMachine.ChangeState(new ChaseState(this, this.animator));
    }

    public void MoveToTarget()
    {
        Vector2 dir = (target.position - transform.position).normalized;

        // ⭐ 앞에 장애물 있으면 이동 금지
        if (IsBlocked(dir))
        {
            Stop();
            return;
        }

        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        // 방향 flip
        if (dir.x > 0.05f)
            spriteRenderer.flipX = false;
        else if (dir.x < -0.05f)
            spriteRenderer.flipX = true;
    }

    bool IsBlocked(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) < 0.01f)
            return false;

        Vector2 rayDir = new Vector2(Mathf.Sign(dir.x), 0f);

        // ⭐ pivot 대신 collider center 사용
        Vector2 origin = col.bounds.center;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            rayDir,
            obstacleCheckDistance,
            obstacleLayer
        );

        return hit.collider != null;
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

    // 디버그용 레이 표시
    void OnDrawGizmos()
    {
        if (!Application.isPlaying || target == null) return;

        Gizmos.color = Color.red;

        Vector2 dir = (target.position - transform.position).normalized;
        Vector2 rayDir = new Vector2(Mathf.Sign(dir.x), 0f);

        if (col != null)
        {
            Vector2 origin = col.bounds.center;
            Gizmos.DrawLine(origin, origin + rayDir * obstacleCheckDistance);
        }
    }
}