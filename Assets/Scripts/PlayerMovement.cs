using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;             //Spillerens hastighed
    public float jumpForce;         //Spillerens hoppe styrke
    private float moveInput;        //Spillerens input

    private bool facingRight = true;
    private bool isRunning = false;

    public int damage;

    private Animator anim;        //refference til animatoren
    private Rigidbody2D rb;

    public int health = 100;        //Modstanderens standard mængde liv
    private int currentHealth;      //Modstanderens nuværende liv

    private SpriteRenderer spriteRenderer;

    private bool isGrounded = true;        //Checker om spilleren står på jorden
    public Transform groundCheck;   //Position som bruges til at tjekke om spilleren står på jorden
    public float checkRadius;       //Radius som bruges til at tjekke om spilleren står på jorden
    public LayerMask whatIsGround;  //Finder ud af hvilket lag der er jorden

    private int extraJumps;
    public int extraJumpsValue;     //Spillerens antal hop


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();     //Henter modstanderens animator, så komponentet kan ændres
        currentHealth = health;              //Sætter modstanderens liv til variablen healths værdi
        rb = GetComponent<Rigidbody2D>();    //Henter modstanderens Rigidbody, så komponentet kan ændres
    }
    private void FixedUpdate()
    {
        //Checker om spilleren står på jorden
        //Ud fra en cirkels position, radius, og om den rammer det rigtige lag
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //Flytter spilleren
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        //Gør så spilleren begynder at løbe når man trykker på A og D
        if (Input.GetButton("Horizontal"))
        {
            anim.SetBool("isRunning",true);
        }
        else
        {
            anim.SetBool("isRunning",false);
        }


        
        if ( moveInput > 0)                 //Checker om spilleren bevæger sig mod højre 
        {
            spriteRenderer.flipX = false;   //Vender spilleren
        }
        if ( moveInput < 0)                 //Checker om spilleren bevæger sig mod venstre
        {
            spriteRenderer.flipX = true;    //Vender spilleren
        }
    }

    private void Update()
    {
        if (isGrounded == true && rb.velocity.y == 0)   //Hvis spilleren står på jorden og ikke bevæger sig opad
        {
            anim.SetBool("isGrounded", true);                    //Gør så det ligner at spilleren kun kan løbe når man er på jorden
            extraJumps = extraJumpsValue;                        //Nulstiller antallet af hop
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)   //Hvis spilleren trykker på "W" og har flere hop
        {
            anim.SetTrigger("jump");                             //Bruges til at animere spilleren
            anim.SetBool("isGrounded", false);                   //Gør så det ikke ligner at spilleren løber når man hopper
            rb.velocity = Vector2.up * jumpForce;                //Sørger for at spilleren kan hoppe
            extraJumps--;                                        //Trækker et jump fra
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        currentHealth -= damage;    //Gør så modstanderen tager skade
        anim.SetTrigger("Hurt");    //Spiller skade animationen
    }
}
