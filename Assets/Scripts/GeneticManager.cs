using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    public GameObject archerPrefab;
    public GameObject arrowPrefab;
    public Transform spawnPoint;
    public Transform targetX;
    private float oldTargetX;

    public bool randomWind = false;
    public float wind = 0f;
    public float windMax = 0f;

    public int populationSize = 10;
    public float mutationRate = 0.1f;
    public int generations = 10;

    public float timeScale = 1f;

    public bool targetMove = false;
    public float targetMoveMax = 5.0f;

    public TextMeshPro genCounter;

    private List<Archer> population = new List<Archer>();
    public int currentGeneration = 0;

    public SimulationManager simManager;

    public void StartEvolution()
    {
        currentGeneration = 0;
        oldTargetX = targetX.position.x;

        CreateInitialPopulation();
        CheckAndMoveTarget();     // Give the target a little wiggle if enabled
        CheckAndChangeWind();     // Windy

        StartCoroutine(ShootAll()); 
    }

    void CreateInitialPopulation()
    {
        population.Clear();

        for (int i = 0; i < populationSize; i++)
        {
            // Spawn each archer with randomized traits
            Archer archer = Instantiate(archerPrefab, spawnPoint.position, Quaternion.identity).GetComponent<Archer>();
            archer.angle = Random.Range(10.0f, 80.0f);
            archer.force = Random.Range(5.0f, 20.0f);
            archer.variance = Random.Range(0.0f, 5.0f);

            population.Add(archer);
        }
    }

    System.Collections.IEnumerator ShootAll()
    {
        Time.timeScale = timeScale;
        int remainingShots = population.Count;

        foreach (var archer in population)
        {
            GameObject arrowObj = archer.Shoot(wind, spawnPoint.position, arrowPrefab);
            Arrow arrowScript = arrowObj.GetComponent<Arrow>();

            // Callback once arrow lands
            arrowScript.OnLanded = (landedX) => {
                archer.CalculateFitness(targetX.position.x, landedX);
                remainingShots--;
            };
        }

        // Wait for everyone to finish before moving on
        yield return new WaitUntil(() => remainingShots <= 0);
        yield return new WaitForSeconds(0.5f);   // A short breather for aesthetics

        simManager.OnGenerationComplete(population, currentGeneration);
        currentGeneration++;

        if (currentGeneration < generations)
        {
            Evolve();
            CheckAndMoveTarget();     // Maybe move the target again
            CheckAndChangeWind();     // Wind keeps changing...
            StartCoroutine(ShootAll());
        }
    }

    void Evolve()
    {
        // Rank by fitness (better archers float to the top)
        population.Sort((a, b) => b.fitness.CompareTo(a.fitness));

        List<Archer> newPop = new List<Archer>();

        for (int i = 0; i < populationSize; i++)
        {
            // Select two "parents" from the top 50%
            Archer parent1 = population[Random.Range(0, populationSize / 2)];
            Archer parent2 = population[Random.Range(0, populationSize / 2)];

            // Simple crossover logic
            float angle = (parent1.angle + parent2.angle) / 2;
            float force = (parent1.force + parent2.force) / 2;
            float variance = (parent1.variance + parent2.variance) / 2;

            // Occasional mutation
            if (Random.value < mutationRate) angle += Random.Range(-5f, 5f);
            if (Random.value < mutationRate) force += Random.Range(-2f, 2f);
            if (Random.value < mutationRate) variance += Random.Range(0f, 1f);

            Archer child = Instantiate(archerPrefab, spawnPoint.position, Quaternion.identity).GetComponent<Archer>();
            child.angle = Mathf.Clamp(angle, 0, 90);
            child.force = Mathf.Max(force, 0);
            child.variance = variance;

            newPop.Add(child);
        }

        // Cleanse old generation
        foreach (var a in population)
        {
            Destroy(a.gameObject);
        }

        population = newPop;
    }

    void CheckAndMoveTarget()
    {
        if (targetMove)
        {
            // Slide the target around a bit within allowed range
            targetX.position = new Vector3(Random.Range(oldTargetX - targetMoveMax, oldTargetX + targetMoveMax), 0.0f, 0.0f);
        }
    }

    void CheckAndChangeWind()
    {
        if (randomWind)
        {
            wind = Random.Range(-windMax, windMax);
        }
    }
}
