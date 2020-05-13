using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;              //refference til spillerens animator
    public int damage;                  //Spillerens skade  
    public Transform attackPoint;       //Refference til punktet hvor at spilleren angriber fra
    public LayerMask whatIsEnemies;     //Refference til laget hvor at modstanderne er på
    public float attackRange = 0.5f;    //Omkredsen på hvor at spilleren kan angribe
    public float attackRate = 2f;       //Hvor ofte spilleren kan angribe
    private float nextAttackTime = 0f;  //Timer til spillerens næste angreb

    public void Start()                     //Hvad der sker når vi starter scenen
    {
        anim = GetComponent<Animator>();    //Henter modstanderens animator, så komponentet kan ændres
    }

    void Update()                                               //Hvad der sker hver frame
    {
        if (Time.time >= nextAttackTime)                        //Hvis der er gået langt nok tid siden sidste angreb
        {
            if (Input.GetMouseButtonDown(0))                   //Hvis man trykker på "Venstreklik"
            {
                Attack();                                       //Kører attack() funktionen
                nextAttackTime = Time.time + 1f / attackRate;   //Sæt nextAttackTime til at være ligt med Time.time + 1f/2f
            }
        }
    }
    public void Attack()                                        //Hvad der sker når spilleren angriber
    {
        anim.SetTrigger("attack");                              //Spiller spillerens angrebsanimationen
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whatIsEnemies);  //Laver en liste over alle de modstandere, som er indenfor angrebsrækkeviden og som er under laget "Enemy"
        foreach (Collider2D enemy in hitEnemies)                //Gør alle modstanderne igennem i listen
        
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);     //Henter deres script "Enemy" og kører funktionen TakeDamage() med variablen damages værdi
        }
    }
    void OnDrawGizmosSelected()                                         //Laver en cirkel som kan ses i scenen men ikke i selve spillet
    {
        Gizmos.color = Color.red;                                       //Laver cirklens farve rød
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);       //Laver en cirkel ud fra variablen "attackPoint" position, og giver den en rækkevide på variablen "attackRange"
    }

}
