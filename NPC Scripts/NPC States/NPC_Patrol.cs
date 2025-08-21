using System.Collections;
using UnityEngine;

public class NPC_Patrol : MonoBehaviour
{
    public float speed = 2;
    public float pauseDuration = 1.5f;

    private bool isPaused;
    public Vector2[] patrolPoints;
    private int currentPatrolIndex;
    private Vector2 target;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        // If no patrol points then just play idle anim
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                anim.Play("Idle");
                enabled = false;
                return;
            }
        StartCoroutine(SetPatrolPoints());
    }

    void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector3)target - transform.position).normalized;
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(transform.position, target) < .1f)
            StartCoroutine(SetPatrolPoints());
    }

    IEnumerator SetPatrolPoints()
    {
        isPaused = true;
        anim.Play("Idle");
        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        target = patrolPoints[currentPatrolIndex];
        isPaused = false;
        anim.Play("Walk");
    }
}
