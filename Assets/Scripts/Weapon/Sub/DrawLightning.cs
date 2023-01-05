using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLightning : MonoBehaviour
{
    List<Vector3> points;
    GameObject[] targets;
    float duration;
    float degreeRange;

    [SerializeField] Color color;
    LineRenderer lineRenderer;

    float lapseTime;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        points = new List<Vector3>();
        lapseTime = 0;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        Draw();
    }

    private void FixedUpdate()
    {
        lapseTime += Time.deltaTime;
        Draw();
        if(lapseTime > duration)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(GameObject origin, GameObject[] targets, float duration, float degreeRange)
    {
        this.targets = new GameObject[targets.Length + 1];
        this.targets[0] = origin;
        for(int i = 0; i < targets.Length; i++)
        {
            this.targets[i + 1] = targets[i];
        }
        this.duration = duration;
        this.degreeRange = degreeRange;
    }

    public void Draw()
    {
        int index = 0;
        points.Clear();
        for (int i = 0; i < targets.Length - 1; i++)
        {
            if (targets[i + 1] == null) break;
            index = GenerateNoisyPath(targets[i].transform.position, targets[i + 1].transform.position, index);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    //points[pointsIndex]にstart-endを繋ぐランダムパスを追加し、最後に追加された点のインデックスを返す
    private int GenerateNoisyPath(Vector3 start, Vector3 end, int pointsIndex)
    {
        if (pointsIndex == 0) points.Add(start);
        Vector3 initVector = end - start;
        Vector3 vec = initVector;
        float radius;
        float degree;

        for (int i = 0; true; i++)
        {
            if (vec.magnitude / initVector.magnitude < 0.1)
            {
                points.Add(end);
                return pointsIndex + i + 1;
            }
            if(i > 100)
            {
                return -1;
            }
            radius = vec.magnitude * Random.Range(0.05f, 1.0f - vec.magnitude / initVector.magnitude * 0.5f);
            degree = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg + Random.Range(-degreeRange, degreeRange);
            points.Add(points[pointsIndex + i] + PolarToCartesian(radius, degree));
            vec -= PolarToCartesian(radius, degree);
        }
    }

    public Vector3 GetRotatedVector(Vector2 vec, float rotation)
    {
        Vector2 temp;
        temp = new Vector2(vec.magnitude, Mathf.Atan2(vec.y, vec.x));
        temp.y += rotation * Mathf.Deg2Rad;
        vec.x = temp.x * Mathf.Cos(temp.y);
        vec.y = temp.x * Mathf.Sin(temp.y);
        return vec;
    }

    public Vector3 PolarToCartesian(float radius, float degree)
    {
        return new Vector3(radius * Mathf.Cos(degree * Mathf.Deg2Rad), radius * Mathf.Sin(degree * Mathf.Deg2Rad), 0);
    }

}