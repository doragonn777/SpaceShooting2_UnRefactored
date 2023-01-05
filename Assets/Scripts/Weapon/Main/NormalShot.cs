using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShot : PlayerAttack
{
    protected override void Start()
    {
        base.Start();      
        rb.velocity = 20f * new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad), Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
        damage = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
            Destroy(gameObject);
        }
    }


}
