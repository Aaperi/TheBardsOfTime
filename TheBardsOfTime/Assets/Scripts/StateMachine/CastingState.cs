using UnityEngine;
using System.Collections;

public class CastingState : IBossState {
    private readonly StatePatternBoss boss;
    private HPScript hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
    private float attackCoolDown = .75f;
    private Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    public CastingState(StatePatternBoss statePatternBoss) {
        boss = statePatternBoss;
    }

    public void UpdateState() {

    }

    public void OnTriggerEnter(Collider other) {

    }

    public void OnTriggerExit(Collider other) {

    }

    public void ToCastingState() {

    }

    public void ToCombatState() {

    }

    void Cast() {

    }
}
