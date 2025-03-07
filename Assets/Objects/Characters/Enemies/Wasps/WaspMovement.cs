using System.Collections;
#if UNITY_EDITOR
using UnityEditor.ShaderGraph.Internal;
#endif
using UnityEngine;
public class WaspMovement : MonoBehaviour
{
    [SerializeField] private float patrolDistance = 3f; // How far to patrol
    [SerializeField] private float speed = 2f; // Movement speed
    [SerializeField] private bool patrolHorizontally = true; // Toggle for horizontal/vertical movement
    private Collider2D[] colliders;

    private Vector2 startPos; // Starting position
    private Vector2 target; // Current target position
    private Animator animator;
    private Rigidbody2D myRigidBody;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        
        myRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x,transform.localScale.y);
    }

    private void Start()
    {
        if(!isDead)
        {
            startPos = transform.position; // Save start position

            // Set the first target position based on direction
            if (patrolHorizontally)
                target = startPos + new Vector2(patrolDistance, 0); // Move right first
            else
                target = startPos + new Vector2(0, patrolDistance); // Move up first
        }
    }

    private void Update()
    {
        // Move towards the target position
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // If we reach the target, swap between start position and patrol distance
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            target = (target == startPos) ? startPos + (patrolHorizontally ? new Vector2(patrolDistance, 0) : new Vector2(0, patrolDistance)) : startPos;
            Flip();
        }
    }

    public IEnumerator Die()
    {
        
        GetComponent<Collider2D>().enabled = false;
        foreach(Collider2D col in colliders)
        {
            col.enabled = false;
        }
        myRigidBody.gravityScale = 0.5f;
        
        animator.SetBool("isDead",true);
        yield return new WaitForSecondsRealtime(2f);
        
        Destroy(gameObject);
    }
}








