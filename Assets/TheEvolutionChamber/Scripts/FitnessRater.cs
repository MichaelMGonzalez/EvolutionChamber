using UnityEngine;
using System.Collections;

public class FitnessRater : MonoBehaviour {


    public Vector3 forward;
    public Vector3 initialPosition;
    void OnEnable()
    {
        Reset();
    }
    public void Reset()
    {
        forward = transform.forward;
        initialPosition = transform.position;
    }
	// Use this for initialization
	void Start () {
	}

    public float MeasureFitness()
    {
        var displacement = initialPosition - transform.position;
        return (Vector3.Angle(displacement, forward));
    }
	
}
