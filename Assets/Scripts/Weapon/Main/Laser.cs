using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : PlayerAttack
{
    //変数の定義が多い、FixedUpdateで何をしているのか分かりづらいような気がするので改善したい
    LineRenderer lineRenderer;
    float lifeTime;
    float hitInterval = 0.2f;
    bool canPierceAll;

    public void Initialize(Rigidbody2D rbShooter, int level)
    {
        lineRenderer = GetComponent<LineRenderer>();
        this.rbShooter = rbShooter;
        switch (level)
        {
            case 1:
                lineRenderer.startWidth = 0.3f;
                lineRenderer.endWidth = 0.3f;
                lifeTime = 0.3f;
                hitInterval = 0.2f;
                canPierceAll = false;
                break;
            case 2:
                lineRenderer.startWidth = 0.5f;
                lineRenderer.endWidth = 0.5f;
                lifeTime = 0.5f;
                hitInterval = 0.15f;
                canPierceAll = false;
                break;
            case 3:
                lineRenderer.startWidth = 0.8f;
                lineRenderer.endWidth = 0.8f;
                lifeTime = 0.8f;
                hitInterval = 0.1f;
                canPierceAll = true;
                break;
        }
    }

    Rigidbody2D rbShooter;
    GameObject laserCollider;
    PolygonCollider2D polygon;

    Vector2[] polygonPoints;
    Vector2[] points;
    Vector2 baseDirection;

    float lapseTime;
    bool isShootingStopped;

    protected override void Start()
    {
        base.Start();
        laserCollider = new GameObject("TrailCollider", typeof(PolygonCollider2D), typeof(LaserCollider));
        laserCollider.tag = "PlayerAttack";
        laserCollider.GetComponent<LaserCollider>().Initialize(this);
        polygon = laserCollider.GetComponent<PolygonCollider2D>();
        polygon.isTrigger = true;
        polygonPoints = new Vector2[4];

        lapseTime = 0;
        isShootingStopped = false;

        points = new Vector2[2];

        speed = 30f;
        damage = 1;

        baseDirection.Set(Mathf.Cos(rbShooter.rotation * Mathf.Deg2Rad), Mathf.Sin(rbShooter.rotation * Mathf.Deg2Rad));
        rb.velocity = speed * baseDirection + rbShooter.velocity;
    }

    RaycastHit2D hit;
    Vector2 laserLine;
    Vector2 start;
    private void FixedUpdate()
    {
        lapseTime += Time.deltaTime;
        if (lapseTime > lifeTime) StopShooting();

        //レーザーの先端位置の移動
        rb.velocity = speed * baseDirection;
        if (!isShootingStopped)
        {
            //向きが変えられる射手（オプション等）がレーザーを放つ場合を想定
            baseDirection.Set(Mathf.Cos(rbShooter.rotation * Mathf.Deg2Rad), Mathf.Sin(rbShooter.rotation * Mathf.Deg2Rad));
            rb.position = rbShooter.position + (rb.position - rbShooter.position).magnitude * baseDirection;
            
            //レーザー発射中はレーザーの開始地点を自機の位置に合わせる
            start = rbShooter.position;

            //自機の動きに合わせてレーザーを移動する
            rb.velocity += rbShooter.velocity;
        }

        StopMovingForwardIfOutOfCamera();

        laserLine = rb.position - start;

        //壁に当たるかどうかの判定
        if (!canPierceAll)
        {
            hit = Physics2D.Raycast(start, baseDirection, laserLine.magnitude);
            if (hit.collider != null)
            {
                rb.position = hit.point;
            }
        }
        
        //Lineの描画
        points[0] = start;
        points[1] = rb.position;
        
        if (isShootingStopped)
        {
            if (laserLine.magnitude <= speed * lapseTime) Destroy(gameObject);
            points[0] += speed * lapseTime * baseDirection;
        }

        lineRenderer.SetPosition(0, points[0]);
        lineRenderer.SetPosition(1, points[1]);

        SetColliderAlongLine();
    }

    private void StopMovingForwardIfOutOfCamera()
    {
        if (CamScroll.IsOutOfCamera(rb.position))
        {
            rb.velocity -= speed * baseDirection;
        }
        else
        {
            return;
        }

        Vector2 temp = rb.velocity;
        if (CamScroll.GetRelativePosX(rb.position.x) >= 1)
        {
            temp.x = Mathf.Min(temp.x, 0);
        } 
        else if (CamScroll.GetRelativePosX(rb.position.x) <= 0)
        {
            temp.x = Mathf.Max(temp.x, 0);
        }

        if (CamScroll.GetRelativePosY(rb.position.y) >= 1)
        {
            temp.y = Mathf.Min(temp.y, 0);
        } 
        else if (CamScroll.GetRelativePosY(rb.position.y) <= 0)
        {
            temp.y = Mathf.Max(temp.y, 0);
        }

        rb.velocity = temp;
    }
    private void SetColliderAlongLine()
    {
        Vector2 vertical = Vector2.Perpendicular(laserLine).normalized;

        polygonPoints[0] = (Vector2) lineRenderer.GetPosition(0) + lineRenderer.startWidth / 2 * vertical;
        polygonPoints[1] = (Vector2) lineRenderer.GetPosition(0) - lineRenderer.startWidth / 2 * vertical;
        polygonPoints[2] = (Vector2) lineRenderer.GetPosition(1) - lineRenderer.startWidth / 2 * vertical;
        polygonPoints[3] = (Vector2) lineRenderer.GetPosition(1) + lineRenderer.startWidth / 2 * vertical;
        polygon.SetPath(0, polygonPoints);
    }

    Dictionary<Collider2D, float> timeLaserHit = new();
    public void OnLaserTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!timeLaserHit.ContainsKey(collision)) timeLaserHit.Add(collision, 0);
            if (Time.time - timeLaserHit[collision] > hitInterval)
            {
                timeLaserHit[collision] = Time.time;
                collision.gameObject.GetComponent<Enemy>().Damage(damage);
            } 
        }
    }

    private void OnBecameInvisible()
    {

    }

    private void StopShooting()
    {
        if(!isShootingStopped)
        {
            lapseTime = 0;
            isShootingStopped = true;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Destroy(laserCollider);
    }
}
