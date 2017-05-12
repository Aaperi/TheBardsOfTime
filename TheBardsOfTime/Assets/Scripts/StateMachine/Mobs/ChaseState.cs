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

   /* public void OnTriggerEnter(Collider other) {

    }

    public void OnTriggerExit(Collider other) {
        ToAlertState();
    }*/

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
        Debug.DrawRay(enemy.eyes.position, enemy.eyes.forward.normalized * enemy.enemyStats.SightRange, Color.red);

        if (Physics.SphereCast(enemy.eyes.transform.position, enemy.enemyStats.SphereRadius, enemy.eyes.forward, out hit, enemy.mask) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
        }
    }

    private void Chase() {
        enemy.stateGizmoColor = Color.red;
        enemy.meshRendererFlag.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        if (enemy.navMeshAgent.remainingDistance < (enemy.enemyStats.AttackRange)) {
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
        } else
            enemy.navMeshAgent.isStopped = false;
    }

}
