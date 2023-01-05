using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerTest : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 velocity;
    private Vector3 norm;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            velocity.y = Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity.y = -Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity.x = Time.deltaTime;
        }
        else
        {
            velocity.x = 0;
        }
        this.transform.Translate(velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        norm = collision.contacts[0].normal;
        Debug.Log(Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg);
        
    }
}
