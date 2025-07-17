using UnityEngine;

public class Archer : MonoBehaviour
{
    public float angle;
    public float force;
    public float fitness;
    public float variance; // introduces controlled randomness to aim and force

    private Rigidbody2D arrowRb;

    public GameObject Shoot(float wind, Vector2 spawnPosition, GameObject arrowPrefab)
    {
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        arrowRb = arrow.GetComponent<Rigidbody2D>();

        // Apply random offset within the specified variance range
        float variedAngle = angle + Random.Range(-variance, variance);
        float variedForce = force + Random.Range(-variance * 0.5f, variance * 0.5f);

        // Convert angle to radians and clamp to avoid invalid input
        float rad = Mathf.Deg2Rad * Mathf.Clamp(variedAngle, 0f, 90f);

        // Compute launch velocity based on angle and force
        Vector2 velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * force;

        // Add wind influence to horizontal motion
        velocity.x += wind;

        arrowRb.velocity = velocity;
        return arrow;
    }

    public void CalculateFitness(float targetX, float landedX)
    {
        // Fitness is better the closer the arrow lands to the target
        fitness = -Mathf.Abs(landedX - targetX);
    }
}
