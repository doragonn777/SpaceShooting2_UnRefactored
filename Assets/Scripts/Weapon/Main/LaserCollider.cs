using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollider : MonoBehaviour
{
    Laser laser;
    public void Initialize(Laser laser)
    {
        this.laser = laser;
    }

    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        laser.OnLaserTriggerStay2D(collision);  
    }
}
