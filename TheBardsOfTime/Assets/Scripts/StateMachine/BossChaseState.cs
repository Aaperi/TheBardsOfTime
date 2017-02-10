using UnityEngine;
using System.Collections;

public class BossChaseState : IBossState {
    private readonly StatePatternBoss boss;

    public BossChaseState(StatePatternBoss statePatternBoss) {
        boss = statePatternBoss;
    }

    public void UpdateState() {
        if (boss.startCasting) {
            ToCastingState();
        }
        else {
            Look();
            Chase();
        }
    }

    public void OnTriggerEnter(Collider other) {
        boss.player.inCombat = true;
    }

    public void OnTriggerExit(Collider other) {

    }

    public void ToChaseState() {

    }

    public void ToCombatState() {
        boss.currentState = boss.combatState;
    }

    public void ToCastingState() {
        boss.currentState = boss.castingState;
    }

    void Look() {
        RaycastHit hit;
        Vector3 enemyToTarget = (boss.player.transform.position + boss.offset) - boss.transform.position;
        if (Physics.Raycast(boss.transform.position, enemyToTarget, out hit, boss.sightRange, boss.mask) && hit.collider.CompareTag("Player")) {
            boss.chaseTarget = hit.transform;
        }
    }

    void Chase() {
        if (boss.navMeshAgent.remainingDistance < 4.1f) {
            boss.navMeshAgent.Stop();
            ToCombatState();
        } else
            boss.navMeshAgent.Resume();
    }
}
