using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLightning : MonoBehaviour
{
    private int maxChain;
    private float maxRange;
    private float lapseTime;
    private const float coolDown = 1f;
    [SerializeField] GameObject lightning;
    // Start is called before the first frame update
    void Start()
    {
        maxChain = 10;
        maxRange = 5;
        lapseTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lapseTime += Time.deltaTime;
    }

    public void shootLightning()
    {
        if (lapseTime < coolDown) return;
        lapseTime = 0;
        GameObject[] targets = new GameObject[maxChain];
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        targets[0] = FindClosestEnemy(gameObject, enemies.ToArray(), maxRange);
        if (targets[0] == null) return;
        enemies.Remove(targets[0]);
        for (int i = 1; i < maxChain; i++)
        {
            targets[i] = FindClosestEnemy(targets[i-1], enemies.ToArray(), maxRange);
            if (targets[i] == null) break;
            enemies.Remove(targets[i]);
        }
        Instantiate(lightning).GetComponent<DrawLightning>().Initialize(gameObject, targets, 0.5f, 30);
    }

    GameObject FindClosestEnemy(GameObject standard, GameObject[] enemies, float range)
    {
        float distance;
        Vector3 pos = standard.transform.position;
        GameObject closestEnemy = null;
        foreach (GameObject target in enemies)
        {
            if (target == standard) continue;
            distance = Vector3.Distance(pos, target.transform.position);
            if (distance < range)
            {
                range = distance;
                closestEnemy = target;
            }
        }
        return closestEnemy;
    }
}
