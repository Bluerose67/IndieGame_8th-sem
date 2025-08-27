using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayer;


    public Animator anim;
    public float cooldown = 2;
    private float timer;


    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);

            timer = cooldown;
        }
    }
    public void DealDamage()
{
    if (attackPoint == null)
    {
        Debug.LogError("Attack Point not assigned!");
        return;
    }

    if (StatsManager.Instance == null)
    {
        Debug.LogError("StatsManager instance is missing!");
        return;
    }

    Collider2D[] enemies = Physics2D.OverlapCircleAll(
        attackPoint.position,
        StatsManager.Instance.weaponRange,
        enemyLayer
    );

    foreach (Collider2D enemy in enemies)
    {
        Enemy_Health health = enemy.GetComponent<Enemy_Health>();
        Enemy_Knockback knockback = enemy.GetComponent<Enemy_Knockback>();

        if (health != null)
        {
            health.ChangeHealth(-StatsManager.Instance.damage);
        }
        else
        {
            Debug.LogWarning(enemy.name + " is missing Enemy_Health component!");
        }

        if (knockback != null)
        {
            knockback.Knockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime, StatsManager.Instance.stunTime);
        }
        else
        {
            Debug.LogWarning(enemy.name + " is missing Enemy_Knockback component!");
        }
    }
}


    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(attackPoint.position, StatsManager.Instance.weaponRange);
    // }
}
