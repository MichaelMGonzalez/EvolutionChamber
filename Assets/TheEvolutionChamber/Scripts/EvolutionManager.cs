using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EvolutionManager : MonoBehaviour {

    [Header("Experiment Settings")]
    public GameObject baseCreature;
    public float evaluationTime;
    [Range(0,100)]
    public int evaluationSpeed = 50;

    [Header("Genetic Pool Parameters")]
    public int populationSize;

    [Range(0,1)]
    public float mutationRate = 0.02f;

    [Range(0,1)]
    public float crossoverRate = 0.02f;

    [Header("Active Population Objects")]
    public GameObject[] creatures;
    public GameObject activeObject;

    [Header("GUI Elements")]
    public Text generationLabel;
    public Text bestFitnessLabel, uptimeLabel;
    public Slider progressBar;

    private string generationDefault, fitnessDefault, uptimeDefault;
    private int idx;
    private float bestFitness;

    void SetupGUI()
    {
        generationDefault = generationLabel.text;
        fitnessDefault = bestFitnessLabel.text;
        uptimeDefault = uptimeLabel.text;
    }

	void Start () {
        SetupGUI();
        idx = 0;
        creatures = new GameObject[populationSize];
        CreateNewCreature();
        InvokeRepeating("EvaluateActiveCreatures", evaluationTime, evaluationTime);
        Time.timeScale = evaluationSpeed;
        generationLabel.text = generationDefault + 0;
	}
    void Update()
    {
        uptimeLabel.text = uptimeDefault + Time.unscaledTime;
    }

    void EvaluateActiveCreatures()
    {
        activeObject.SetActive(false);
        var rater = activeObject.GetComponent<FitnessRater>();
        var rating = (rater.MeasureFitness());
        activeObject.name = "Fitness: " + rating;
        activeObject.transform.SetParent(transform);
        if(rating > bestFitness)
        {
            bestFitness = rating;
            bestFitnessLabel.text = fitnessDefault + rating;
        }
        Debug.Log(activeObject.name);
        CreateNewCreature();
        idx++;
        progressBar.value = (idx * 1.0f / populationSize);
    }

    void CreateNewCreature()
    {
        activeObject = (GameObject)GameObject.Instantiate(baseCreature, baseCreature.transform.position, baseCreature.transform.rotation);
        activeObject.SetActive( true );
    }
	
}
