using UnityEngine;
using UnityEngine.Audio;

public class LevelExit : MonoBehaviour
{
    private bool isPlayerInRange = false;

    [Header("Geçiş Ayarları")]
    public string nextSceneName = "Level3";

    [Header("Ses Ayarları")]
    public AudioSource audioSource;
    public AudioClip exitSound;
    [Range(0f, 1f)] public float exitVolume = 1f;
    [Range(0.1f, 3f)] public float exitPitch = 1f;

    void Update()
    {
        if (isPlayerInRange && LevelManager.instance.AreAllDisksCollected()) 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // ✅ GEÇİŞ SESİ
                if (audioSource != null && exitSound != null)
                {
                    audioSource.pitch = exitPitch;
                    audioSource.PlayOneShot(exitSound, exitVolume);
                }

                NextLevel();
            }
        }
    }

    void NextLevel()
    {
        if (SceneChanger.instance != null)
            SceneChanger.instance.StartFadeOut(nextSceneName);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }
}