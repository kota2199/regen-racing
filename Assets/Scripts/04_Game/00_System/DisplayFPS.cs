using UnityEngine;
using UnityEngine.UI;

public class DisplayFPS : MonoBehaviour
{
    public Text fpsText;
    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {fps:F1}"; // 小数点1桁で表示
    }
}
