using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailShooter : Shooter
{
    private void Start()
    {
        existenceLimit1 = 8;
        existenceLimit2 = 16;
        existenceLimit3 = 16;
        shotInterval = 0.15f;

        shot = (GameObject) Resources.Load("Prefab/Tail");       
    }

    Quaternion rotation;
    public override void MakeShot(int level)
    {
        rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180));
        switch (level)
        {
            case 1:
                Instantiate(shot, transform.position, rotation).GetComponent<Tail>().Initialize(true);
                Instantiate(shot, transform.position, rotation).GetComponent<Tail>().Initialize(false);
                break;

            case 2:
                Instantiate(shot, transform.position, rotation);
                break;

            case 3:
                Instantiate(shot, transform.position, rotation);
                break;
        }
    }
}
