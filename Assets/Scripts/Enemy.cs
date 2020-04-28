using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;             //Modstanderens hastighed
    public int attackDamage;
    private bool isRunning = false;

    private bool hasTarget = false; // checker om modstanderen har et target
    private GameObject target;      // Modstanderens target

    public float minAttackRange = 2.5f; //mindste distance før at modstanderen vil angribe
    private float attackTime = 3f;
    public float nextAttackTime = 2f;

    GameObject player;
    PlayerMovement playerHealth;

    private Animator anim;        //refference til animatoren
    private Rigidbody2D rb;

    public int health = 100;        //Modstanderens standard mængde liv
    private int currentHealth;      //Modstanderens nuværende liv
   
    private SpriteRenderer spriteRenderer;
    //Retning variable
    private Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();     //Henter modstanderens animator, så komponentet kan ændres
        currentHealth = health;              //Sætter modstanderens liv til variablen healths værdi
        rb = GetComponent<Rigidbody2D>();    //Henter modstanderens Rigidbody, så komponentet kan ændres
    }
    void Update()
    {
        if (hasTarget)
        {
            //Finder spillerens position
            Direction = (target.transform.position - transform.position).normalized;

            float distance = Vector2.Distance(transform.position, target.transform.position); //Finder distancen mellem modstanderen og spilleren
            
            if (distance > 2f)                          //Hvis modstanderen er mere end 2 units væk fra spilleren
            {
                follow(target.transform);               //Følg efter spilleren

                if (Direction.x > 0)                    //Checker om spilleren bevæger sig mod højre 
                {
                    spriteRenderer.flipX = true;        //Vender spilleren
                }
                if (Direction.x < 0)                    //Checker om spilleren bevæger sig mod venstre
                {
                    spriteRenderer.flipX = false;       //Vender spilleren
                }
            }     
            if (distance < 2f)
            {
                attack();
            }
        }
    }
    
   
    private void OnTriggerEnter2D(Collider2D collision) //Hvis modstanderen rammer en collidere
    {
        if (collision.tag.Equals("Player"))     //Hvis modstanderens collider rammer spillerens
        {                                       
            target = collision.gameObject;      //Bliver det modstanderens mål
            hasTarget = true;                   //Sætter boolen til true
            anim.SetBool("isRunning", true);    //Gør så modstanderen kan animeres
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //Hvis modstanderen ikke collider med noget længere
    {
        if (collision.tag.Equals("Player"))             //Hvis modstanderens collider ikke rammer spillerens længere
        {
            target = null;                              //Gør så modstanderen ikke har noget target længere
            hasTarget = false;                          //Sætter boolen til false
            rb.velocity = new Vector2(0,0);             //Stopper modstanderens bevægelser
            anim.SetBool("isRunning", false);
        }
    }

    private void attack()
    {
        if (attackTime >= nextAttackTime) //if the set amount of time has passed
        {
            if (playerHealth.currentHealth > 0)
            {
                anim.SetTrigger("attack");
                playerHealth.TakeDamage(attackDamage); //make player take damage               
            }
            attackTime = 0;
        }
        else
        {
            attackTime += Time.deltaTime; //add time
        }
    }


    private void follow(Transform target)
    {
        // add force to my rigid body to make me move
        rb.velocity = new Vector2((target.transform.position.x - transform.position.x)*speed*Time.deltaTime,0f);
    }
    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hurt");    //Spiller skade animationen
        Debug.Log(damage);
        currentHealth -= damage;    //Gør så modstanderen tager skade

        if (currentHealth <= 0)
        {
            StartCoroutine(die());
        }
    }

    IEnumerator die()
    {
        rb.velocity = new Vector2(0, 0);
        //Spiller døds animationen
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        

    }




}
