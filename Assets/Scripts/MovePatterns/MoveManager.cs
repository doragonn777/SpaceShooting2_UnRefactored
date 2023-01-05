using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    protected float baseSpeed = 4.0f;
    protected float speedRatio = 1.0f;

    protected int phase = 0;
    protected float lapseTime = 0;
    protected float traveledDistance;
    protected bool visibleOnce = false;

    //Common Options
    protected bool doesMoveAlongTerrain;
    protected bool doesMeasureOnlyFacing = true;
    protected bool isSynchronized;

    //必ずしも使用するとは限らない変数、子クラスごとに定義すべき？
    //インスタンス毎にenemyShot等をロードしなくて済むようにしたい→シーン上のオブジェクトにアタッチして参照させる？
    protected GameObject enemyShot;
    protected GameObject player;
    protected Rigidbody2D rb;

    public enum MovePattern
    {
        Curve, SineCurve, Z, RoundTripAtEdge
    }

    int id;
    GameObject anchor;
    EnemySpawner spawner;
    protected virtual void Start()
    {
        phase = 0;
        lapseTime = 0;
        traveledDistance = 0;
        enemyShot = (GameObject)Resources.Load("Prefab/EnemyShot");
        player = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody2D>();
        spawner = transform.parent.GetComponent<EnemySpawner>();

        
        //if (doesMoveAlongTerrain) traveledDistance += baseSpeed * Time.deltaTime;
    }

    /*
    エディタとゲーム画面での位置ズレについて、上手く説明できないが、敵が生成されてから動き始めるまでの
    1，2フレームの差が現れてしまっていると思われる。FixedUpdateでrb.velocity * Time.deltaTimeのように
    実際に移動したかどうかに係わらず移動距離を加算していたり、ScrollingObjectsの移動に合わせた移動にズレがあったり
    前者は誤差レベル（ただし移動速度が大きくなるほど差が大きくなる）
    */
    protected virtual void FixedUpdate()
    {
        Vector2 vector = Vector2.zero;
        vector += rb.position - GetAnchorPos();

        if (doesMeasureOnlyFacing)
        {
            traveledDistance = Vector2.Dot(vector, transform.right);
        }
        else
        {
            traveledDistance = vector.magnitude;
        }
    }

    public virtual void Move()
    {

    }

    public void SetId(int id)
    {
        this.id = id;
    }

    /*
     * Anchorに関する説明
     * isSynchronizedがtrueのとき、各MoveManagerにanchorを付与し、それぞれでanchorを管理する
     * isSynchronizedがfalseのとき、親のEnemySpawnerがphaseごとのanchorを一括管理する
     * この時、敵は各自のphaseに応じたanchorの座標にアクセスし、traveledDistanceを計算する
     * 最初に目標地点に到達した機体が次phaseのanchorの位置を記録し、それ以降の敵はそれを基準に行動する
    */
    //関数の入れ子構造？みたいになっていて分かりづらいのでできれば改善したい
    public void SetAnchor(GameObject anchor)
    {
        this.anchor = anchor;
    }

    protected void SetAnchorPos()
    {
        if (isSynchronized)
        {
            anchor.transform.position = rb.position;
        }
        else
        {
            spawner.UpdateAnchorPos(rb.position, phase);
        }
    }

    protected Vector2 GetAnchorPos()
    {
        if (isSynchronized)
        {
            return anchor.transform.position;
        }
        else
        {
            return spawner.GetAnchorPos(phase);
        }
    }

    //transform.rightとほぼ同じ
    protected Vector2 GetFacingVector()
    {
        return new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad), Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
    }

    //Vector2.Perpendicularで代用可

    protected Vector2 GetVerticalVector()
    {
        return new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad - Mathf.PI / 2), Mathf.Sin(rb.rotation * Mathf.Deg2Rad - Mathf.PI / 2));
    }

    protected void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
        if (doesMoveAlongTerrain) rb.velocity += ScrollingObjects.GetScrollVelocity();  
    }


    protected void NextPhase()
    {
        phase++;
        SetAnchorPos();
    }

    public void SetCommonOptions(float baseSpeed, bool doesMoveAlongTerrain, bool isSynchronized)
    {
        this.baseSpeed = baseSpeed;
        this.doesMoveAlongTerrain = doesMoveAlongTerrain;
        this.isSynchronized = isSynchronized;
    }
    public virtual void AdaptOptions(float floatOption1, float floatOption2,
                                     float floatOption3, bool boolOption1)
    {

    }
    public virtual void AdaptOptions(float floatOption1, float floatOption2)
    {

    }
    public virtual void AdaptOptions(float floatOption1, bool boolOption1)
    {

    }

    public void SetBaseSpeed(float baseSpeed)
    {
        this.baseSpeed = baseSpeed;
    }

    public void ChangeSpeedRatio(float speedRatio)
    {
        this.speedRatio = speedRatio;
    }


    protected float GetDegreeToObject(GameObject obj) 
    {
        if (obj != null)
        {
            Vector2 temp = obj.transform.position - this.transform.position;
            return Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
        }
        return 180;
    }

    private void OnBecameVisible()
    {
        visibleOnce = true;
    }
}
