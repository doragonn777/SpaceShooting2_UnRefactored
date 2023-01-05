using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    protected Rigidbody2D rb;

    protected float damage;
    protected float speed;

    protected virtual void Start()
    {
        //親クラスでRigidBodyを取得するのは逆に分かりづらい説
        rb = this.GetComponent<Rigidbody2D>();
        MainWeaponManager.instance.AddShotNum();
    }

    public float GetDamage()
    {
        return damage;
    }

    public virtual void OnEnemyKill(GameObject killedEnemy)
    {
        Debug.Log(killedEnemy);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        MainWeaponManager.instance.SubtractShotNum();
    }

}
