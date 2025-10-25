using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerKilled : MonoBehaviour
{
    // this should be integrated with the main player script first if new functionality needs to be added
    [Header("Respawn Settings")]
    public Transform respawnPoint;

    [Header("Health Settings")]
    public int maxHP = 6;
    private int currentHP;

    [Header("UI")]
    public Slider hpSlider;
    public Image healthStatusImage; // UI image to change
    public Sprite fullHealthSprite;
    public Sprite highHealthSprite;
    public Sprite lowHealthSprite;
    public Sprite deadSprite;
    public GameOverScreenManager GameOverScreenManager;

    private Rigidbody2D rb;
    private bool isDead = false;

    int score = 0; // add score to be displayed on GameOverScreen

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillZone") && !isDead)
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHealthUI();

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void Die()
    {
        GameOverScreenManager.Setup(score);
        isDead = true;
        gameObject.SetActive(false);
    }

    private void UpdateHealthUI()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        if (healthStatusImage != null)
        {
            float hpPercent = (float)currentHP / maxHP;

            if (currentHP <= 0)
            {
                healthStatusImage.sprite = deadSprite;
            }
            else if (hpPercent >= 0.9f)
            {
                healthStatusImage.sprite = fullHealthSprite;
            }
            else if (hpPercent >= 0.4f)
            {
                healthStatusImage.sprite = highHealthSprite;
            }
            else
            {
                healthStatusImage.sprite = lowHealthSprite;
            }
        }
    }
}
