using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]  float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f,10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField]GameSession session;
    float climbSpeed = 2f;
    Collider2D myCollider ;
    float startingGravity;
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator animator;
    Collider2D myFeetCollider;
    float speedMultiplier = 0.6f;
    float regularSpeedMultiplier = 0.6f;
    bool isAlive = true;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        



        startingGravity = myRigidBody.gravityScale;
        speedMultiplier = regularSpeedMultiplier;
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
        if(!myCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            myRigidBody.gravityScale = startingGravity;
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
        if(value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground","Climb")))
        {
            myRigidBody.linearVelocity += new Vector2(0f,jumpSpeed);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed*speedMultiplier,myRigidBody.linearVelocity.y);
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
