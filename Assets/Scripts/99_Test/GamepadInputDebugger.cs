using UnityEngine;

public class GamepadInputDebugger : MonoBehaviour
{
    // InputManager�Őݒ肳��Ă����{�I�ȃ{�^����
    private string[] buttonNames = {
        "Fire1", "Fire2", "Fire3", "Fire_0", "Fire_1", "Fire_2", "Fire_3", "Fire_R1", "Fire_L1", "Fire_R2", "Fire_L2", "Fire_R3", "Fire_L3","Jump",
        "Submit", "Cancel"
    };

    // InputManager�ł悭�g����A�i���O���͖�
    private string[] axisNames = {
        "Horizontal", "Vertical",
        "Mouse X", "Mouse Y", "Trigger_R2", "Trigger_L2", "Horizontal Stick-L", "Horizontal Stick-R", "Vertical Stick-R", "Vertical Stick-L"
    };

    private void Update()
    {
        // �{�^���̏�Ԃ��`�F�b�N
        foreach (var button in buttonNames)
        {
            if (Input.GetButtonDown(button))
            {
                Debug.Log($"Button Pressed: {button}");
            }
        }

        // �A�i���O�X�e�B�b�N��g���K�[�̏�Ԃ��`�F�b�N
        foreach (var axis in axisNames)
        {
            float value = Input.GetAxis(axis);
            if (Mathf.Abs(value) > 0.5f) // �����ȓ��͖͂���
            {
                //Debug.Log($"Axis Moved: {axis} Value: {value}");
            }
        }
    }
}
