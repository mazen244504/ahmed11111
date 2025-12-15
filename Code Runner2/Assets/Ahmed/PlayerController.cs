using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;            // how fast the character moves
    public float jumpHeight;           // how high the character jumps
    private bool isFacingRight;        // check if player is facing right (not used now, kept for compatibility)
    public KeyCode Spacebar;           // jump button (set in inspector)
    public KeyCode L;                  // left movement button
    public KeyCode R;                  // right movement button
    public Transform groundCheck;      // an invisible point in space. We use it to see if the player is touching the ground
    public float groundCheckRadius;    // value to determine how big the circle around the player's feet is
    public LayerMask whatIsGround;     // this variable stores what is considered a ground to the character
    private bool grounded;             // check if the character is standing on solid ground

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // ---------- SHOOTING ----------
    [Header("Shooting")]
    public GameObject projectilePrefab;    // bullet prefab (drag in inspector)
    public Transform firePoint;            // muzzle transform (child of player or UpperBody)
    public string shootAnimTrigger = "Shoot"; // trigger name in Animator
    public float cooldown = 0.25f;         // seconds between shots
    public float projectileSpeed = 12f;    // bullet speed
    public int damagePerShot = 1;

    public int maxAmmo = 999;              // optional
    public bool useAmmo = false;

    private float lastShotTime = -999f;
    private int currentAmmo;
    // -------------------------------

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // initialize ammo
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        // Jump (uses public Spacebar key set in inspector)
        if (Input.GetKeyDown(Spacebar) && grounded)
        {
            Jump();
        }

        // Left movement
        if (Input.GetKey(L))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

            if (sr != null)
            {
                sr.flipX = true;
            }
        }

        // Right movement
        if (Input.GetKey(R))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

            if (sr != null)
            {
                sr.flipX = false;
            }
        }

        // Shooting input: single-shot on Fire1 (mouse left / Ctrl) or X key
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.X))
        {
            TryShoot();
        }

        // update animator params
        anim.SetFloat("Height", rb.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("Grounded", grounded);
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        // this statement calculates when exactly the character is considered by Unity’s engine to be standing on the ground
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        // player character jumps vertically along the y-axis without disrupting horizontal walk
    }

    // ===================== Shooting methods =====================
    public void TryShoot()
    {
        // enforce cooldown
        if (Time.time < lastShotTime + cooldown) return;

        // enforce ammo if used
        if (useAmmo && currentAmmo <= 0) return;

        lastShotTime = Time.time;
        if (useAmmo) currentAmmo--;

        // trigger animation; actual bullet will spawn from animation event FireFromAnimation()
        if (anim != null && !string.IsNullOrEmpty(shootAnimTrigger))
        {
            anim.SetTrigger(shootAnimTrigger);
        }
        else
        {
            // fallback: spawn immediately if no animator assigned
            Shoot();
        }
    }

    // This method must be called by an Animation Event placed on the shoot animation
    // Name the Animation Event function "FireFromAnimation" (exactly) when creating the event.
    public void FireFromAnimation()
    {
        Shoot();
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("Projectile prefab or firePoint not assigned in PlayerController.");
            return;
        }

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // determine direction from flipX: flipX true => facing left
        float dir = (sr != null && sr.flipX) ? -1f : 1f;

        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.velocity = new Vector2(dir * projectileSpeed, 0f);
        }

        // pass damage to projectile if it has Projectile script
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null) p.SetDamage(damagePerShot);
    }
    // =============================================================
}
