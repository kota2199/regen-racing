using UnityEngine;

public class GamepadInputDebugger : MonoBehaviour
{
    // InputManagerで設定されている基本的なボタン名
    private string[] buttonNames = {
        "Fire1", "Fire2", "Fire3", "Fire_0", "Fire_1", "Fire_2", "Fire_3", "Fire_R1", "Fire_L1", "Fire_R2", "Fire_L2", "Fire_R3", "Fire_L3","Jump",
        "Submit", "Cancel"
    };

    // InputManagerでよく使われるアナログ入力名
    private string[] axisNames = {
        "Horizontal", "Vertical",
        "Mouse X", "Mouse Y", "Trigger_R2", "Trigger_L2", "Horizontal Stick-L", "Horizontal Stick-R", "Vertical Stick-R", "Vertical Stick-L"
    };

    private void Update()
    {
        // ボタンの状態をチェック
        foreach (var button in buttonNames)
        {
            if (Input.GetButtonDown(button))
            {
                Debug.Log($"Button Pressed: {button}");
            }
        }

        // アナログスティックやトリガーの状態をチェック
        foreach (var axis in axisNames)
        {
            float value = Input.GetAxis(axis);
            if (Mathf.Abs(value) > 0.5f) // 小さな入力は無視
            {
                //Debug.Log($"Axis Moved: {axis} Value: {value}");
            }
        }
    }
}
