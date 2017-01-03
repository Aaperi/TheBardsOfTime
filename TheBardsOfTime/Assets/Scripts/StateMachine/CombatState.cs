using UnityEngine;
using System.Collections;

public class CombatState : IEnemyState {

    private readonly StatePatternEnemy enemy;
    private HPScript hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
    private float attackCoolDown = .75f;

    public CombatState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Attack();
    }

    public void OnTriggerEnter(Collider other)
    {
       
    }

    public void OnTriggerExit(Collider other)
    {
        
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToAttackState()
    {
        Debug.Log("Can't transition to same state");
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange, enemy.mask) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    void Attack()
    {
        attackCoolDown -= Time.deltaTime;

        if(attackCoolDown <= 0)
        {
            Debug.Log("Lyö");
            hp.TakeDamage(10f);
            attackCoolDown = .75f;
        }

        if(enemy.navMeshAgent.destination != enemy.chaseTarget.position || enemy.navMeshAgent.remainingDistance > 4.1f)
        {
            ToChaseState();
        }

        if(hp.hitpoints <= 0)
        {
            ToPatrolState();
        }
    }
}
