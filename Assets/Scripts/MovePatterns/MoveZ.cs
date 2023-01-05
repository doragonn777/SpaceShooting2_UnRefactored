using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZ : MoveManager
{
    int curveDirection;
    float rotateAngle;
    float distanceUntilFirstRotate;
    float distanceUntilSecondRotate;
    bool doesRotateToCenter;

    protected override void Start()
    {
        base.Start();

        curveDirection = 1;
        //doesRotateToCenterはEnemySpawnerクラスでAdaptOptions()によって定義されるが、MoveZクラスがアタッチされてこのStart()が呼び出されるのと、
        //アタッチ後にAdaptOptions()が呼び出されるのは後者が先であることが分かった。しかし、必ずしもこの順番で呼び出されるのか不明。
        //多分並列で処理しているので、順番が前後してしまう可能性があるのではないか？その場合この設定は危うい？
        if (doesRotateToCenter)
        {
            rotateAngle = Mathf.Abs(rotateAngle);
            if (CamScroll.GetRelativePosY(this.transform.position.y) < 0.5)
            {
                curveDirection = -1;
            }
            else
            {
                curveDirection = 1;
            }
        }
        SetVelocity(baseSpeed * transform.right);
    }

    public override void AdaptOptions(float rotateAngle, float distanceUntilFirstRotate, float distanceUntilSecondRotate, bool doesRotateToCenter)
    {
        this.rotateAngle = rotateAngle;
        this.distanceUntilFirstRotate = distanceUntilFirstRotate;
        this.distanceUntilSecondRotate = distanceUntilSecondRotate;
        this.doesRotateToCenter = doesRotateToCenter;
    }

    public override void Move()
    {
        lapseTime += Time.deltaTime;
        //if (visibleOnce) traveledDistance += Mathf.Abs(rb.velocity.magnitude) * Time.deltaTime;

        switch (phase)
        {
            case 0:
                if (traveledDistance > distanceUntilFirstRotate)
                {
                    rb.rotation += curveDirection * rotateAngle;
                    NextPhase();
                }
                break;
            case 1:
                if (traveledDistance > distanceUntilSecondRotate)
                {
                    rb.rotation -= curveDirection * rotateAngle;
                    NextPhase();
                }
                break;
            case 2:
                break;
            default:
                break;
        }
        SetVelocity(baseSpeed * transform.right);
    }
}
