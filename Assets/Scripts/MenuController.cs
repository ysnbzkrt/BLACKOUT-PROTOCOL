using UnityEngine;
using UnityEngine.SceneManagement; // Sahne işlemleri için bu şart!

public class MenuController : MonoBehaviour
{
    // 1. TEKRAR DENE BUTONU İÇİN FONKSİYON
    public void RestartGame()
    {
        // Eğer oyun donmuşsa (Time.timeScale = 0 ise) tekrar başlatmak için 1 yapıyoruz
        Time.timeScale = 1f; 
        
        // Şu anki aktif sahneyi bulup yeniden yükler
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // 2. ANA MENÜ BUTONU İÇİN FONKSİYON
    public void GoToMainMenu()
    {
        // Zamanı normale döndür
        Time.timeScale = 1f; 
        
        // "MainMenu" yazan yere senin giriş sahnenin adını yazmalısın.
        // Eğer isminden emin değilsen 0 (sıfır) yazarak ilk sahneye gönderebilirsin.
        SceneManager.LoadScene("MainMenu"); 
    }
}