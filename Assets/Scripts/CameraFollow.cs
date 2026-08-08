using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player
    public Vector3 offset = new Vector3(0f, 12f, -10f); // Kameranın yüksekliği ve uzaklığı
    public float smoothSpeed = 5f; // Takip hızı

    private void Start()
    {
        // Başlangıçta kameranın açısını ayarla (İzometrik açı)
        // X: 45 derece aşağı bakış, Y: 0 karakterin arkası
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Gitmek istediğimiz yer
            Vector3 desiredPosition = target.position + offset;
            
            // Yumuşak takip (Lerp yerine MoveTowards veya SmoothDamp de olur ama bu en stabili)
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}