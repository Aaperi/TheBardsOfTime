using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState {
    private readonly StatePatternEnemy enemy;
    private int nextWayPoint;

    public PatrolState(StatePatternEnemy statePatternEnemy) {
        enemy = statePatternEnemy;
    }

    public void UpdateState() {
        Look();
        Patrol();
    }

   /* public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            enemy.withinRange = true;
            ToAlertState();
        }

    }

    public void OnTriggerExit(Collider other) {
        enemy.withinRange = false;
    }*/

    public void ToPatrolState() {
        Debug.Log("Can't transition to same state");
    }

    public void ToAlertState() {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState() {
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState() {

    }

    private void Look() {
        RaycastHit hit;
        Debug.DrawRay(enemy.eyes.position, enemy.eyes.forward.normalized * enemy.enemyStats.search.SphereRadius, Color.green);

        if (Physics.SphereCast(enemy.eyes.transform.position, enemy.enemyStats.search.SphereRadius, enemy.eyes.forward, out hit, enemy.mask) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    void Patrol() {
        enemy.meshRendererFlag.material.color = Color.green;
        enemy.navMeshAgent.destination = enemy.script.Path[nextWayPoint].transform.position;
        enemy.navMeshAgent.isStopped = false;

        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending) {
            nextWayPoint = (nextWayPoint + 1) % enemy.script.Path.Count;
        }
    }

    /*if(Physics.SphereCast(controller.eyes.position, controller.enemyStats.search.SphereRadius, controller.eyes.forward, out hit, controller.enemyStats.attack.AttackRange)
           && hit.collider.CompareTag("Player"))
        {
            if (controller.CheckIfCountDownElapsed(controller.enemyStats.attack.Cooldown))
            {
                controller.hpScript.TakeDamage(controller.enemyStats.attack.Damage);
            }
        }*/
}
