using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    SpriteRenderer myRenderer;
    BoxCollider2D reversePeriscope ;
    Rigidbody2D myRigidBody;
    [SerializeField] float flipCooldown = 0.2f; // Small delay to prevent flipping too fast
    private float lastFlipTime = 0f; // Time tracking for cooldown
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        reversePeriscope = GetComponent<BoxCollider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myRigidBody.linearVelocity = new Vector2(moveSpeed, 0);
    }


    void Update()
    {
        myRigidBody.linearVelocity = new Vector2(moveSpeed, 0);
        
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (Time.time - lastFlipTime > flipCooldown) // Only flip if cooldown has passed
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
            lastFlipTime = Time.time; // Update last flip time
        }
    }
    void OnTriggerEnter2D(Collider2D other) // Flips on entering instead of exiting
    {
        if (Time.time - lastFlipTime > flipCooldown)
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
            lastFlipTime = Time.time; // Update last flip time
        }
    }
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.linearVelocity.x)),1f);
    }
}
