using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;        //refference til animatoren
    public int damage;
    public Transform attackPoint;
    public LayerMask whatIsEnemies;
    public float attackRange =0.5f;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

        void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }
    }
    public void Attack()
    {
        anim.SetTrigger("attack");
        Collider2D[] hitEnemies =Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whatIsEnemies);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
