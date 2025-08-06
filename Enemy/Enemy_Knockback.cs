using UnityEngine;
using System.Collections;


public class Enemy_Knockback : MonoBehaviour
{

    private Rigidbody2D rb;
    private Enemy_Movement enemy_Movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy_Movement = GetComponent<Enemy_Movement>();
    }


    public void Knockback(Transform playerTransform, float knockbackForce, float stunTime)
    {
        enemy_Movement.ChangeState(EnemyState.knockback);
        StartCoroutine(StunTimer(stunTime));
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
    }

    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        enemy_Movement.ChangeState(EnemyState.Idle);
    }
}

