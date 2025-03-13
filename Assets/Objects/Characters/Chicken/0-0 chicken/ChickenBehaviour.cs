using System.Collections;
using UnityEngine;

public class ChickenBehaviour : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    Animator animator;
    float moveSpeed = 2;
    bool keepMoving = true;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(MoveAndPeck());
    }

    IEnumerator Move(int duration)
    {
        Vector3 vector = myRigidBody.transform.localScale;
        animator.SetBool("isRunning", true);
        myRigidBody.linearVelocity = new Vector2(moveSpeed * vector.x, 0); // Changed linearVelocity to velocity
        
        yield return new WaitForSeconds(duration);

        animator.SetBool("isRunning", false);
        myRigidBody.linearVelocity = Vector2.zero; // Stop movement properly
    }

    void Flip()
    {
        float horizontalSpeed = myRigidBody.linearVelocity.x; // Fixed incorrect variable
        myRigidBody.transform.localScale = new Vector3(-myRigidBody.transform.localScale.x, 1, 1); // Flip properly
    }

    IEnumerator Peck(float duration)
    {
        animator.SetBool("isPecking", true);
        yield return new WaitForSeconds(duration);
        animator.SetBool("isPecking", false);
    }   

    IEnumerator MoveAndPeck()
    {
        while (keepMoving) // Keeps the chicken moving forever
        {
            yield return StartCoroutine(Move(1)); // ✅ Move first
            yield return StartCoroutine(Peck(1)); // ✅ Then peck
            Flip(); // ✅ Flip direction AFTER both actions complete
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            keepMoving = false;
            myRigidBody.transform.localScale = new Vector3(-1,1,1);
            myRigidBody.linearVelocity = new Vector2(10,3);
            StartCoroutine(destroySelfAfter(3));
            
        }
    }
    IEnumerator destroySelfAfter(int duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

        
    

}
