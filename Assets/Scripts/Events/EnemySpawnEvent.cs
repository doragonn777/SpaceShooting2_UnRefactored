using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnBecameVisible()
    {
        Transform child;
        this.GetComponent<SpriteRenderer>().enabled = false;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            child = gameObject.transform.GetChild(i);
            
            if (child.CompareTag("EnemySpawner"))
            {
                child.GetComponent<EnemySpawner>().Spawn();
            }   
        }
    }
}
