using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // �t�F�[�h�p�̍���Image
    [SerializeField] private float fadeDuration = 1.0f; // �t�F�[�h�̎���

    private void Start()
    {
        // �Q�[���J�n���Ƀt�F�[�h�C������i�K�v�Ȃ�j
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// ��ʂ��t�F�[�h�A�E�g����
    /// </summary>
    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
    }

    /// <summary>
    /// ��ʂ��t�F�[�h�C������
    /// </summary>
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f; // ������Ԃŕs����
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;
    }
}
