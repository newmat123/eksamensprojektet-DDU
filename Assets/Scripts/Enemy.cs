using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    private int currentHealth;
    public float speed;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        //Sætter modstanderens liv til variablen healths værdi
        currentHealth = health;
    }

    void Update()
    {
        
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
