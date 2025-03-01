using UnityEngine;

public class RenameCheckPoints : MonoBehaviour
{
    [SerializeField] private Transform parentObject; // �e�I�u�W�F�N�g���C���X�y�N�^�[�Ŋ��蓖��

    public void RenameChildObjects()
    {
        if (parentObject == null)
        {
            Debug.LogWarning("�e�I�u�W�F�N�g�����蓖�Ă��Ă��܂���B");
            return;
        }

        int count = 1;
        foreach (Transform child in parentObject)
        {
            //Undo.RecordObject(child.gameObject, "Rename Children"); // Undo�@�\
            child.name = "Point"+count.ToString(); // ���O�ύX
            count++;
        }

        Debug.Log("�q�I�u�W�F�N�g�̖��O��ύX���܂����B");
    }
}
