using UnityEngine;
using System.Collections;

public class SpacetimeTest : MonoBehaviour {

	Rigidbody rb;
	public Vector3 impulse;
	public ForceMode mode;
	public string logFile = "spacetime.csv";
	public int count = 0;
	public int maxCount = 300;
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (impulse, mode);
	}
	
	void FixedUpdae () {
		if (count++ < maxCount)
			FileWriter.LogVector (logFile, transform.position);
		else
			FileWriter.singleton.sw.Close ();

	}
}
