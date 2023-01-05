using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoundTripAtEdge : MoveManager
{
    private float moveRadius;
    private float timeUntilMoveForward;

    private float shotInterval;

    protected override void Start()
    {
        base.Start();
        doesMeasureOnlyFacing = false;
        shotInterval = 0;
    }

    public override void AdaptOptions(float moveRadius, float timeUntilMoveForward)
    {
        this.moveRadius = moveRadius;
        this.timeUntilMoveForward = timeUntilMoveForward;
    }

    public override void Move()
    {
        lapseTime += Time.deltaTime;
        switch (phase)
        {
            case 0:
                SetVelocity(baseSpeed * transform.right);
                if (traveledDistance > 3)
                {
                    SetVelocity(baseSpeed * transform.up);
                    NextPhase();
                }
                break;
            case 1:
            case 2:
                shotInterval += Time.deltaTime;
                if (shotInterval >= 0.75)
                {
                    Instantiate(enemyShot, this.transform.position, this.transform.rotation);
                    shotInterval = 0;
                }
                if (phase == 1 && traveledDistance > moveRadius)
                {
                    rb.velocity *= -1;
                    traveledDistance = 0;
                    NextPhase();
                }
                if (traveledDistance > moveRadius * 2)
                {
                    SetAnchorPos();
                    rb.velocity *= -1;
                    traveledDistance = 0;
                }
                if (lapseTime > timeUntilMoveForward)
                {
                    SetVelocity(baseSpeed * transform.right);
                    NextPhase();
                }
                break;
            default:
                break;
        }
    }
}
