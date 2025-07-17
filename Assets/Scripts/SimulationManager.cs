using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public GeneticManager geneticManager;
    public CSVExporter csvExporter;

    public void StartSimulation()
    {
        geneticManager.simManager = this;  // Connect back-reference for callbacks
        geneticManager.StartEvolution();   // Kick off the whole simulation process
    }

    public void OnGenerationComplete(List<Archer> pop, int gen)
    {
        float best = float.MinValue;
        float worst = float.MaxValue;
        float sumFitness = 0f;

        Archer bestArcher = null;
        Archer worstArcher = null;

        float sumAngle = 0f;
        float sumForce = 0f;
        float sumVariance = 0f;

        // Loop through current population to collect stats
        foreach (var archer in pop)
        {
            float f = archer.fitness;
            sumFitness += f;

            sumAngle += archer.angle;
            sumForce += archer.force;
            sumVariance += archer.variance;

            // Track best and worst based on fitness
            if (f > best)
            {
                best = f;
                bestArcher = archer;
            }

            if (f < worst)
            {
                worst = f;
                worstArcher = archer;
            }
        }

        // Average metrics for reporting
        float avg = sumFitness / pop.Count;
        float avgAngle = sumAngle / pop.Count;
        float avgForce = sumForce / pop.Count;
        float avgVar = sumVariance / pop.Count;

        // Push all results to CSV for later analysis
        csvExporter.LogGeneration(
            gen, pop.Count, best, avg, worst,
            geneticManager.wind, geneticManager.targetX.position.x,
            bestArcher.angle, bestArcher.force, bestArcher.variance,
            avgAngle, avgForce, avgVar,
            worstArcher.angle, worstArcher.force, worstArcher.variance
        );
    }
}
