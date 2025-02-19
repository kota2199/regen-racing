using UnityEngine;

public class RenameCheckPoints : MonoBehaviour
{
    [SerializeField] private Transform parentObject; // 親オブジェクトをインスペクターで割り当て

    public void RenameChildObjects()
    {
        if (parentObject == null)
        {
            Debug.LogWarning("親オブジェクトが割り当てられていません。");
            return;
        }

        int count = 1;
        foreach (Transform child in parentObject)
        {
            //Undo.RecordObject(child.gameObject, "Rename Children"); // Undo機能
            child.name = "Point"+count.ToString(); // 名前変更
            count++;
        }

        Debug.Log("子オブジェクトの名前を変更しました。");
    }
}
