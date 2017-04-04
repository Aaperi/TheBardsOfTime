using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState {

    private readonly StatePatternEnemy enemy;

    public ChaseState(StatePatternEnemy statePatternEnemy) {
        enemy = statePatternEnemy;
    }

    public void UpdateState() {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other) {

    }

    public void OnTriggerExit(Collider other) {
        ToAlertState();
    }

    public void ToPatrolState() {
        enemy.currentState = enemy.patrolState;
    }

    public void ToAlertState() {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState() {

    }

    public void ToAttackState() {
        enemy.currentState = enemy.combatState;
        enemy.player.inCombat = true;
    }

    private void Look() {
        RaycastHit hit;
        Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.offset) - enemy.eyes.transform.position;
        if (Physics.Raycast(enemy.eyes.transform.position, enemyToTarget, out hit, enemy.enemyStats.search.SightRange, enemy.mask) && hit.collider.CompareTag("Player")) {
            enemy.chaseTarget = hit.transform;
        } else 
            ToAlertState();
        
    }

    private void Chase() {
        enemy.meshRendererFlag.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        if (enemy.navMeshAgent.remainingDistance < (enemy.enemyStats.attack.AttackRange)) {
            enemy.navMeshAgent.Stop();
            ToAttackState();
        } else
            enemy.navMeshAgent.Resume();
    }
}
