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
        Look();
        Search();
    }

    public void OnTriggerEnter(Collider other) {

    }

    public void OnTriggerExit(Collider other) {
        ToPatrolState();
    }

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
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange, enemy.mask) && hit.collider.CompareTag("Player")) {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    private void Search() {
        Vector3 targetDir = player.position - enemy.transform.position;
        enemy.meshRendererFlag.material.color = Color.yellow;
        enemy.navMeshAgent.Stop();
        Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDir, enemy.searchingTurnSpeed * Time.deltaTime, 0.0f);
        enemy.transform.rotation = Quaternion.LookRotation(newDir);

        searchTimer += Time.deltaTime;

        if (searchTimer >= enemy.searchingDuration)
            ToPatrolState();
    }
}
