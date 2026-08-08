using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float walkSpeed = 3f;
    public float runSpeed = 7f;
    public float rotationSpeed = 10f;
    
    [Header("Zıplama Ayarları")]
    public float jumpHeight = 2.5f;
    public float gravity = -20f;

    [Header("Ses Ayarları")]
    public AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip jumpSound;

    [Header("Ses Volumeleri")]
    [Range(0f, 1f)] public float walkVolume = 0.5f;
    [Range(0f, 1f)] public float runVolume = 0.8f;
    [Range(0f, 1f)] public float jumpVolume = 1f;

    [Header("Ses Pitch'leri")]
    [Range(0.1f, 3f)] public float walkPitch = 1f;
    [Range(0.1f, 3f)] public float runPitch = 1.2f;
    [Range(0.1f, 3f)] public float jumpPitch = 1f;

    [Header("Adım Aralıkları")]
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.3f;

    private CharacterController controller;
    private Animator anim;
    private Vector3 velocity;
    private float footstepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        bool yerleTemasVar = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 0.2f);

        if (yerleTemasVar && velocity.y < 0)
            velocity.y = -2f;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        bool isMoving = move.magnitude >= 0.1f;

        if (isMoving)
        {
            controller.Move(move * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // YÜRÜME / KOŞMA SESİ
            if (yerleTemasVar)
            {
                footstepTimer -= Time.deltaTime;
                if (footstepTimer <= 0f)
                {
                    if (isRunning && runSound != null)
                        PlaySound(runSound, runVolume, runPitch);
                    else if (!isRunning && walkSound != null)
                        PlaySound(walkSound, walkVolume, walkPitch);

                    footstepTimer = isRunning ? runStepInterval : walkStepInterval;
                }
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        // ZIPLAMA SESİ
        if (Input.GetKeyDown(KeyCode.Space) && yerleTemasVar)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            PlaySound(jumpSound, jumpVolume, jumpPitch);

            if (anim != null)
                anim.SetTrigger("Jump");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float animationValue = 0f;
        if (isMoving)
            animationValue = isRunning ? 6.0f : 3.0f;
        
        if (anim != null)
            anim.SetFloat("Speed", animationValue, 0.1f, Time.deltaTime);
    }

    void PlaySound(AudioClip clip, float volume, float pitch)
    {
        if (audioSource == null || clip == null) return;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
    }
}