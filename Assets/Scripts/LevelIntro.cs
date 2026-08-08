using UnityEngine;
using TMPro; // TextMeshPro kullanıyorsan bu kalmalı
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    [Header("Bileşenler")]
    public CanvasGroup introGroup;     // Panelin kendisi (Canvas Group olan)
    public TextMeshProUGUI missionText; // TMP Yazı objesi

    [Header("Görev Ayarları")]
    [TextArea(3, 10)]
    public string fullMessage = "GÖREV: YAKALANMADAN 15 DİSKİ TOPLA VE BİLGİSAYARA ULAŞ...";
    public float typeSpeed = 0.05f;    // Yazım hızı (küçüldükçe hızlanır)
    public float displayDuration = 3f; // Yazı bittikten sonra ne kadar beklesin?

    void Start()
    {
        // Başlangıçta her şeyi sıfırla
        introGroup.alpha = 0;
        missionText.text = "";
        
        // Karizmatik başlangıcı tetikle
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // 1. Panel yavaşça belirsin (Fade In)
        while (introGroup.alpha < 1)
        {
            introGroup.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // 2. Yazıyı harf harf yaz (Typewriter Efekti)
        foreach (char letter in fullMessage.ToCharArray())
        {
            missionText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        // 3. Yazı bittikten sonra bekle
        yield return new WaitForSeconds(displayDuration);

        // 4. Panel yavaşça kaybolsun (Fade Out)
        while (introGroup.alpha > 0)
        {
            introGroup.alpha -= Time.deltaTime * 1.5f;
            yield return null;
        }

        // 5. İşlem bitince paneli tamamen kapat ki arkadaki butonlara engel olmasın
        introGroup.gameObject.SetActive(false);
    }
}