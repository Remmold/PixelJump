using System;
using System.Collections;
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
    float climbSpeed; //Yspeed while climbing
    float regularGravity;
    
    
    //Benchmarks------------------------------------

    const float XSPEEDBASE = 6f; // benchmark for regular Xspeed
    const float YSPEEDBASE  = 9f; // benchmark for regular Yspeed
    const float CLIMBSPEEDBASE = 4f; // benchmarks for Yspeed While Climbing
    const float GRAVITYBASE = 2.5f; // benchmark for regular gravity for the player
    const float SPEEDMULTIPLIERBASE = 1f; //Benchmark for regular speed multiplier
    float speedMultiplier; //Multiplier applied to player movementspeed  this is the  thing to alter for temporary boosts to movementspeed
    
    #endregion
    #region  Special Effects Vectors
    [SerializeField] Vector2 deathKick = new Vector2 (3f,8f); // Happens when player dies
    #endregion
    
    #region Colliders Body and Animation
    Collider2D myColliderCapsule ;
    Collider2D myColliderFeet;
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator animator;
    #endregion
    #region Player State Variables
    bool isAlive = true;
    #endregion


    void Start()
    {
       
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myColliderCapsule = GetComponent<Collider2D>();
        myColliderFeet = GetComponent<BoxCollider2D>();
        regularGravity = GRAVITYBASE;
        SetMovementParams(); //Initialize base movmentspeeds to player object
 
    }
    void Update()
    {
        if(!isAlive){ return;}//Player is dead
        
        Run();
        FlipSprite(); //Checks direction and flips sprite
        ClimbLadder(); 
        TouchDangerCheck(); //Checks if player touches layer Hazards or Enemies
        
    }
    //Sets the diffrent movement parameters PERMANENTLY
    public void SetMovementParams(float x =XSPEEDBASE,float y = YSPEEDBASE,float climb = CLIMBSPEEDBASE,float multiplier = SPEEDMULTIPLIERBASE,float gravity = GRAVITYBASE)
    {
        Xspeed = x;   
        Yspeed = y;
        speedMultiplier = multiplier;
        climbSpeed = climb;
        myRigidBody.gravityScale = gravity;
    }
    /// <summary>
    /// Sets player movement stats temporarily
    /// </summary>
    /// <param name="x = new Xspeed"></param>
    /// <param name="y = new Yspeed"></param>
    /// <param name="climb = newClimbspeed"></param>
    /// <param name="multiplier = new SpeedMultiplier"></param>
    /// <param name="gravity = New gravity scale"></param>
    /// <param name="duration = Duration in SECONDS"></param>
    /// <returns></returns>
    public IEnumerator TemporaryMovementChange(float x =XSPEEDBASE,float y = YSPEEDBASE,float climb = CLIMBSPEEDBASE,float multiplier = SPEEDMULTIPLIERBASE,float gravity = GRAVITYBASE, float duration = 0)
    {
        // Store original values
        float originalX = Xspeed;
        float originalY = Yspeed;
        float originalClimb = climbSpeed;
        float originalMultiplier = speedMultiplier;
        float originalGravity = myRigidBody.gravityScale;
        

        // Apply new values
        SetMovementParams(x, y, climb, multiplier, gravity);
        regularGravity = gravity;
        Debug.Log("Temporary movement change applied!");

        // Wait for duration
        yield return new WaitForSecondsRealtime(duration);

        // Revert to original values
        SetMovementParams(originalX, originalY, originalClimb, originalMultiplier, originalGravity);
        regularGravity = originalGravity;
        Debug.Log("Movement reverted to original values.");
    }


    #region Input Methods
    void OnMove(InputValue value)
    {
        if(!isAlive){ return;}
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
        
    }
    //Change this. bullets have no room in my game! :)
    void OnAttack(InputValue value)
    {
        if(value.isPressed)
        {
            //This happens when pressing left mb

        }
    }

    void OnJump(InputValue value)
    {
        if(value.isPressed && myColliderFeet.IsTouchingLayers(LayerMask.GetMask("Ground","Climb","Bouncing")))
        {
            //animator.SetBool("isJumping",true);
            myRigidBody.linearVelocity += new Vector2(0f,Yspeed);
        }
    }
    #endregion
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
    void TouchDangerCheck()
    {
        if(myRigidBody.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {

            isAlive = false;
            animator.SetBool("isDead",true);
            myRigidBody.linearVelocity = deathKick;
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
    void ClimbLadder()
    {
        
        if(!myColliderCapsule.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            myRigidBody.gravityScale = regularGravity;
            animator.SetBool("isClimbing",false);
            return;
        }
        else
        {
            Vector2 playerVelocity = new Vector2 (myRigidBody.linearVelocity.x,moveInput.y * climbSpeed * speedMultiplier);
            myRigidBody.linearVelocity = playerVelocity;

            animator.SetBool("isClimbing",true);
            myRigidBody.gravityScale = 0f;
        }
        
    }
}
