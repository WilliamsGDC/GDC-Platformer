using UnityEngine;

public class PlayerKilled : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillZone"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Reset pos
        transform.position = respawnPoint.position;
        rb.linearVelocity = Vector2.zero;
    }
}
