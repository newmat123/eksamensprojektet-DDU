using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;             //Spillerens hastighed
    public float jumpForce;         //Spillerens hoppe styrke
    private float moveInput;        //Spillerens input

    Rigidbody2D rb;
    Animator anim;

    bool facingRight = true;
    bool isRunning = false;

    private bool isGrounded;        //Checker om spilleren står på jorden
    public Transform groundCheck;   //Position som bruges til at tjekke om spilleren står på jorden
    public float checkRadius;       //Radius som bruges til at tjekke om spilleren står på jorden
    public LayerMask whatIsGround;  //Finder ud af hvilket lag der er jorden

    private int extraJumps;
    public int extraJumpsValue;     //Spillerens antal hop

    private void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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


        //Flipper spilleren
        if (facingRight == false && moveInput > 0)
        {
            flip();
        }
        if (facingRight == true && moveInput < 0)
        {
            flip();
        }
    }

    private void Update()
    {
        if (isGrounded == true && rb.velocity.y == 0)   //Hvis spilleren står på jorden og ikke bevæger sig opad
        {
            extraJumps = extraJumpsValue; //Nulstilles antallet af hop
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)   //Hvis spilleren trykker på "W" og har flere hop
        {
            anim.SetTrigger("jump");
            rb.velocity = Vector2.up * jumpForce;    //Sørger for at spilleren kan hoppe
            extraJumps--;                            //Trækker et jump fra
        }
    }

    void flip()
    {
        //Funktion der flipper spilleren
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        
    }
}
