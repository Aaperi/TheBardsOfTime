﻿using UnityEngine;
using System.Collections;

public class StatePatternEnemy : MonoBehaviour {

    public float searchingTurnSpeed;
    public float searchingDuration;
    public float sightRange;
    //public Transform[] wayPoints;
    public Transform eyes;
    public Vector3 offset = new Vector3(0, .5f, 0);
    public MeshRenderer meshRendererFlag;
    public LayerMask mask;
    public PathScript script;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public CombatState combatState;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public bool withinRange = false;

    private void Awake()
    {
        combatState = new CombatState(this);
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        patrolState = new PatrolState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
	// Use this for initialization
	void Start ()
    {
        currentState = patrolState;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawRay(eyes.position, eyes.forward * sightRange, Color.red);
        currentState.UpdateState();
	}

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}