using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region Player Movement Speeds&benchmarks
    //Actual variables affecting game
    [SerializeField]  float Xspeed ; // The Xspeed
    [SerializeField] float Yspeed; // The Yspeed
    float climbSpeed = 2f; //Yspeed while climbing
    
    
    //Benchmarks------------------------------------

    const float XSPEEDBASE = 10f; // benchmark for regular Xspeed
    const float YSPEEDBASE  = 10f; // benchmark for regular Yspeed
    const float CLIMBSPEEDBASE = 2f; // benchmarks for Yspeed While Climbing
    const float GRAVITYBASE = 3; // benchmark for regular gravity for the player
    const float SPEEDMULTIPLIERBASE = 1f; //Benchmark for regular speed multiplier
    float speedMultiplier; //Multiplier applied to player movementspeed  this is the  thing to alter for temporary boosts to movementspeed
    
    #endregion
    #region  Special Effects Vectors
    [SerializeField] Vector2 deathKick = new Vector2 (3f,8f); // Happens when player dies
    #endregion

    #region  Gun Related Variables

    //These are leftover remnants of the tutorial game doesnt fit actual project and will be removes but kept for disecting the systems for learning
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    #endregion
    #region Colliders Body and Animation
    Collider2D myColliderCapsule ;
    float myGravity ;
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator animator;
    Collider2D myColliderFeet;
    #endregion

    bool isAlive = true;
    void Start()
    {
       
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myColliderCapsule = GetComponent<Collider2D>();
        myColliderFeet = GetComponent<BoxCollider2D>();
        SetMovementSpeed(); //Initialize base movmentspeeds to player object



        
    }
    //Sets the diffrent movement parameters
    public void SetMovementSpeed(float x =XSPEEDBASE,float y = YSPEEDBASE,float climb = CLIMBSPEEDBASE,float multiplier = SPEEDMULTIPLIERBASE,float gravity = GRAVITYBASE)
    {
        Xspeed = x;   
        Yspeed = y;
        speedMultiplier = multiplier;
        climbSpeed = climb;
        myRigidBody.gravityScale = gravity;
    }

    void Update()
    {
        if(!isAlive){ return;}
        
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        
    }

    void OnMove(InputValue value)
    {
        if(!isAlive){ return;}
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
        
    }
    void OnAttack(InputValue value)
    {
        if(value.isPressed)
        {
            Instantiate(bullet, gun.position,transform.rotation);

        }
    }
    void ClimbLadder()
    {
        if(!myColliderCapsule.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            myRigidBody.gravityScale = myGravity;
            animator.SetBool("isClimbing",false);
            return;
        }
        Vector2 playerVelocity = new Vector2 (myRigidBody.linearVelocity.x,moveInput.y * climbSpeed * speedMultiplier);
        myRigidBody.linearVelocity = playerVelocity;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.linearVelocity.y)> Mathf.Epsilon;
        animator.SetBool("isClimbing",true);
        myRigidBody.gravityScale = 0f;
        
    }
    void OnJump(InputValue value)
    {
        if(value.isPressed && myColliderFeet.IsTouchingLayers(LayerMask.GetMask("Ground","Climb")))
        {
            myRigidBody.linearVelocity += new Vector2(0f,Yspeed);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * (Xspeed*speedMultiplier),myRigidBody.linearVelocity.y);
        myRigidBody.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.linearVelocity.x)> Mathf.Epsilon;
        animator.SetBool("isRunning",playerHasHorizontalSpeed);
    }
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.linearVelocity.x)> Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.linearVelocity.x),1f);
           
        }

        
    }
    public void ChangeMovespeed(float multiplier)
    {
        speedMultiplier += multiplier;
    }
    void Die()
    {
        if(myRigidBody.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {

            isAlive = false;
            animator.SetBool("isDead",true);
            myRigidBody.linearVelocity = deathKick;
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
}
