using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RenameCheckPoints))]
public class RenameChildrenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // デフォルトのインスペクター表示

        RenameCheckPoints script = (RenameCheckPoints)target;

        if (GUILayout.Button("子オブジェクトの名前を変更"))
        {
            script.RenameChildObjects(); // ボタンを押すとリネーム
            EditorUtility.SetDirty(script); // インスペクターを更新
        }
    }
}
