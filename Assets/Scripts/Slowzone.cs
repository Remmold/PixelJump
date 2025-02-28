using UnityEngine;

public class Slowzone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            FindAnyObjectByType<GameSession>().AlterTime(-0.05f);
            FindAnyObjectByType<PlayerMovement>().ChangeMovespeed(-0.05f);
        }
    }
}
