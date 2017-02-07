using UnityEngine;
using System.Collections;

public class StatePatternBoss : MonoBehaviour {
    public float castingRadius;
    public float castingRange;
    public float turnSpeed;
    public float sightRange;
    public float timeToCasting;
    public Vector3 offset = new Vector3(0, .5f, 0);
    public LayerMask mask;

    [HideInInspector]
    public Transform chaseTarget;
    [HideInInspector]
    public IBossState currentState;
    [HideInInspector]
    public BossChaseState chaseState;
    [HideInInspector]
    public CastingState castingState;
    [HideInInspector]
    public BossCombatState combatState;
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public bool withinRange = false;
    [HideInInspector]
    public CC player;
    [HideInInspector]
    public BossData bossData;
    [HideInInspector]
    public bool startCasting;
    [HideInInspector]
    public float cd;
    [HideInInspector]
    public float attackCoolDown;

    private HPScript hps;

    private void Awake() {
        combatState = new BossCombatState(this);
        castingState = new CastingState(this);
        chaseState = new BossChaseState(this);

        bossData = Resources.Load("Data/" + gameObject.name) as BossData;
        player = FindObjectOfType<CC>();
        hps = GetComponent<HPScript>();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start() {
        currentState = chaseState;
    }

    // Update is called once per frame
    void Update() {
        Debug.DrawRay(transform.position, transform.forward * sightRange, Color.red);
        Debug.Log(currentState);
        if (!hps.rooted) {
            currentState.UpdateState();
        }

        cd = timeToCasting;
        cd -= Time.deltaTime;
        if(cd <= 0) {
            startCasting = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        currentState.OnTriggerEnter(other);
    }
}