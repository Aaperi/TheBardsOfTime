using UnityEngine;
using System.Collections;

public class StatePatternBoss : MonoBehaviour {
    public float castingTime;
    public float castingRadius;
    public float castingRange;
    public float sightRange;
    public LayerMask mask;

    [HideInInspector]
    public Transform chaseTarget;
    [HideInInspector]
    public IBossState currentState;
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

    private HPScript hps;

    private void Awake() {
        combatState = new BossCombatState(this);
        castingState = new CastingState(this);

        bossData = Resources.Load("Data/" + gameObject.name) as BossData;
        player = FindObjectOfType<CC>();
        hps = GetComponent<HPScript>();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start() {
        currentState = combatState;
    }

    // Update is called once per frame
    void Update() {
        if (!hps.rooted) {
            currentState.UpdateState();
        }
    }

    private void OnTriggerEnter(Collider other) {
        currentState.OnTriggerEnter(other);
    }
}