using UnityEngine;

public class HeadBehavior : MonoBehaviour
{
    BoxCollider2D headCollider;
    void Start()
    {
        headCollider = GetComponent<BoxCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            FindAnyObjectByType<PlayerMovement>().Bounce(10);
            StartCoroutine(GetComponentInParent<WaspMovement>().Die());
        }
    }
    
}
