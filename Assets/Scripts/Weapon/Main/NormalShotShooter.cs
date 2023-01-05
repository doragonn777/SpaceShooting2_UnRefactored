using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShotShooter : Shooter
{

    private void Start()
    {
        existenceLimit1 = 4;
        existenceLimit2 = 8;
        existenceLimit3 = 16;
        shotInterval = 0.15f;

        shot = (GameObject)Resources.Load("Prefab/NormalShot");
    }

    public override void MakeShot(int level)
    {
        switch (level)
        {
            case 1:
                Instantiate(shot, transform.position, transform.rotation);
                break;

            case 2:
                Instantiate(shot, transform.position + Vector3.up * 0.2f, transform.rotation);
                Instantiate(shot, transform.position + Vector3.down * 0.2f, transform.rotation);
                break;

            case 3:
                Instantiate(shot, transform.position + Vector3.up * 0.1f, transform.rotation);
                Instantiate(shot, transform.position + Vector3.down * 0.1f, transform.rotation);
                Instantiate(shot, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward * 20));
                Instantiate(shot, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward * -20));
                break;
        }
    }
}
