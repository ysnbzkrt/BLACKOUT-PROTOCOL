using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Objeleri")]
    public GameObject howToPlayImage; 
    public GameObject creditsImage; 
    public Image fadePanel; 

    [Header("Geçiş Ayarları")]
    public float fadeDuration = 0.8f; // Normal menü hızı
    public float startGameFadeDuration = 2.0f; // Sadece oyuna başlarkenki yavaş hız

    void Awake()
    {
        if (howToPlayImage != null) howToPlayImage.SetActive(false);
        if (creditsImage != null) creditsImage.SetActive(false);
        
        if (fadePanel != null) 
        {
            fadePanel.raycastTarget = false;
            fadePanel.color = new Color(0, 0, 0, 0);
        }
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // --- OYUNA BAŞLA (ÖZEL YAVAŞ GEÇİŞ) ---
    public void PlayGame()
    {
        StartCoroutine(PlayGameSequence());
    }

    IEnumerator PlayGameSequence()
    {
        fadePanel.raycastTarget = true; 
        // Burada startGameFadeDuration kullanarak yavaşça karartıyoruz
        yield return StartCoroutine(Fade(0, 1, startGameFadeDuration)); 
        SceneManager.LoadScene(1); 
    }

    // --- NASIL OYNANIR ---
    public void OpenHowToPlay() { StartCoroutine(FadeSequence(howToPlayImage, true)); }
    public void CloseHowToPlay() { StartCoroutine(FadeSequence(howToPlayImage, false)); }

    // --- HAZIRLAYANLAR ---
    public void OpenCredits() { StartCoroutine(FadeSequence(creditsImage, true)); }
    public void CloseCredits() { StartCoroutine(FadeSequence(creditsImage, false)); }

    // GENEL GEÇİŞ SİSTEMİ (NORMAL HIZ)
    IEnumerator FadeSequence(GameObject targetUI, bool opening)
    {
        fadePanel.raycastTarget = true; 

        yield return StartCoroutine(Fade(0, 1, fadeDuration)); // Normal kararma

        if (targetUI != null) targetUI.SetActive(opening);

        yield return StartCoroutine(Fade(1, 0, fadeDuration)); // Normal aydınlanma

        fadePanel.raycastTarget = false; 
    }

    // Süreyi (duration) dışarıdan alabilen geliştirilmiş Fade fonksiyonu
    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0;
        Color c = fadePanel.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadePanel.color = c;
            yield return null;
        }
        c.a = endAlpha;
        fadePanel.color = c;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}