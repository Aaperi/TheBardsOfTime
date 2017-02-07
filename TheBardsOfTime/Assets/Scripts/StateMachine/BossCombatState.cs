using UnityEngine;
using System.Collections;

public class BossCombatState : IBossState {

    private readonly StatePatternBoss boss;
    private HPScript hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
    private float attackCoolDown = .75f;
    private Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    public BossCombatState(StatePatternBoss statePatternBoss) {
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

    void Attack() {
        attackCoolDown -= Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(boss.transform.position, boss.transform.forward, out hit, boss.sightRange, boss.mask) && hit.collider.CompareTag("Player")) {
            if (attackCoolDown <= 0) {
                hp.TakeDamage(10f);
                attackCoolDown = .75f;
            }
        }
    }
}
