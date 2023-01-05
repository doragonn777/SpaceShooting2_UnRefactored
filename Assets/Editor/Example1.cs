using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SubLightning))]//拡張するクラスを指定
public class LightningTest : Editor
{

    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SubLightning subLightning = (SubLightning)target;

        if (GUILayout.Button("ShootLightning"))
        {
            subLightning.shootLightning();
        }
    }

}