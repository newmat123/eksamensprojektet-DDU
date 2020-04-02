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
            //get distance between me and my target
            float distance = Vector3.Distance(transform.position, target.transform.position);
            // am I further than 2 units away
            if (distance > 2)
            {
                // I am over 2 units away
                follow(target.transform); // do a follow
            }
        }
    }
    
    // if anything starts to collide with me I will run this method
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {    // is the other object the player
            target = collision.gameObject;      // it is so set him as my target
            hasTarget = true;   // I have a target
        }
    }

    // if something is no longer coliiding with me I will run this code
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            target = null;
            hasTarget = false;
            rb.velocity = new Vector2(0,0);
        }
    }

    private void follow(Transform target)
    {
        // add force to my rigid body to make me move
        rb.velocity = (target.transform.position - transform.position).normalized * speed;
    }

    public void TakeDamage(int damage)
    {
        //Gør så modstanderen tager skade
        currentHealth -= damage;
        //Spiller skade animationen
        anim.SetTrigger("Hurt");
       
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
