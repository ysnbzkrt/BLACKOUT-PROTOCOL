using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    public Image fadeImage; // Siyah paneli buraya sürükle
    public float fadeSpeed = 1.5f;

    void Awake()
    {
        instance = this;
    }

    public void StartFadeOut(string sceneName)
    {
        StartCoroutine(FadeOutCoroutine(sceneName));
    }

    IEnumerator FadeOutCoroutine(string sceneName)
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}