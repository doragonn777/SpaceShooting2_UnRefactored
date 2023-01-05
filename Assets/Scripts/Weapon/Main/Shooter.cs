using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    protected GameObject shot;

    protected int existenceLimit1;
    protected int existenceLimit2;
    protected int existenceLimit3;

    protected float shotInterval = 0.15f;
    public abstract void MakeShot(int level);
    public int GetExistenceLimitOfShots(int level)
    {
        switch (level)
        {
            case 1:
                return existenceLimit1;

            case 2:
                return existenceLimit2;

            case 3:
                return existenceLimit3;

            default:
                return 0;
        }
    }
    public float GetShotInterval()
    {
        return shotInterval;
    }
}
