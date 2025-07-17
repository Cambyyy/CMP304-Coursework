using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHandler : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public TextMeshProUGUI textMeshProUGUI2;
    public TextMeshProUGUI textMeshProUGUI3;

    string text;
    public GeneticManager manager;

    // Start is called before the first frame update
    void Start()
    {
        text = $"Current Generation: {manager.currentGeneration}";
        textMeshProUGUI.text = text;

        text = $"Current Wind: {manager.wind}";
        textMeshProUGUI2.text = text;

        text = $"Current Target X Coord: {manager.targetX.position.x}";
        textMeshProUGUI3.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        // +1 to currentGeneration for UI display (user-friendly indexing)
        text = $"Current Generation: {manager.currentGeneration + 1}";
        textMeshProUGUI.text = text;

        text = $"Current Wind: {manager.wind}";
        textMeshProUGUI2.text = text;

        text = $"Current Target X Coord: {manager.targetX.position.x}";
        textMeshProUGUI3.text = text;
    }
}
