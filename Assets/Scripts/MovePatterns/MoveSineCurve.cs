using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSineCurve : MoveManager
{
    private float amplitude = 2f;
    private float cycle;

    public override void AdaptOptions(float amplitude, float cycle)
    {
        this.amplitude = amplitude;
        this.cycle = cycle;
    }

    public override void Move()
    {
        lapseTime += Time.deltaTime;
        SetVelocity(
            (Vector2) transform.right * baseSpeed +
            (Vector2) transform.up * (amplitude * 2 * Mathf.PI / cycle * Mathf.Cos(2 * Mathf.PI * lapseTime / cycle))
            );
    }
}
