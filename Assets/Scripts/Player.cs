using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float lapseTime;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private MainWeaponManager weapon;

    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject shotPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector2(0, 0);
        rb = this.GetComponent<Rigidbody2D>();
        lapseTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        lapseTime += Time.deltaTime;
        rb.velocity = velocity + CamScroll.GetScrollVector();
    }

    public void SetVelocityX(float f)
    {
        velocity.x = f;
    }

    public void SetVelocityY(float f)
    {
        velocity.y = f;
    }

    public float GetSpeed()
    {
        return speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyAttack") || collision.CompareTag("Terrain"))
        {
            Debug.Log("Game Over");
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnBecameInvisible()
    {
        Debug.Log("player invisible");
    }

    private void OnBecameVisible()
    {
        Debug.Log("player visible");
    }
}
