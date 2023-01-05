using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //各Moveクラスのphase数が高々3であると想定、不足分はresizeによって補う
    private const int AssumedMaxPhase = 3;

    //privateだとインスペクタ―上の設定(enemyType)が反映されない？
    //というよりも、enemyの値が保持されないと思われる
    //[SerializeField] privateまたはpublicだと問題無く動作するため、privateなGameObjectは毎回破棄されている？
    //→「シリアライズ可能でないすべてのデータは、スクリプトのリロード後に失われます」（マニュアル：スクリプトのシリアル化）
    [SerializeField][HideInInspector] private GameObject enemy;
    [SerializeField][HideInInspector] private Enemy.EnemyType enemyType;
    [SerializeField][HideInInspector] private SpawnPattern spawnPattern;
    [SerializeField][HideInInspector] private MoveManager.MovePattern movePattern;
    private enum LineDirection { Horizontal, Vertical }
    [SerializeField][HideInInspector] private LineDirection lineDirection;

    //SpawnPattern Options
    [SerializeField][HideInInspector] private int groupSize = 5;
    [SerializeField][HideInInspector] private float placeInterval = 1;
    [SerializeField][HideInInspector] private float arrowAngle = 60;
    private int enemyNum;

    public enum SpawnPattern { Line, Arrow }

    //MovePattern Options
    [SerializeField][HideInInspector] private float baseSpeed = 4.0f;
    [SerializeField][HideInInspector] private float rotateAngle = 180;
    [SerializeField][HideInInspector] private float amplitude = 2;
    [SerializeField][HideInInspector] private float cycle = 3;
    [SerializeField][HideInInspector] private float distanceUntilRotate1 = 12;
    [SerializeField][HideInInspector] private float distanceUntilRotate2 = 4;
    [SerializeField][HideInInspector] private float moveRadius = 4;
    [SerializeField][HideInInspector] private float requireTimeToRotate = 0.5f;
    [SerializeField][HideInInspector] private float timeUntilMoveForward = 5;

    //MovePattern Common Options
    [SerializeField][HideInInspector] private bool doesMoveAlongTerrain = false;
    [SerializeField][HideInInspector] private bool isSynchronized = false;
    [SerializeField][HideInInspector] private bool doesRotateToCenter = true;

    //Anchor関連
    private GameObject[] anchors;
    private int updatedPhase;
    private void Start()
    {
        this.transform.localScale = Vector3.one;
    }

    public void SetEnemy(GameObject enemy)
    {
        if (!enemy.CompareTag("Enemy"))
        {
            Debug.LogWarning("選択されたオブジェクトがEnemyではありません");
            return;
        }
        this.enemy = enemy;
        this.GetComponent<SpriteRenderer>().sprite = this.enemy.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnBecameVisible()
    {
        Spawn();
    }

    
    public Vector2 GetAnchorPos(int phase)
    {

        return anchors[phase].transform.position;
    }

    public void UpdateAnchorPos(Vector2 pos, int phase)
    {
        if (phase <= updatedPhase) return;
        if (phase >= anchors.Length)
        {
            Debug.LogWarning("phase数が想定された値を超過しています");
            System.Array.Resize(ref anchors, anchors.Length + 1);
            anchors[phase] = new GameObject();
            if (doesMoveAlongTerrain) anchors[phase].transform.parent = anchors[0].transform.parent;
        }
        anchors[phase].transform.position = pos;
    }

    public void Spawn()
    {
        if (!isSynchronized)
        {
            anchors = new GameObject[AssumedMaxPhase];
            for (int i = 0; i < AssumedMaxPhase; i++)
            {
                anchors[i] = new GameObject();
                if (doesMoveAlongTerrain) anchors[i].transform.parent = this.transform.parent;
            }
            
        }
        
        switch (spawnPattern)
        {
            case SpawnPattern.Line:
                SpawnLine();
                break;

            case SpawnPattern.Arrow:
                SpawnArrow();
                break;

            default:
                break;
        }
        this.transform.parent = null;
    }

    public int GetEnemyNum()
    {
        return this.enemyNum;
    }

    public void OnEnemyKilled()
    {
        if (enemyNum == 1)
        {
            Destroy(gameObject);
        }
        enemyNum--;
    }

    private void SpawnLine()
    {
        float rotation = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector3 vec = new Vector3();
        this.GetComponent<SpriteRenderer>().enabled = false;
        enemyNum = groupSize;
        for (int i = 0; i < enemyNum; i++)
        {
            vec.Set(Mathf.Cos(rotation), Mathf.Sin(rotation), 0);
            if (lineDirection == LineDirection.Horizontal)
            {
                SpawnEnemy(-i * placeInterval * vec, Vector3.zero);
            }
            else if(lineDirection == LineDirection.Vertical)
            {
                vec = Vector2.Perpendicular(vec).normalized;
                vec *= Mathf.Pow(-1, i);
                SpawnEnemy(-(i + i % 2) / 2 * placeInterval * vec, Vector3.zero);             
            }
        }
    }

    private void SpawnArrow()
    {
        Vector3 vec = new();
        this.transform.localScale = Vector3.one;
        this.GetComponent<SpriteRenderer>().enabled = false;
        float rotation = this.transform.eulerAngles.z * Mathf.Deg2Rad;
        enemyNum = 2 * groupSize - 1;
        arrowAngle -= 180;
        for (int i = 0; i < enemyNum; i++)
        {
            if (i == 0)
            {
                SpawnEnemy(Vector3.zero, Vector3.zero);
            }
            else
            {
                vec.Set(Mathf.Cos(rotation + arrowAngle * Mathf.Deg2Rad * Mathf.Pow(-1, i)), 
                        Mathf.Sin(rotation + arrowAngle * Mathf.Deg2Rad * Mathf.Pow(-1, i)), 0);
                SpawnEnemy((i + i % 2) / 2 * placeInterval * vec.normalized, Vector3.zero);
            }
        }
    }

    private GameObject SpawnEnemy(Vector3 posOffset, Vector3 rotOffset)
    {
        GameObject result;
        
        result = Instantiate(enemy, transform.position + posOffset, Quaternion.Euler(transform.rotation.eulerAngles + rotOffset), transform);
        
        MoveManager moveManager = SetMoveManager(result);
        GiveAnchorToMoveManager(moveManager);

        return result;
    }

    private MoveManager SetMoveManager(GameObject enemy)
    {
        //case毎にMoveManagerを継承するクラス型の変数を定義した方が、AdaptOptionsの使用法が分かりやすい？
        //MoveManager型で定義すると記述の冗長性は改善されるが、AdaptOptionsの引数が何を示すのか分からない。
        //キャストすることで解決した
        MoveManager moveManager;
        
        switch (movePattern)
        {
            case MoveManager.MovePattern.Curve:
                moveManager = enemy.AddComponent<MoveCurve>();
                ((MoveCurve)moveManager).AdaptOptions(distanceUntilRotate1, requireTimeToRotate, rotateAngle, doesRotateToCenter);
                break;

            case MoveManager.MovePattern.SineCurve:
                moveManager = enemy.AddComponent<MoveSineCurve>();
                ((MoveSineCurve)moveManager).AdaptOptions(amplitude, cycle);
                break;

            case MoveManager.MovePattern.Z:
                moveManager = enemy.AddComponent<MoveZ>();
                ((MoveZ)moveManager).AdaptOptions(rotateAngle, distanceUntilRotate1, distanceUntilRotate2, doesRotateToCenter);
                break;

            case MoveManager.MovePattern.RoundTripAtEdge:
                moveManager = enemy.AddComponent<MoveRoundTripAtEdge>();
                ((MoveRoundTripAtEdge)moveManager).AdaptOptions(moveRadius, timeUntilMoveForward);
                break;

            default:
                //Error
                moveManager = enemy.AddComponent<MoveManager>();
                Debug.LogWarning("MovePattern" + movePattern + "does not exist.");
                break;
        }
        moveManager.SetCommonOptions(baseSpeed, doesMoveAlongTerrain, isSynchronized);
        return moveManager;
    }

    private void GiveAnchorToMoveManager(MoveManager moveManager)
    {
        if (isSynchronized)
        {
            GameObject anchor = new();
            anchor.transform.position = moveManager.transform.position;
            if (doesMoveAlongTerrain) anchor.transform.parent = transform.parent;
            moveManager.SetAnchor(anchor);
        }
        else
        {
            anchors[0].transform.position = transform.position;
            updatedPhase = 0;
            moveManager.SetAnchor(anchors[0]);
        }
    }
}
