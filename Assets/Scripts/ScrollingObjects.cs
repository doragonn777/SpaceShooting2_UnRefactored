using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObjects : MonoBehaviour
{
    private static Rigidbody2D rb;
    [SerializeField] private Vector2 scrollVector;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        scrollVector = new Vector2(-1, 0);
        rb.velocity = new Vector2(-1, 0);
    }

    public static Vector2 GetScrollVelocity()
    {
        return rb.velocity;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = scrollVector;
    }
}
