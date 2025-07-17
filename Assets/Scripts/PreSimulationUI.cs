using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreSimulationUI : MonoBehaviour
{
    public GeneticManager geneticManager;
    public SimulationManager simulationManager;

    public Slider populationSlider;
    public Slider mutationSlider;
    public Slider generationSlider;
    public Slider maxWindSlider;
    public Slider maxTargetMoveSlider;
    public Slider timeScaleSlider;

    public TextMeshProUGUI populationLabel;
    public TextMeshProUGUI mutationLabel;
    public TextMeshProUGUI generationLabel;
    public TextMeshProUGUI windLabel;
    public TextMeshProUGUI targetLabel;
    public TextMeshProUGUI timeScaleLabel;

    public Button startButton;
    public Button stopButton;
    public Button restartButton;
    public Button exitButton;
    public Toggle randomWind;
    public Toggle movingTarget;

    void Start()
    {
        // Set up button callbacks
        startButton.onClick.AddListener(StartSimulation);
        stopButton.onClick.AddListener(StopSimulation);
        restartButton.onClick.AddListener(RestartSimulation);
        exitButton.onClick.AddListener(ExitGame);

        // Update labels whenever sliders change
        populationSlider.onValueChanged.AddListener((v) => UpdateLabels());
        mutationSlider.onValueChanged.AddListener((v) => UpdateLabels());
        generationSlider.onValueChanged.AddListener((v) => UpdateLabels());
        maxWindSlider.onValueChanged.AddListener((v) => UpdateLabels());
        maxTargetMoveSlider.onValueChanged.AddListener((v) => UpdateLabels());
        timeScaleSlider.onValueChanged.AddListener((v) => UpdateLabels());

        UpdateLabels(); // Show default values on launch
    }

    void UpdateLabels()
    {
        populationLabel.text = $"Population: {(int)populationSlider.value}";
        mutationLabel.text = $"Mutation Rate: {mutationSlider.value:F2}";
        generationLabel.text = $"Generations: {(int)generationSlider.value}";
        windLabel.text = $"Max Wind: {maxWindSlider.value:F2}";
        targetLabel.text = $"Max Target Movement: {maxTargetMoveSlider.value:F1}";
        timeScaleLabel.text = $"Time Scale:  {timeScaleSlider.value:F1}";
    }

    void StartSimulation()
    {
        // Push current UI values to the genetic manager before starting
        geneticManager.populationSize = (int)populationSlider.value;
        geneticManager.mutationRate = mutationSlider.value;
        geneticManager.generations = (int)generationSlider.value;
        geneticManager.windMax = maxWindSlider.value;
        geneticManager.targetMoveMax = maxTargetMoveSlider.value;
        geneticManager.timeScale = timeScaleSlider.value;
        geneticManager.targetMove = movingTarget.isOn;
        geneticManager.randomWind = randomWind.isOn;

        simulationManager.StartSimulation();
    }

    void StopSimulation()
    {
        // Stops current simulation loop
        geneticManager.StopAllCoroutines();
    }

    void RestartSimulation()
    {
        // Stops and immediately restarts
        geneticManager.StopAllCoroutines();
        simulationManager.StartSimulation();
    }

    void ExitGame()
    {
        Application.Quit(); // Quit the application — note: won’t work in editor
    }
}
