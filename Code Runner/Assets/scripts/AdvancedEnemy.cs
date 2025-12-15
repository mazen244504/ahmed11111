using UnityEngine;
using System.Collections;

public class AdvancedEnemy : MonoBehaviour
{
    [Header("======= STATS =======")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damage = 1;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("======= DEATH EFFECTS =======")]
    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float deathAnimationTime = 0.8f;

    [Header("======= DAMAGE EFFECTS =======")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private Color damageFlashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("======= COMPONENTS =======")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    // Private components
    private int currentHealth;
    private bool isDead = false;
    private bool canAttack = true;
    private bool playerInRange = false;

    // Cached components
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private Collider2D col;

    void Awake()
    {
        InitializeComponents();
    }

    void Start()
    {
        InitializeEnemy();
        FindPlayerIfNull();
    }

    void Update()
    {
        if (isDead) return;

        CheckForPlayer();
        HandleMovement();
        HandleAttack();
    }

    void InitializeComponents()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void InitializeEnemy()
    {
        currentHealth = maxHealth;
        isDead = false;
        canAttack = true;

        // ضمان وجود Rigidbody2D
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }

    void FindPlayerIfNull()
    {
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTarget = player.transform;
        }
    }

    void CheckForPlayer()
    {
        if (playerTarget == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        // التحقق إذا كان اللاعب في مدى الرؤية
        if (distanceToPlayer <= detectionRange)
        {
            // التحقق إذا كان هناك عوائق بين العدو واللاعب
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                (playerTarget.position - transform.position).normalized,
                detectionRange,
                obstacleLayer
            );

            if (hit.collider == null || hit.collider.CompareTag("Player"))
            {
                playerInRange = true;
                return;
            }
        }

        playerInRange = false;
    }

    void HandleMovement()
    {
        if (!playerInRange || playerTarget == null) return;

        Vector2 direction = (playerTarget.position - transform.position).normalized;

        // تحريك العدو
        if (rb != null)
        {
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        // قلب اتجاه السبرايت
        if (direction.x > 0)
            sr.flipX = false;
        else if (direction.x < 0)
            sr.flipX = true;

        // تشغيل أنيميشن الحركة
        if (anim != null)
            anim.SetBool("IsMoving", true);
    }

    void HandleAttack()
    {
        if (!playerInRange || playerTarget == null || !canAttack) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= attackRange)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;

        // تشغيل أنيميشن الهجوم
        if (anim != null)
            anim.SetTrigger("Attack");

        // تطبيق الضرر على اللاعب (سوف تحتاج لإضافة OnTriggerEnter2D في اللاعب)
        if (audioSource != null && damageSound != null)
            audioSource.PlayOneShot(damageSound);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Health: {currentHealth}");

        // تأثير الإصابة
        StartCoroutine(DamageFlash());

        // أنيميشن الإصابة
        if (anim != null)
            anim.SetTrigger("Hurt");

        // صوت الإصابة
        if (audioSource != null && damageSound != null)
            audioSource.PlayOneShot(damageSound);

        // الموت إذا الصحة = 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageFlash()
    {
        Color originalColor = sr.color;
        sr.color = damageFlashColor;

        yield return new WaitForSeconds(flashDuration);

        sr.color = originalColor;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} died!");

        // أنيميشن الموت
        if (anim != null)
            anim.SetTrigger("Die");

        // تأثير الموت المرئي
        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // صوت الموت
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // تعطيل الفيزياء والكوليدر
        if (col != null)
            col.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // تأثير التلاشي والتدمير
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsedTime < deathAnimationTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / deathAnimationTime;

            // التلاشي
            if (sr != null)
            {
                Color currentColor = sr.color;
                sr.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1 - progress);
            }

            // التصغير
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);

            yield return null;
        }

        Destroy(gameObject);
    }

    // لإظهار المدى في محرر Unity (للتعديل المرئي)
    void OnDrawGizmosSelected()
    {
        // مدى الرؤية
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // مدى الهجوم
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // يمكن إضافة هذه الوظائف لتسهيل التعديل من أكواد أخرى
    public void SetTarget(Transform target)
    {
        playerTarget = target;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public bool IsDead()
    {
        return isDead;
    }
}