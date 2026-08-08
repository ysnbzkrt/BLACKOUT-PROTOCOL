using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Görüş Ayarları")]
    public float viewDistance = 10f;
    [Range(0, 360)]
    public float viewAngle = 60f;

    [Header("Katmanlar")]
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [Header("Ses")]
    public AudioSource audioSource;
    public AudioClip detectionSound;

    private EnemyPatrol patrolScript;
    private bool canSeePlayer;

    void Start()
    {
        patrolScript = GetComponent<EnemyPatrol>();
        if (patrolScript == null) patrolScript = GetComponentInParent<EnemyPatrol>();
    }

    void Update()
    {
        FindVisiblePlayer();
    }

    void FindVisiblePlayer()
    {
        Collider[] playersInRadius = Physics.OverlapSphere(transform.position, viewDistance, playerMask);
        bool playerFoundThisFrame = false;

        foreach (Collider p in playersInRadius)
        {
            Transform player = p.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    playerFoundThisFrame = true;

                    if (patrolScript != null)
                        patrolScript.StartChasing();

                    if (!canSeePlayer)
                        PlayDetectionSound();
                }
            }
        }

        if (canSeePlayer && !playerFoundThisFrame)
        {
            if (patrolScript != null)
                patrolScript.StopChasing();

            StopDetectionSound();
        }

        canSeePlayer = playerFoundThisFrame;
    }

    void PlayDetectionSound()
    {
        if (audioSource != null && detectionSound != null)
        {
            audioSource.clip = detectionSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void StopDetectionSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Game Over veya script kapanınca sesi durdur
    void OnDisable()
    {
        StopDetectionSound();
    }

    // Dışarıdan çağırmak için (Game Over scriptinden)
    public void StopDetectionSoundPublic()
    {
        StopDetectionSound();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2, false);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}