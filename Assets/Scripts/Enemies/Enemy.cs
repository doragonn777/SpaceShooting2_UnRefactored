using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected bool visibleOnce;
    
    private EnemySpawner spawner;
    protected MoveManager moveManager;

    [SerializeField] float hp;
    [SerializeField] GameObject powerUp;

    public enum EnemyType
    {
        Gear, Blue, Big
    }

    protected virtual void Start()
    {
        visibleOnce = false;
        spawner = transform.parent.gameObject.GetComponent<EnemySpawner>();
        moveManager = this.GetComponent<MoveManager>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall") && visibleOnce)
        {
            Destroy(gameObject);
            return;
        }
        if (collision.CompareTag("PlayerAttack"))
        {
            OnHit();
        }
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }

    public void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            OnDeath();
        }
    }

    protected virtual void OnBecameVisible()
    {
        visibleOnce = true;
    }
    protected virtual void OnHit()
    {
        SoundManager.Instance.PlaySE(0);
    }

    protected virtual void OnDeath()
    {
        if (spawner.GetEnemyNum() == 1)
        {
            Instantiate(powerUp, this.transform.position, Quaternion.Euler(0, 0, 0));
        }
        spawner.OnEnemyKilled();
        Destroy(gameObject);
    }

    public float GetHp()
    {
        return hp;
    }
}
