using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Elevator : MonoBehaviour
{
    public Transform targetPosition; // Set this to the "up" position
    public float riseSpeed = 2f;
    public float vibrationIntensity = 0.05f;
    public float vibrationFrequency = 0.05f;
    public float waitAtTopDuration = 3f;

    private Vector3 startPosition;
    private Rigidbody2D rb;
    private Coroutine vibrationCoroutine;
    private Coroutine riseRoutine;

    private bool playerOnPlatform = false;
    private GameObject playerRef;
    private float collisionTimer = 0f;
    private float leaveTimer = 0f;
    private bool isRising = false;

    private Vector3 previousPlayerPos;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (playerOnPlatform && playerRef != null && !isRising)
        {
            Vector3 playerDelta = playerRef.transform.position - previousPlayerPos;
            rb.MovePosition(rb.position + new Vector2(playerDelta.x, 0)); // follow only horizontal movement
            previousPlayerPos = playerRef.transform.position;
        }

        if (playerOnPlatform)
        {
            collisionTimer += Time.deltaTime;
            leaveTimer = 0f;

            if (collisionTimer > 1f && !isRising)
            {
                StartRisingSequence();
            }
        }
        else
        {
            if (!isRising)
            {
                leaveTimer += Time.deltaTime;
                collisionTimer = 0f;

                if (leaveTimer > 2f)
                {
                    StopAllCoroutines();
                    rb.MovePosition(startPosition);
                }
            }
        }
    }

    private void StartRisingSequence()
    {
        if (riseRoutine != null)
            StopCoroutine(riseRoutine);

        riseRoutine = StartCoroutine(RisingLoop());
    }

    IEnumerator RisingLoop()
    {
        isRising = true;

        if (vibrationCoroutine != null)
            StopCoroutine(vibrationCoroutine);

        vibrationCoroutine = StartCoroutine(Vibrate());

        yield return new WaitForSeconds(0.5f); // delay before lift

        if (vibrationCoroutine != null)
            StopCoroutine(vibrationCoroutine);

      
        rb.MovePosition(startPosition);

        yield return StartCoroutine(MovePlatform(startPosition, targetPosition.position));

        yield return new WaitForSeconds(waitAtTopDuration);

        yield return StartCoroutine(MovePlatform(targetPosition.position, startPosition));

        isRising = false;
        collisionTimer = 0f;
    }

    IEnumerator MovePlatform(Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * riseSpeed;
            Vector3 newPos = Vector3.Lerp(from, to, t);
            rb.MovePosition(newPos);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Vibrate()
    {
        while (true)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-vibrationIntensity, vibrationIntensity),
                Random.Range(-vibrationIntensity, vibrationIntensity),
                0f);

            rb.MovePosition(startPosition + randomOffset);

            yield return new WaitForSeconds(vibrationFrequency);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = true;
            playerRef = collision.gameObject;
            previousPlayerPos = playerRef.transform.position;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
            playerRef = null;
        }
    }
}
