using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    EnemySpawner spawner;

    //enum
    private SerializedProperty enemyType;
    private SerializedProperty spawnPattern;
    private SerializedProperty movePattern;
    private SerializedProperty lineDirection;

    //int common options
    private SerializedProperty groupSize;


    //float common options
    private SerializedProperty baseSpeed;
    private SerializedProperty placeInterval;

    //float unique options
    private SerializedProperty arrowAngle;
    private SerializedProperty rotateAngle;
    private SerializedProperty amplitude;
    private SerializedProperty cycle;
    private SerializedProperty distanceUntilRotate1;
    private SerializedProperty distanceUntilRotate2;
    private SerializedProperty moveRadius;
    private SerializedProperty requireTimeToRotate;
    private SerializedProperty timeUntilMoveForward;

    //bool common options
    private SerializedProperty doesMoveAlongTerrain;
    private SerializedProperty isSynchronized;

    //bool unique options
    private SerializedProperty doesRotateToCenter;



    private void OnEnable()
    {
        spawner = target as EnemySpawner;

        enemyType = serializedObject.FindProperty("enemyType");
        spawnPattern = serializedObject.FindProperty("spawnPattern");
        movePattern = serializedObject.FindProperty("movePattern");
        lineDirection = serializedObject.FindProperty("lineDirection");


        groupSize = serializedObject.FindProperty("groupSize");


        baseSpeed = serializedObject.FindProperty("baseSpeed");
        placeInterval = serializedObject.FindProperty("placeInterval");

        rotateAngle = serializedObject.FindProperty("rotateAngle");
        amplitude = serializedObject.FindProperty("amplitude");
        cycle = serializedObject.FindProperty("cycle");
        distanceUntilRotate1 = serializedObject.FindProperty("distanceUntilRotate1");
        distanceUntilRotate2 = serializedObject.FindProperty("distanceUntilRotate2");
        moveRadius = serializedObject.FindProperty("moveRadius");
        requireTimeToRotate = serializedObject.FindProperty("requireTimeToRotate");
        timeUntilMoveForward = serializedObject.FindProperty("timeUntilMoveForward");

        doesMoveAlongTerrain = serializedObject.FindProperty("doesMoveAlongTerrain");
        isSynchronized = serializedObject.FindProperty("isSynchronized");
        doesRotateToCenter = serializedObject.FindProperty("doesRotateToCenter");
    }

    private bool commonOptionsFold;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        commonOptionsFold = EditorGUILayout.Foldout(commonOptionsFold, "CommonOptions");
        if (commonOptionsFold)
        {
            using (new EditorGUI.IndentLevelScope(1))
            {
                EditorGUILayout.PropertyField(groupSize, new GUIContent("敵団のサイズ"));
                EditorGUILayout.PropertyField(placeInterval, new GUIContent("PlaceInterval", "敵の配置間隔を設定します"));
                EditorGUILayout.PropertyField(baseSpeed, new GUIContent("BaseSpeed", "進行方向への速度を設定します"));
                EditorGUILayout.PropertyField(doesMoveAlongTerrain, new GUIContent("MoveAlongTerrain", "画面内の敵がスクロールに合わせて移動し、エディタ上の距離感に合わせて動きます"));
                EditorGUILayout.PropertyField(isSynchronized, new GUIContent("Synchronize", "グループ内の敵が先頭と同じタイミングで動くようになります"));

            }
        }

        EditorGUILayout.PropertyField(enemyType, new GUIContent("Enemy"));
        switch (enemyType.enumNames[enemyType.enumValueIndex])
        {
            case "Gear":
                spawner.SetEnemy((GameObject)Resources.Load("Prefab/Gear"));
                break;

            case "Blue":
                spawner.SetEnemy((GameObject)Resources.Load("Prefab/Blue"));
                break;

            case "Big":
                spawner.SetEnemy((GameObject)Resources.Load("Prefab/Big"));
                break;

            default:
                break;
        }

        

        EditorGUILayout.PropertyField(spawnPattern, new GUIContent("SpawnPattern"));
        using (new EditorGUI.IndentLevelScope(1)) 
        {
            switch (spawnPattern.enumNames[spawnPattern.enumValueIndex])
            {
                case "Line":
                    EditorGUILayout.PropertyField(lineDirection, new GUIContent("LineDirection", "敵の進行方向に対して水平または垂直方向に配置します"));
                    break;
                case "Arrow":
                    EditorGUILayout.PropertyField(arrowAngle, new GUIContent("ArrowAngle", "0~180°で矢の形の開き具合を調整します"));
                    break;
            }
        }
        

         
        EditorGUILayout.PropertyField(movePattern, new GUIContent("MovePattern"));
        using (new EditorGUI.IndentLevelScope(1))
        {
            switch (movePattern.enumNames[movePattern.enumValueIndex])
            {
                case "Curve":
                    EditorGUILayout.PropertyField(distanceUntilRotate1, new GUIContent("Distance", "方向転換までに進む距離を指定します"));
                    EditorGUILayout.PropertyField(requireTimeToRotate, new GUIContent("RotateTime", "方向転換にかける時間を指定します"));
                    RotateAngleGUI();
                    break;

                case "SineCurve":
                    EditorGUILayout.PropertyField(amplitude, new GUIContent("Amplitude", "三角関数による移動の幅を指定します"));
                    EditorGUILayout.PropertyField(cycle, new GUIContent("Cycle", "三角関数による移動の周期を指定します"));
                    break;

                case "Z":
                    EditorGUILayout.PropertyField(distanceUntilRotate1, new GUIContent("Distance1", "方向転換までに進む距離を指定します"));
                    EditorGUILayout.PropertyField(distanceUntilRotate2, new GUIContent("Distance2", "方向転換までに進む距離を指定します"));
                    RotateAngleGUI();

                    break;

                case "RoundTripAtEdge":
                    EditorGUILayout.PropertyField(moveRadius, new GUIContent("MoveRadius", "出現地点から垂直方向に移動する範囲を指定します"));
                    EditorGUILayout.PropertyField(timeUntilMoveForward, new GUIContent("TimeUntilMoveForward", "上下移動を停止し、前進し始めるまでの時間"));
                    break;
            }
        }


        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(spawner);
    }

    private void RotateAngleGUI()
    {
        EditorGUILayout.PropertyField(rotateAngle, new GUIContent("RotateAngle",
                                                                  "敵の向いている方向からn°回転します。\n" +
                                                                  "正の値ならば左回転、負の値ならば右回転になります。"));

        if (doesRotateToCenter.boolValue && rotateAngle.floatValue < 0)
        {
            EditorGUILayout.HelpBox("「中心に向けて方向転換」がtrueの場合、角度は実行時に正の値に修正されます", MessageType.Warning);
        }

        EditorGUILayout.PropertyField(doesRotateToCenter, new GUIContent("RotateToCenter",
                                                  "敵が画面中央に向けて回転するようになります\n" +
                                                  "また、回転する角度は自動的に正の値に修正されます"));
    }

}
