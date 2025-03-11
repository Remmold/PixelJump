using System.Collections;
using UnityEngine;

public class ChickenBehaviour : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    Animator animator;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(MoveAndPeck());
    }
    IEnumerator MoveAndPeck()
    {
        animator.SetBool("isPecking",false);
        animator.SetBool("isRunning",true);
        myRigidBody.linearVelocity = new Vector2(2,0);
        myRigidBody.transform.localScale = new Vector3(1,1,0);
        yield
        return new WaitForSeconds(2);
        myRigidBody.linearVelocity = new Vector2(0,0);
        animator.SetBool("isRunning",false);
        animator.SetBool("isPecking",true);
        yield
        return new WaitForSeconds(2);
        animator.SetBool("isPecking",false);
        animator.SetBool("isRunning",true);
        myRigidBody.linearVelocity = new Vector2(-2,0);
        myRigidBody.transform.localScale = new Vector3(-1,1,0);
        yield
        return new WaitForSeconds(2);
        myRigidBody.linearVelocity = new Vector2(0,0);
        animator.SetBool("isRunning",false);
        animator.SetBool("isPecking",true);
        yield
        return new WaitForSeconds(2);
        StartCoroutine(MoveAndPeck());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
