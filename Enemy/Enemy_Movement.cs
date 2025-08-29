using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed = 3f;
    public float attackCooldown = 2f;

    private float attackCooldownTimer;
    private int facingDirection = 1;

    private EnemyState enemyState;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (enemyState != EnemyState.knockback && attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
    }

    // --- Movement input from EnemyAI ---
    public void Move(Vector2 direction)
    {
        if (enemyState == EnemyState.Attacking) return;

        if (direction != Vector2.zero)
        {
            if ((direction.x > 0 && facingDirection == -1) ||
                (direction.x < 0 && facingDirection == 1))
                Flip();

            rb.linearVelocity = direction.normalized * speed;
            ChangeState(EnemyState.Chasing);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    // --- Attack input from EnemyAI ---
    public void Attack()
    {
        if (attackCooldownTimer <= 0)
        {
            attackCooldownTimer = attackCooldown;
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Attacking);
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z);
    }

    public void ChangeState(EnemyState newState)
    {
        // Exit animations
        anim.SetBool("isIdle", false);
        anim.SetBool("isChasing", false);
        anim.SetBool("isAttacking", false);

        enemyState = newState;

        // Enter animations
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    knockback
}
