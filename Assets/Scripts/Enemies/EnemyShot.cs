using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    Rigidbody2D rb;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        rb = this.transform.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed * Mathf.Cos(rb.rotation * Mathf.Deg2Rad), speed * Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.Translate(speed * Time.deltaTime, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hit!");
            Destroy(gameObject);
        }
    }
}
