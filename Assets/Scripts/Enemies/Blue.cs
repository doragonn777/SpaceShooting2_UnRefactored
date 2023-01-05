using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : Enemy
{
    [SerializeField] GameObject enemyShot;

    protected override void Start()
    {
        base.Start();
        moveManager = this.GetComponent<MoveManager>();
    }

    void FixedUpdate()
    {
        moveManager.Move();
    }

}
