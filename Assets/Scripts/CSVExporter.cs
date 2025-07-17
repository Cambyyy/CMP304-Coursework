using System.IO;
using UnityEngine;

public class CSVExporter : MonoBehaviour
{
    private string path;

    void Awake()
    {
        // Define the output file path and initialize the CSV with headers
        path = Application.dataPath + "/GA_results.csv";
        File.WriteAllText(path,
            "Generation,Population,BestFitness,AverageFitness,WorstFitness,Wind,TargetX," +
            "BestAngle,BestForce,BestVariance,AvgAngle,AvgForce,AvgVariance," +
            "WorstAngle,WorstForce,WorstVariance\n"
        );
    }

    public void LogGeneration(int gen, int pop, float best, float avg, float worst, float wind, float targetX,
                              float bestAngle, float bestForce, float bestVar,
                              float avgAngle, float avgForce, float avgVar,
                              float worstAngle, float worstForce, float worstVar)
    {
        // Format one row of data for the current generation
        string line = $"{gen + 1}, {pop},{best},{avg},{worst},{wind},{targetX}," +
                      $"{bestAngle},{bestForce},{bestVar}," +
                      $"{avgAngle},{avgForce},{avgVar}," +
                      $"{worstAngle},{worstForce},{worstVar}\n";

        // Append the data to the file
        File.AppendAllText(path, line);
    }
}
