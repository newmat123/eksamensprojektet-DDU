using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;              //Modstanderens standard mængde liv
    private int currentHealth;      //Modstanderens nuværende liv
    public float speed;             //Modstanderens hastighed

    private bool hasTarget = false; // checker om modstanderen har et target
    private GameObject target;      // Modstanderens target

    private Animator anim;          //Modstanderens animator
    private Rigidbody2D rb;

    private bool isRunning = false; //Bruges til at animere modstanderen når han løber

    void Start()
    {
        anim = GetComponent<Animator>();     //Henter modstanderens animator, så komponentet kan ændres
        currentHealth = health;              //Sætter modstanderens liv til variablen healths værdi
        rb = GetComponent<Rigidbody2D>();    //Henter modstanderens Rigidbody, så komponentet kan ændres
    }

    void Update()
    {
        if (hasTarget)
        {
           
            float distance = Vector2.Distance(transform.position, target.transform.position); //Finder distancen mellem modstanderen og spilleren
            
            if (distance > 2)               //Hvis modstanderen er mere end 2 units væk fra spilleren
            {
                follow(target.transform);   //Følg efter spilleren
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

    private void follow(Transform target)
    {
        // add force to my rigid body to make me move
        rb.velocity = new Vector2((target.transform.position.x - transform.position.x)*speed*Time.deltaTime,0f);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;    //Gør så modstanderen tager skade
        anim.SetTrigger("Hurt");    //Spiller skade animationen

        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void die()
    {
            //Spiller døds animationen
            anim.SetBool("IsDead", true);
            //Gør så selve scriptet er slået fra
            Destroy(gameObject);
    }

}
