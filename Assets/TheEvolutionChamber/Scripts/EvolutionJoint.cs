using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvolutionJoint : MonoBehaviour {

    public Vector3 forceDirection;
    public float fireRate;
    public float timeOffset;
    public float maxForce;
    public List<EvolutionJoint> joints;
    public bool mutateDirection;
    public bool mutateTimeOffset;
    private Rigidbody rb;

    void Awake()
    {
        joints = new List<EvolutionJoint>();
        ApplyMutations();
    }

    void Start () {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("FireForce", timeOffset, fireRate);
	}
	
    public void FireForce()
    {
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }

    public void ApplyMutations()
    {
        if(TestAndSet(ref mutateDirection, false))
            forceDirection = RNG.UniformRandomVector(-maxForce, maxForce);
        if(TestAndSet(ref mutateTimeOffset, false))
            timeOffset = Random.Range(0, fireRate);
    }

    internal void ConnectWith(EvolutionJoint evolutionJoint)
    {
        joints.Add(evolutionJoint);
    }

    public bool TestAndSet(ref bool variable, bool val) {
        bool rv = variable;
        variable = val;
        return rv;
    }
}
