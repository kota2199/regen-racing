using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RenameCheckPoints))]
public class RenameChildrenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // �f�t�H���g�̃C���X�y�N�^�[�\��

        RenameCheckPoints script = (RenameCheckPoints)target;

        if (GUILayout.Button("�q�I�u�W�F�N�g�̖��O��ύX"))
        {
            script.RenameChildObjects(); // �{�^���������ƃ��l�[��
            EditorUtility.SetDirty(script); // �C���X�y�N�^�[���X�V
        }
    }
}
