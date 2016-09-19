using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvolutionCreature : MonoBehaviour {


    public Transform[] jointLocations;
    public GameObject mirrorSide;
    public GameObject joint;
    public int totalJoints;
    public bool createMirror;
    public bool shouldMirror;
    public bool isBase;
    private CapsuleCollider myCollider;
    private Rigidbody rb;
    public GameObject connectedObject;

    void Awake()
    {
        myCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start() {
        if (connectedObject != null)
            AttachTo(connectedObject);
        if(isBase)
            CreateJoints(totalJoints);
        CreateMirror();
        enabled = false;
	}


    void CreateMirror()
    {
        if (!createMirror) return;
        GameObject side = (GameObject)GameObject.Instantiate(mirrorSide, mirrorSide.transform.parent);
        side.name = side.name.Remove(mirrorSide.name.Length);
        side.transform.localScale = new Vector3(-1, 1, 1);
    }

    void CreateJoints(int totalJoints)
    {
        int numOfTotalJointsRemaining = (shouldMirror ? (totalJoints / 2) + (totalJoints % 2) : totalJoints);
        List<Transform> validTransforms = GetValidTransforms();
        int maxJointsAtLayer = Mathf.Min(numOfTotalJointsRemaining, validTransforms.Count);
        if(maxJointsAtLayer == 0) return;
        int numberOfJointsAtLayer = Random.Range(1, maxJointsAtLayer+1);
        List<EvolutionCreature> newParts = CreateNewJoints(numberOfJointsAtLayer, ref validTransforms);
        // Distribute the remaining joints
        int remainingJoints = numOfTotalJointsRemaining - numberOfJointsAtLayer;
        DistributeRemainingJoints(newParts, remainingJoints);
    }

    void DistributeRemainingJoints(List<EvolutionCreature> newParts, int remainingJoints)
    {
        foreach (EvolutionCreature p in newParts)
        {
            int numberOfJoints = Random.Range(0, remainingJoints + 1);
            p.totalJoints = numberOfJoints;
            remainingJoints -= numberOfJoints;
        }
        if (remainingJoints > 0)
            newParts[Random.Range(0, newParts.Count)].totalJoints += remainingJoints;
        foreach (EvolutionCreature p in newParts)
        {
            p.CreateJoints(p.totalJoints);
        }
    }


    void AttachTo(GameObject other)
    {
        CharacterJoint newJoint = other.AddComponent<CharacterJoint>();
        newJoint.connectedBody = rb;
        other.GetComponent<EvolutionJoint>().ConnectWith(this.GetComponent<EvolutionJoint>());
    }

    List<EvolutionCreature> CreateNewJoints(int numberOfJoints, ref List<Transform> validTs)
    {
        List<EvolutionCreature> newParts = new List<EvolutionCreature>(); 
        for(int i = 0; i < numberOfJoints; i++ )
        {
            Transform tCurr = GetRandomTransform(ref validTs);
            EvolutionCreature p = CreateNewJointAt(tCurr);
            p.createMirror = false;
            p.shouldMirror = p.transform.parent.parent.name == mirrorSide.name;
            newParts.Add(p);
        }
        return newParts;
    }

    EvolutionCreature CreateNewJointAt( Transform t )
    {
        GameObject oCurr = GameObject.Instantiate(joint);
        SetJointToTransform(oCurr, t);
        EvolutionCreature rv = oCurr.GetComponent<EvolutionCreature>();
        rv.connectedObject = gameObject;
        return rv;
    }

    void SetJointToTransform( GameObject o, Transform t )
    {
        o.transform.parent = t;
        o.transform.localPosition = Vector3.zero;
        o.transform.localRotation = Quaternion.identity;
    }

    Transform GetRandomTransform( ref List<Transform> ts)
    {
        int randIdx = Random.Range(0, ts.Count);
        Transform t = ts[randIdx];
        ts.RemoveAt(randIdx);
        return t;
    }

    List<Transform> GetValidTransforms()
    {
        List<Transform> validTransforms = new List<Transform>();
        var colliderOffset = (myCollider.height / 2 - myCollider.radius);
        foreach (var t in jointLocations)
        {
            var p1 = t.position + (t.forward * colliderOffset);
            var p2 = t.position - (t.forward * colliderOffset);
            if (!Physics.CheckCapsule(p1, p2, myCollider.radius))
                validTransforms.Add(t);
        }
        return validTransforms;
    }

	
}
