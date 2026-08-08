using UnityEngine;

public class DiskItem : MonoBehaviour
{
    public float rotationSpeed = 100f;

    [Header("Ses Ayarları")]
    public AudioClip collectSound;
    [Range(0f, 1f)] public float collectVolume = 1f;
    [Range(0.1f, 3f)] public float collectPitch = 1f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.instance.CollectDisk();

            // ✅ TOPLAMA SESİ
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position, collectVolume);

            Destroy(gameObject);
        }
    }
}