using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : Shooter
{
    Rigidbody2D rbShooter;

    private void Start()
    {
        existenceLimit1 = 1;
        existenceLimit2 = 1;
        existenceLimit3 = 1;
        shotInterval = 0.3f;

        rbShooter = GetComponent<Rigidbody2D>();
        shot = (GameObject)Resources.Load("Prefab/Laser");
    }

    public override void MakeShot(int level)
    {
        switch (level)
        {
            case 1:
                Instantiate(shot, transform.position, transform.rotation).GetComponent<Laser>().Initialize(rbShooter, level);
                break;

            case 2:
                Instantiate(shot, transform.position, transform.rotation).GetComponent<Laser>().Initialize(rbShooter, level);
                break;

            case 3:
                Instantiate(shot, transform.position, transform.rotation).GetComponent<Laser>().Initialize(rbShooter, level);
                break;
        }
    }
}
