using UnityEngine;
using System.Collections;

public class AlertState : IEnemyState {
    private readonly StatePatternEnemy enemy;
    private float searchTimer;
    private Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    public AlertState(StatePatternEnemy statePatternEnemy) {
        enemy = statePatternEnemy;
    }

    public void UpdateState() {
        if (enemy.withinRange)
            Look();
        else
            Search();
    }

    /*public void OnTriggerEnter(Collider other) {
        enemy.withinRange = true;
    }

    public void OnTriggerExit(Collider other) {
        enemy.withinRange = false;
    }*/

    public void ToPatrolState() {
        enemy.currentState = enemy.patrolState;
        searchTimer = 0f;
        enemy.player.inCombat = false;
    }

    public void ToAlertState() {
        Debug.Log("Can't transition to same state");
    }

    public void ToChaseState() {
        enemy.currentState = enemy.chaseState;
        searchTimer = 0f;
        enemy.player.inCombat = true;
    }

    public void ToAttackState() {

    }

    private void Look() {
        Vector3 targetDir = player.position - enemy.transform.position;
        enemy.meshRendererFlag.material.color = Color.yellow;
        enemy.navMeshAgent.isStopped = true;
        Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDir, enemy.enemyStats.search.SearchSpeed * Time.deltaTime, 0.0f);
        enemy.transform.rotation = Quaternion.LookRotation(newDir);

        Debug.DrawRay(enemy.eyes.position, enemy.eyes.forward.normalized * enemy.enemyStats.search.SphereRadius, Color.yellow);

        RaycastHit hit;
        if (Physics.SphereCast(enemy.eyes.transform.position, enemy.enemyStats.search.SphereRadius, enemy.eyes.forward, out hit, enemy.mask) && hit.collider.CompareTag("Player")) {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    private void Search() {
        enemy.meshRendererFlag.material.color = Color.yellow;
        enemy.navMeshAgent.isStopped = true;
        enemy.transform.Rotate(0, enemy.enemyStats.search.SearchSpeed * Time.deltaTime, 0); 


        searchTimer += Time.deltaTime;

        if (searchTimer >= enemy.enemyStats.search.SearchDuration)
            ToPatrolState();
    }

}
