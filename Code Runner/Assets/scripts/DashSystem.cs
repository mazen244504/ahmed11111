using UnityEngine;
using UnityEngine.UI;

public class DashSystem : MonoBehaviour
{
    public float dashSpeed = 35f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 0.3f;
    public float maxCharge = 100f;
    public float currentCharge = 0f;
    public float chargePerSecond = 10f;
    public float dashCost = 30f;
    public Slider chargeBar;
    public Image chargeFill;
    public Color fullColor = Color.green;
    public Color lowColor = Color.red;
    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing = false;
    private float originalGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        currentCharge = 0f;
        UpdateChargeBar();
    }

    void Update()
    {
        if (currentCharge < maxCharge)
        {
            currentCharge += chargePerSecond * Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
            UpdateChargeBar();
        }

        bool hasEnoughCharge = currentCharge >= dashCost;
        bool isDashKeyPressed = Input.GetKeyDown(KeyCode.G);

        if (isDashKeyPressed && canDash && hasEnoughCharge && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    System.Collections.IEnumerator PerformDash()
    {
        isDashing = true;
        canDash = false;
        currentCharge -= dashCost;
        UpdateChargeBar();

        float originalGravitySave = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 dashDirection = GetDashDirection();
        rb.velocity = dashDirection * dashSpeed;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.cyan;

        Debug.Log("Dash performed. Remaining charge: " + currentCharge);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = rb.velocity * 0.3f;
        rb.gravityScale = originalGravitySave;
        sr.color = originalColor;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    Vector2 GetDashDirection()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.D))
            direction.x += 1f;
        if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.A))
            direction.x -= 1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
            direction.y += 1f;
        if (Input.GetKey(KeyCode.S))
            direction.y -= 1f;

        if (direction == Vector2.zero)
            direction.x = transform.localScale.x > 0 ? 1f : -1f;

        return direction.normalized;
    }

    void UpdateChargeBar()
    {
        if (chargeBar != null)
        {
            chargeBar.value = currentCharge / maxCharge;
            if (chargeFill != null)
                chargeFill.color = Color.Lerp(lowColor, fullColor, currentCharge / maxCharge);
        }
    }

    public void AddCharge(float amount)
    {
        currentCharge = Mathf.Clamp(currentCharge + amount, 0, maxCharge);
        UpdateChargeBar();
        Debug.Log("+ " + amount + " charge added. Total: " + currentCharge);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 25), "Dash Charge: " + currentCharge.ToString("F0") + "/100");
    }
}
