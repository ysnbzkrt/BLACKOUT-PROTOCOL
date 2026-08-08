using UnityEngine;
using System.Collections;

public class LaserGameOverManager : MonoBehaviour
{
    [Header("Gerekli Objeler")]
    public GameObject player;         
    public Camera mainCamera;         
    public GameObject gameOverCanvas; 

    [Header("Ses Ayarları")]
    public AudioSource audioSource;   // Sahnedeki AudioSource (hoparlör)
    public AudioClip laserDeadClip;   // "laserdead" ses dosyanı buraya sürükle

    [Header("Ayarlar")]
    public float zoomDuration = 2.0f; 
    private bool isDead = false;

    public void PlayerDied()
    {
        if (isDead) return;
        isDead = true;

        // --- SES BURADA TETİKLENİYOR ---
        if (audioSource != null && laserDeadClip != null)
        {
            audioSource.PlayOneShot(laserDeadClip);
        }

        if(player.GetComponent<PlayerMovement>() != null)
            player.GetComponent<PlayerMovement>().enabled = false;

        Animator anim = player.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Die");

        StartCoroutine(CameraZoomRoutine());
    }

    IEnumerator CameraZoomRoutine()
    {
        float elapsed = 0;
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        while (elapsed < zoomDuration)
        {
            Vector3 targetPos = player.transform.position + new Vector3(0, 1.5f, 3f); 
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / zoomDuration);
            
            Vector3 direction = (player.transform.position + Vector3.up) - mainCamera.transform.position;
            if (direction != Vector3.zero)
                mainCamera.transform.rotation = Quaternion.Slerp(startRot, Quaternion.LookRotation(direction), elapsed / zoomDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (gameOverCanvas != null) gameOverCanvas.SetActive(true);
    }
}