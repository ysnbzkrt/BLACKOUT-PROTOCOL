using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Animasyon Ayarları")]
    private Animator anim;
    [Tooltip("Bu düşman yakaladığında oyuncunun oynatacağı animasyon (Örn: Captured, geriyedüş)")]
    public string playerDeathAnimName = "Captured"; 
    [Tooltip("Bu düşmanın yakalama anında kendisinin oynatacağı animasyon (Örn: Attack1, Kick)")]
    public string enemyAttackAnimName = "Captured"; 

    [Header("Devriye Ayarları")]
    public Transform[] waypoints;
    public float patrolSpeed = 3f;
    private int currentWaypointIndex = 0;

    [Header("Kovalama Ayarları")]
    public float chaseSpeed = 5.5f;
    public float killDistance = 1.8f; 
    public float loseDistance = 15f; 
    
    [Header("Işık Ayarları")]
    public float normalIntensity = 80f; 
    public float chaseIntensity = 150f;
    public float normalRange = 10f; 
    public float chaseRange = 20f;  

    [Header("UI Ayarları")]
    public GameObject gameOverPanel; 

    private bool isChasing = false;
    private bool isGameOver = false; 
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Light enemyLight;
    private Color originalLightColor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyLight = GetComponentInChildren<Light>();
        anim = GetComponentInChildren<Animator>();
        
        if (enemyLight != null) 
        {
            originalLightColor = enemyLight.color;
            enemyLight.intensity = normalIntensity;
            enemyLight.range = normalRange;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        agent.speed = patrolSpeed;
        GoToNextWaypoint();
    }

    void Update()
    {
        if (agent == null || isGameOver) return; 

        if (isChasing && playerTransform != null)
        {
            agent.destination = playerTransform.position;
            agent.speed = chaseSpeed;

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= killDistance)
            {
                StartCoroutine(CinematicDeath());
            }

            if (distanceToPlayer > loseDistance)
            {
                StopChasing();
            }
        }
        else
        {
            agent.speed = patrolSpeed;
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextWaypoint();
            }
        }

        if (anim != null)
        {
            float currentMoveSpeed = agent.velocity.magnitude;
            anim.SetFloat("Speed", currentMoveSpeed, 0.1f, Time.deltaTime);
        }
    }

    IEnumerator CinematicDeath()
    {
        isGameOver = true;
        
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        
        var playerMovement = playerTransform.GetComponent<PlayerMovement>();
        if(playerMovement != null) playerMovement.enabled = false;

        Vector3 enemyLookPos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(enemyLookPos);

        Vector3 playerLookPos = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
        playerTransform.LookAt(playerLookPos);

        if (anim != null) anim.SetTrigger(enemyAttackAnimName); 
        
        Animator playerAnim = playerTransform.GetComponent<Animator>();
        if (playerAnim != null) playerAnim.SetTrigger(playerDeathAnimName); 

        if (enemyLight != null)
        {
            enemyLight.color = Color.red;
            enemyLight.intensity = 250f;
        }

        yield return new WaitForSeconds(1.5f); 

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Vector3 startPos = mainCam.transform.position;
            Vector3 offset = (playerTransform.forward * 2.8f) + (Vector3.up * 1.6f);
            Vector3 endPos = playerTransform.position + offset;
            
            float elapsed = 0;
            float duration = 3.0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                t = t * t * (3f - 2f * t);

                mainCam.transform.position = Vector3.Lerp(startPos, endPos, t);
                mainCam.transform.LookAt(playerTransform.position + Vector3.up * 0.5f);
                
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        // ✅ RADAR SESLERİNİ DURDUR
        EnemyVision[] enemies = FindObjectsOfType<EnemyVision>();
        foreach (EnemyVision enemy in enemies)
            enemy.StopDetectionSoundPublic();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); 
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.None; 
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0); 
    }

    public void StartChasing()
    {
        if (!isChasing && !isGameOver) 
        {
            isChasing = true;
            if (enemyLight != null)
            {
                enemyLight.color = Color.red;
                enemyLight.intensity = chaseIntensity; 
                enemyLight.range = chaseRange;
            }
        }
    }

    public void StopChasing()
    {
        if (isGameOver) return;
        isChasing = false;
        if (enemyLight != null)
        {
            enemyLight.color = originalLightColor;
            enemyLight.intensity = normalIntensity; 
            enemyLight.range = normalRange;
        }
        GoToNextWaypoint();
    }

    void GoToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0 || isChasing || agent == null || isGameOver) return;
        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}