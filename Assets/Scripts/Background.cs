using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnBecameInvisible()
    {
        this.transform.Translate(38f, 0, 0);
    }

    private void FixedUpdate()
    {
        this.transform.Translate(-1f * Time.deltaTime, 0, 0);
    }
}
