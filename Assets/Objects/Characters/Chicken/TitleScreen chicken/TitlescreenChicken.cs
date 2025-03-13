using System.Collections;
using UnityEngine;

public class TitlescreenChicken : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    Animator animator;
    float moveSpeed = 2;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

           StartCoroutine(MoveAndPeckAndRest(1)); 
        
        
    }

    IEnumerator Sit(int duration)
    {
        animator.SetBool("isSitting",true);
        myRigidBody.linearVelocity = new Vector2(0,0);
        yield return new WaitForSeconds(duration);
        animator.SetBool("isSitting",false);
    }
    IEnumerator Move(int duration)
    {
        animator.SetBool("isRunning", true);
        Vector3 vector = myRigidBody.transform.localScale;
        
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

    //Move back and worth pecking on each turn. after duration turns it lays down for set amount of time then repeats
    IEnumerator MoveAndPeckAndRest(int duration)
    {
        while(true)
        {
            for(int i = 0 ;i < 3;i++)
            {
                yield return StartCoroutine(Move(duration)); 
                yield return StartCoroutine(Peck(duration)); 
                Flip(); 
            }

                yield return StartCoroutine(Sit(4));
            
        }
    }
}
