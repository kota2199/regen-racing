using UnityEngine;

public class RawControllerInputLogger : MonoBehaviour
{
    void Update()
    {
        // ボタン 0 ~ 19 までをチェック（コントローラーによって異なる）
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.JoystickButton0 + i)))
            {
                Debug.Log($"Joystick Button {i} が押されました");
            }
        }
    }
}
