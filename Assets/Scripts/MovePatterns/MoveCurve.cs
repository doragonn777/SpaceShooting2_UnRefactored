using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCurve : MoveManager
{
    private float initRotation;

    private float distanceUntilRotate;
    private float requireTimeToRotate;
    private float rotateAngle;

    private bool doesRotateCenter;
    private int curveDirection;

    protected override void Start()
    {
        base.Start();
        initRotation = rb.rotation;
        
        curveDirection = 1;

        if (doesRotateCenter)
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
    }

    public override void AdaptOptions(float distanceUntilRotate, float requireTimeToRotate, float rotateAngle, bool doesRotateCenter)
    {
        this.distanceUntilRotate = distanceUntilRotate;
        this.requireTimeToRotate = requireTimeToRotate;
        this.rotateAngle = rotateAngle;
        this.doesRotateCenter = doesRotateCenter;
    }

    public override void Move()
    {
        lapseTime += Time.deltaTime;
        SetVelocity(baseSpeed * transform.right);
        switch (phase)
        {
            case 0:
                if (traveledDistance > distanceUntilRotate)
                {
                    Instantiate(enemyShot, this.transform.position,
                    Quaternion.Euler(new Vector3(0, 0, GetDegreeToObject(player) + Random.Range(-25f, 25f))));
                    lapseTime = 0;
                    NextPhase();
                }
                break;
            case 1:
                rb.rotation = initRotation + curveDirection * Mathf.Lerp(0, rotateAngle, lapseTime / requireTimeToRotate);
                if (lapseTime >= requireTimeToRotate) NextPhase();
                break;
            case 2:
                break;
            default:
                break;
        }
    }
}
