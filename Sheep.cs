using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour
{
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float idleTime = 1.5f;

    private int facingDirection = 1;
    private Animator animator;
    private bool isMoving = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WanderRoutine());
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            // Idle state
            SetMoving(false);
            yield return new WaitForSeconds(idleTime);

            // Choose random direction
            int direction = Random.Range(0, 2) == 0 ? -1 : 1;

            if (direction != facingDirection)
                Flip();

            // Start moving
            Vector3 targetPosition = transform.position + new Vector3(direction * moveDistance, 0f, 0f);

            SetMoving(true);
            while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            SetMoving(false);
        }
    }

    private void SetMoving(bool moving)
    {
        isMoving = moving;
        if (animator != null)
            animator.SetBool("isMoving", moving);
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
