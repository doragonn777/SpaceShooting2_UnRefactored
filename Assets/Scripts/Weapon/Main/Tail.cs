using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : PlayerAttack
{
    const float rotateTime = 0.1f;
    bool isTailDirectionUp;
    public void Initialize(bool isTailDirectionUp)
    {
        this.isTailDirectionUp = isTailDirectionUp;
    }

    float initRotation;
    float lapseTime;
    protected override void Start()
    {
        base.Start();
        initRotation = rb.rotation;
        lapseTime = 0;
        speed = 20;
        damage = 1;
    }
    private void FixedUpdate()
    {
        lapseTime += Time.deltaTime;
        rb.velocity = speed * transform.right;
        if (isTailDirectionUp)
        {
            rb.rotation = Mathf.Lerp(initRotation, initRotation - 180, lapseTime / rotateTime);
        }
        else
        {
            rb.rotation = Mathf.Lerp(initRotation, initRotation + 180, lapseTime / rotateTime);
        }

        //rb.rotation = initRotation + curveDirection * Mathf.Lerp(0, rotateAngle, lapseTime / requireTimeToRotate);
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
