using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 7f;
    Rigidbody2D myRigidBody;
    PlayerMovement player;
    float xSpeed;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x*bulletSpeed;
    }
    void Update()
    {
        myRigidBody.linearVelocity = new Vector2 (xSpeed,0f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);  
    }


}
