using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [Header("Ayarlar")]
    public float laserDistance = 5f; // Lazerin maksimum uzunluğu (Burayı 3-5 yaparsan kısa olur)
    public LayerMask hitLayers;    // Lazerin çarpacağı katmanlar (Default ve Player seç)
    public Transform firePoint;    // Lazerin çıkış noktası (LaserBeam'i sürükle)

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (firePoint == null) firePoint = transform;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, firePoint.position);

        RaycastHit hit;
        // Lazer bir şeye çarparsa boyu orada kesilir
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, laserDistance, hitLayers))
        {
            lineRenderer.SetPosition(1, hit.point);
            
            if (hit.collider.CompareTag("Player"))
            {
                FindObjectOfType<LaserGameOverManager>().PlayerDied();
            }
        }
        else
        {
            // Hiçbir şeye çarpmazsa sadece belirlediğin mesafe (laserDistance) kadar uzar
            lineRenderer.SetPosition(1, firePoint.position + (firePoint.forward * laserDistance));
        }
    }
}