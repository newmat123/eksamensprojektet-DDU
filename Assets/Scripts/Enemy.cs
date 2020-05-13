using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;                     //Modstanderens hastighed

    private bool hasTarget = false;         //Checker om modstanderen har et target
    private GameObject target;              //Modstanderens target

    public int health = 100;                //Modstanderens standard mængde liv
    private int currentHealth;              //Modstanderens nuværende liv

    public int attackDamage;                //Modstanderens skade
    public float minAttackRange = 2.5f;     //Mindste distance før at modstanderen vil angribe
    private float attackTime = 3f;          //Tid der skal gå før at modstanderen kan angribe
    public float nextAttackTime = 2f;       //Timer der skal tælle op til attackTime, før at modstanderen kan angribe

    private Animator anim;                  //Refference til modstanderens animator
    private Rigidbody2D rb;                 //Refference til modstanderens rigidbody, så vi kan flytte på modstanderen
    private SpriteRenderer spriteRenderer;  //refference til modstanderens spriterenderer, så vi kan vende modstanderen istedet for at have 2 sprites

    GameObject player;                      //Spillerens gameobjekt, så spilleren kan blive skadet
    PlayerMovement playerHealth;            //Refference til scriptet Playermovement så spilleren kan blive skadet

    public Vector2 Direction;               //Checker hvilken retning modstanderen vender

    public HealthBar healthBar;


    public void Start()                                         //Hvad der sker når vi starter scenen
    {
        player = GameObject.FindGameObjectWithTag("Player");    //Henter spilleren som et gameobjekt
        playerHealth = player.GetComponent<PlayerMovement>();   //Henter spillerens script "PlayerMovement"
        spriteRenderer = GetComponent<SpriteRenderer>();        //Henter modstanderens spriterenderer, så komponentet kan ændres
        anim = GetComponent<Animator>();                        //Henter modstanderens animator, så komponentet kan ændres
        currentHealth = health;                                 //Sætter modstanderens liv til variablen healths værdi
        rb = GetComponent<Rigidbody2D>();                       //Henter modstanderens Rigidbody, så komponentet kan ændres
        healthBar.SetMaxHealth(health);                         //Sætter spillerens liv i UI'en til at være lig med spillerens maks liv
    }
    
    void Update()                                                                               //Hvad der sker hver frame
    {
        if (hasTarget)                                                                          //Hvis modstanderen har et mål            
        {
            Direction = (target.transform.position - transform.position).normalized;            //Beregner hvilken retning at spilleren befinder sig i

            float distance = Vector2.Distance(transform.position, target.transform.position);   //Finder distancen mellem modstanderen og spilleren
            
            if (distance > 2f)                          //Hvis modstanderen er mere end 2 units væk fra spilleren
            {
                follow(target.transform);               //Kører follow() funktionen med spilleren som mål

                if (Direction.x > 0)                    //Checker om spilleren er til højre for modstanderen 
                {
                    spriteRenderer.flipX = true;        //Vender spilleren til højre
                }
                if (Direction.x < 0)                    //Checker om spilleren er til venstre for modstanderen 
                {
                    spriteRenderer.flipX = false;       //Vender spilleren til venstre
                }

                anim.SetBool("isRunning", true);        //Gør så modstanderen kan animeres til at løbe

            }     
            if (distance < 2f)                          //Hvis spilleren er indenfor 2f af modstanderen
            {
                attack();                               //Kører attack() funktionen
                anim.SetBool("isRunning", false);       //Gør så modstanderen ikke er animeret til at løbe længere
            }
        }
    }
    
   
    private void OnTriggerEnter2D(Collider2D collision) //Sker når modstanderen rammer en collider
    {
        if (collision.tag.Equals("Player"))             //Hvis modstanderens collider rammer spillerens
        {                                       
            target = collision.gameObject;              //Bliver spilleren til modstanderens mål
            hasTarget = true;                           //Sætter boolen til true, så modstanderen kan følge efter spilleren
        }
    }
    private void OnTriggerExit2D(Collider2D collision)  //Sker når modstanderen ikke collider med noget længere
    {
        if (collision.tag.Equals("Player"))             //Hvis modstanderens collider ikke rammer spillerens længere
        {
            target = null;                              //Gør så modstanderen ikke har noget target længere
            hasTarget = false;                          //Sætter boolen til true, så modstanderen ikke længere kan følge efter spilleren
            rb.velocity = new Vector2(0,0);             //Stopper modstanderens bevægelser
            anim.SetBool("isRunning", false);           //Gør så modstanderen ikke er animeret til at løbe længere
        }
    }

    private void PlayerTakeDamage()
    {
        playerHealth.TakeDamage(attackDamage);  //Skad spilleren med variablen attackDamage værdi  
    }

    private void attack()                               //Sker når modstanderen er indenfor 2f af spilleren
    {
        if (attackTime >= nextAttackTime)               //hvis variablen attackTime er større end variablen nextAttackTime
        {
            if (playerHealth.currentHealth > 0)         //Hvis spilleren stadig er i live
            {
                anim.SetTrigger("attack");              //Animer modstanderen til at angribe spilleren
                attackTime = 0;                         //Gør variablen attackTime lig med 0
            }
        }
        else                                            //Hvis variablen attackTime ikke er større end variablen nextAttackTime
        {
            attackTime += Time.deltaTime;               //Tilføj tid til variablen attackTime
        }
    }


    private void follow(Transform target)                                                                           //Sker når modstanderen har et mål
    {
        rb.velocity = new Vector2((target.transform.position.x - transform.position.x)*speed*Time.deltaTime, rb.velocity.y);    // Gør så modstanderen følger efter spilleren
    }

    public void TakeDamage(int damage)          //Sker når modstanderen bliver angrebet
    {
        anim.SetTrigger("Hurt");                //Spiller skade animationen
        Debug.Log(damage);                      //Viser hvor meget skade at modstanderen tog i konsolen
        currentHealth -= damage;                //Gør så modstanderen tager skade
        healthBar.SetHealth(currentHealth);     //Ændrer UI'en

        if (currentHealth <= 0)                 //Hvis modstanderens liv er lig eller under 0
        {
            StartCoroutine(die());              //Kør die() funktionen
        }
    }

    IEnumerator die()                                  //Sker når modstanderens liv er lig eller under 0
    {
        hasTarget = false;
        target = null; 
        anim.SetBool("IsDead", true);                  //Spiller døds animationen  
        yield return new WaitForSeconds(2);            //Vent 2 sekunder
        Destroy(gameObject);                           //Fjern modstanderen fra spillet
    }
}
