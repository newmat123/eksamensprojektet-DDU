using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;                     //Spillerens hastighed
    public float jumpForce;                 //Spillerens hoppe styrke
    private float moveInput;                //Spillerens input A/D eller piletasterne

    private Animator anim;                  //Refference til spillerens animatoren
    private Rigidbody2D rb;                 //Refference til spillerens rigidbody
    private SpriteRenderer spriteRenderer;  //Refference til modstanderens spriterenderer, så vi kan vende modstanderen istedet for at have 2 sprites

    public int health = 100;                //Spillerens standard mængde liv
    public int currentHealth;               //Spillerens nuværende liv
    public int damage;                      //Spillerens skade

    private bool isGrounded = true;         //Checker om spilleren står på jorden
    public Transform groundCheck;           //Position som bruges til at tjekke om spilleren står på jorden
    public float checkRadius;               //Radius som bruges til at tjekke om spilleren står på jorden
    public LayerMask whatIsGround;          //Finder ud af hvilket lag der er jorden

    private int extraJumps;                 //Spillerens nuværende antal hop
    public int extraJumpsValue;             //Spillerens standard antal hop


    void Start()                                            //Hvad der sker når vi starter scenen
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    //Henter modstanderens spriterenderer, så komponentet kan ændres
        anim = GetComponent<Animator>();                    //Henter modstanderens animator, så komponentet kan ændres
        currentHealth = health;                             //Sætter modstanderens liv til variablen healths værdi
        rb = GetComponent<Rigidbody2D>();                   //Henter modstanderens Rigidbody, så komponentet kan ændres
    }
    private void FixedUpdate()
    {
        //Checker om spilleren står på jorden
        //Ud fra en cirkels position, radius, og om den rammer det rigtige lag
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");                        //Checker om man trykker på A/D eller piletasterne til venstre eller højre
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);    //Flytter spilleren på x-aksen

        if (Input.GetButton("Horizontal"))                              //Hvis man trykker på A/D eller piletasterne til venstre eller højre
        {
            anim.SetBool("isRunning",true);                             //Animeres spilleren til at løbe    
        }
        else                                                            //Hvis man ikke trykker på A/D eller piletasterne til venstre eller højre
        {
            anim.SetBool("isRunning",false);                            //Skal spilleren ikke animeres til at løbe
        }
        
        if ( moveInput > 0)                 //Checker om spilleren bevæger sig mod højre 
        {
            spriteRenderer.flipX = false;   //Vender spillerens sprite sådan at man vender mod højre
        }
        if ( moveInput < 0)                 //Checker om spilleren bevæger sig mod venstre
        {
            spriteRenderer.flipX = true;    //Vender spillerens sprite sådan at man vender mod venstre
        }
    }

    private void Update()                                        //Hvad der sker hver frame        
    {
        if (isGrounded == true && rb.velocity.y == 0)            //Hvis spilleren står på jorden og ikke bevæger sig opad
        {
            anim.SetBool("isGrounded", true);                    //Gør så det ligner at spilleren kun kan løbe når man er på jorden
            extraJumps = extraJumpsValue;                        //Nulstiller antallet af hop
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 || Input.GetKeyDown(KeyCode.W) && extraJumps > 0)   //Hvis spilleren trykker på "W" eller "Space" og har flere hop
        {
            anim.SetTrigger("jump");                             //Animeres spilleren til at hoppe
            anim.SetBool("isGrounded", false);                   //Gør så det ikke ligner at spilleren løber når man hopper
            rb.velocity = Vector2.up * jumpForce;                //Bevæger spilleren opad
            extraJumps--;                                        //Trækker et jump fra variablen "extraJumps"
        }
    }

    public void TakeDamage(int damage)  //Sker når spilleren tager skade
    {
        Debug.Log(damage);              //Viser hvor meget skade at spilleren tog i konsolen
        currentHealth -= damage;        //trækker skaden fra spillerens nuværende liv
        //anim.SetTrigger("Hurt");        //Spiller skade animationen

        if (currentHealth <= 0)         //Hvis spillerens liv er lig eller under 0
        {
            StartCoroutine(die());      //Kør die() funktionen
        }
    }

    IEnumerator die()                                  //Sker når modstanderens liv er lig eller under 0
    {
        rb.velocity = new Vector2(0, 0);               //Få spilleren til at stå stille        
        anim.SetBool("isDead", true);                  //Spiller døds animationen  
        yield return new WaitForSeconds(2);            //Vent 2 sekunder
        Destroy(gameObject);                           //Fjern spilleren fra spillet


    }

}
