using UnityEngine;
using TMPro;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; 

    [Header("Disk Ayarları")]
    public int totalDisks = 15; 
    private int collectedDisks = 0;

    [Header("UI Ayarları")]
    public CanvasGroup counterGroup; 
    public TextMeshProUGUI counterText; // "TOPLANAN DİSKLER: 10 / 15" yazan yer
    public TextMeshProUGUI missionText; // "DİSKLERİ TOPLADIN..." yazacak olan yer

    private bool isUIVisible = false;

    void Awake()
    {
        instance = this;
        // Inspector'daki değeri kodla garantiye almak istersen burayı açabilirsin:
        // totalDisks = 15; 
    }

    void Start()
    {
        // Oyun başında görev yazısı görünmesin
        if (missionText != null) missionText.text = "";
    }

    public void CollectDisk()
    {
        collectedDisks++;
        UpdateUI();

        if (!isUIVisible)
        {
            StartCoroutine(FadeInUI());
        }

        // Tüm diskler toplandığında
        if (collectedDisks >= totalDisks)
        {
            ShowMissionComplete();
        }
    }

    void UpdateUI()
    {
        counterText.text = "TOPLANAN DİSKLER: " + collectedDisks + " / " + totalDisks;
    }

    void ShowMissionComplete()
    {
        // 1. Eski yazıyı (sayacı) tamamen kapatıyoruz
        if (counterText != null) counterText.gameObject.SetActive(false);

        // 2. Yeni görev yazısını aktif edip içeriğini yazıyoruz
        if (missionText != null)
        {
            missionText.text = "DİSKLERİ TOPLADIN! BİLGİSAYARA ULAŞ!";
            missionText.color = Color.yellow;
            StartCoroutine(FlashMissionText());
        }
    }

    // Bilgisayarın kontrol etmesi için toplama sayısını veren fonksiyon
    public bool AreAllDisksCollected()
    {
        return collectedDisks >= totalDisks;
    }

    IEnumerator FlashMissionText()
    {
        while (collectedDisks >= totalDisks)
        {
            missionText.enabled = !missionText.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator FadeInUI()
    {
        isUIVisible = true;
        while (counterGroup.alpha < 1)
        {
            counterGroup.alpha += Time.deltaTime * 2f;
            yield return null;
        }
    }
}