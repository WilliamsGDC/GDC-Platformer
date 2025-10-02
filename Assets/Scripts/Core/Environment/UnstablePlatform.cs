using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class UnstablePlatform : MonoBehaviour
{
    public float countdownBeforeFall = 2f;
    public float resetDelay = 5f;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 20f;
    public float floatBackSpeed = 2f;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isShaking = false;
    private bool hasStarted = false;
    private bool isFloatingBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        FreezeObject(); // Start frozen
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasStarted)
        {
            // Check if the collision came from above
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = contact.normal;

            // If the normal points downward, player is above
            if (normal.y < -0.5f)
            {
                hasStarted = true;
                StartCoroutine(VibrateThenTopple());
            }
        }
    }

    IEnumerator VibrateThenTopple()
    {
        isShaking = true;
        if (audioSource != null) audioSource.Play();

        float timer = 0f;
        while (timer < countdownBeforeFall)
        {
            float xShake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.position = originalPosition + new Vector3(xShake, 0f, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        if (audioSource != null) audioSource.Stop();
        isShaking = false;

        Topple();
    }

    void Topple()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.isKinematic = false;
        rb.gravityScale = 1f;

        rb.AddTorque(20f);

        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        StartCoroutine(FloatBackToStart());
    }

    IEnumerator FloatBackToStart()
    {
        rb.isKinematic = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        isFloatingBack = true;

        float elapsed = 0f;
        float duration = 1.5f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, originalPosition, t);
            transform.rotation = Quaternion.Lerp(startRot, originalRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        isFloatingBack = false;
        hasStarted = false;

        FreezeObject();
    }

    void FreezeObject()
    {
        rb.isKinematic = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                         RigidbodyConstraints2D.FreezePositionY |
                         RigidbodyConstraints2D.FreezeRotation;
    }
}
