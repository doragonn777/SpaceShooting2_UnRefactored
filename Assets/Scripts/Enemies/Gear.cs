using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Enemy
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveManager.Move();
    }
    
}