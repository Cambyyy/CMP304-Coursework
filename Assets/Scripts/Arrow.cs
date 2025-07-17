using System.Threading;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private bool hasLanded = false;
    public System.Action<float> OnLanded;  // Callback to notify where the arrow landed
    private float timeSince;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  // Cache the Rigidbody2D for reuse
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasLanded)
        {
            hasLanded = true;

            // Inform whoever is listening that the arrow has landed
            OnLanded?.Invoke(transform.position.x);

            // Stop physics simulation to freeze the arrow in place
            rb.simulated = false;

            StartCoroutine(WaitTime(timeSince));  // Wait a bit before cleanup
        }
    }

    void Update()
    {
        // Rotate the arrow to align with its velocity vector (for realism)
        if (rb != null && rb.velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    System.Collections.IEnumerator WaitTime(float timeSince)
    {
        // Wait until a bit of time has passed before destroying the arrow
        while (timeSince < 1.0f)
        {
            timeSince += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);  // Clean up the arrow after it's done
    }
}
