using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    #region Player Movement Speeds & Benchmarks
    [SerializeField] private float Xspeed; // The Xspeed
    [SerializeField] private float Yspeed; // The Yspeed
    private float climbSpeed;
    private float regularGravity;
    private float speedMultiplier;
    private bool isActive;

    // Benchmarks
    private const float XSPEEDBASE = 6f;
    private const float YSPEEDBASE = 9f;
    private const float CLIMBSPEEDBASE = 4f;
    private const float GRAVITYBASE = 2.5f;
    private const float SPEEDMULTIPLIERBASE = 1f;
    #endregion

    #region Special Effects
    [SerializeField] private Vector2 deathKick = new Vector2(3f, 8f);
    #endregion

    #region Components & States
    private Collider2D myColliderCapsule;
    private Collider2D myColliderFeet;
    private Rigidbody2D myRigidBody;
    private Animator animator;
    private Vector2 moveInput;
    private bool isAlive = true;
    #endregion

    // STUPID STUFF THAT SHOULD BE REMOVED BUT I CANT DEAL WITH IT THIS SECONDS
    [SerializeField] PauseMenu pauseMenu;

    private void Start()
    {
        // Get the checkpoint system and ensure correct spawning position
        Checkpoint_Handler checkpointHandler = FindAnyObjectByType<Checkpoint_Handler>();
        if (checkpointHandler != null)
        {
            checkpointHandler.RegisterPlayer(this);
            transform.position = checkpointHandler.GetStartingLocation().position;
        }

        // Initialize components
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myColliderCapsule = GetComponent<Collider2D>();
        myColliderFeet = GetComponent<BoxCollider2D>();
            
        // Set initial movement parameters
        regularGravity = GRAVITYBASE;
        SetMovementParams();
    }

    private void Update()
    {
        if (!isAlive) return;
        if (FindAnyObjectByType<DialoguePlayer>().GetStatus() == false)
        {
            Run();
            FlipSprite();
            ClimbLadder();
        }

        TouchDangerCheck();
    }
    public void StopMovement()
    {
        moveInput = new Vector2(0,0);
        myRigidBody.linearVelocity = new Vector2(0,0);
        animator.SetBool("isRunning",false);
    }
    public void SetStartingLocation(Transform location)
    {
        transform.position = location.position;
    }

    public void Respawn()
    {
        Checkpoint_Handler checkpointHandler = FindAnyObjectByType<Checkpoint_Handler>();
        Transform checkpoint = checkpointHandler?.GetStartingLocation();
        
        Vector2 spawnPosition = checkpoint != null ? checkpoint.position : Vector2.zero;

        Debug.Log($"✅ Respawning at: {spawnPosition}");
        
        // Stop physics first
        myRigidBody.simulated = false; // Temporarily disable physics
        transform.position = spawnPosition; // Move player
        myRigidBody.simulated = true; // Re-enable physics

        //Stop movement
        moveInput = new Vector2(0,0);

        // Reset position
        transform.position = spawnPosition;
        myRigidBody.MovePosition(spawnPosition); // Ensures physics updates

        // Reset state
        isAlive = true;
        animator.SetBool("isDead", false);
    }



    #region Movement Methods
    private void Run()
    {
        
        Vector2 playerVelocity = new Vector2(moveInput.x * (Xspeed * speedMultiplier), myRigidBody.linearVelocity.y);
        myRigidBody.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.linearVelocity.x), 1f);
        }
    }

    private void ClimbLadder()
    {
        if (!myColliderCapsule.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            myRigidBody.gravityScale = regularGravity;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 playerVelocity = new Vector2(myRigidBody.linearVelocity.x, moveInput.y * climbSpeed * speedMultiplier);
        myRigidBody.linearVelocity = playerVelocity;
        animator.SetBool("isClimbing", true);
        myRigidBody.gravityScale = 0f;
    }
    #endregion

    #region Input Methods
    private void OnPause()
    {
        
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.TogglePause();
        }
        else
        {
            Debug.LogError("❌ PauseMenu instance is missing! Make sure it's in the scene.");
        }
    }

    private void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }
    private void OnJump(InputValue value)
    {
        if (value.isPressed && myColliderFeet.IsTouchingLayers(LayerMask.GetMask("Ground", "Climb", "Bouncing")) && FindAnyObjectByType<DialoguePlayer>().GetStatus() == false)
        {
            animator.SetBool("isJumping", true);
            myRigidBody.linearVelocity += new Vector2(0f, Yspeed);
        }

        //Sends signal to dialoguesystem
        if(FindAnyObjectByType<DialoguePlayer>().GetStatus())
        {
            FindAnyObjectByType<DialoguePlayer>().PlayerPressed();
        }
    }
    #endregion

    #region Collision Methods
    private void TouchDangerCheck()
    {
        if (myRigidBody.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            animator.SetBool("isDead", true);
            myRigidBody.linearVelocity = deathKick;
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        animator.SetBool("isJumping", false);
    }
  

    public void Bounce(float height)
    {
        myRigidBody.linearVelocity = new Vector2(myRigidBody.linearVelocity.x, height);
    }
    #endregion

    #region Temporary Movement Changes
    public void SetMovementParams(float x = XSPEEDBASE, float y = YSPEEDBASE, float climb = CLIMBSPEEDBASE, float multiplier = SPEEDMULTIPLIERBASE, float gravity = GRAVITYBASE)
    {
        Xspeed = x;
        Yspeed = y;
        speedMultiplier = multiplier;
        climbSpeed = climb;
        myRigidBody.gravityScale = gravity;
    }

    public IEnumerator TemporaryMovementChange(float x = XSPEEDBASE, float y = YSPEEDBASE, float climb = CLIMBSPEEDBASE, float multiplier = SPEEDMULTIPLIERBASE, float gravity = GRAVITYBASE, float duration = 0)
    {
        float originalX = Xspeed;
        float originalY = Yspeed;
        float originalClimb = climbSpeed;
        float originalMultiplier = speedMultiplier;
        float originalGravity = myRigidBody.gravityScale;

        SetMovementParams(x, y, climb, multiplier, gravity);
        regularGravity = gravity;
        Debug.Log("Temporary movement change applied!");

        yield return new WaitForSeconds(duration);

        SetMovementParams(originalX, originalY, originalClimb, originalMultiplier, originalGravity);
        regularGravity = originalGravity;
        Debug.Log("Movement reverted to original values.");
    }
    #endregion
}
