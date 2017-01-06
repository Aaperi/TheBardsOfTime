using UnityEngine;
using System.Collections;

public class CombatState : IEnemyState
{

    private readonly StatePatternEnemy enemy;
    private HPScript hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
    private float attackCoolDown = .75f;
    private Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

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
        else
        {
            Vector3 targetDir = player.position - enemy.transform.position;
            Vector3 newDir = Vector3.RotateTowards(enemy.transform.forward, targetDir, enemy.searchingTurnSpeed * Time.deltaTime, 0.0f);
            enemy.transform.rotation = Quaternion.LookRotation(newDir);
        }
            
    }

    void Attack()
    {
        attackCoolDown -= Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange, enemy.mask) && hit.collider.CompareTag("Player"))
        {
            if (attackCoolDown <= 0)
            {
                Debug.Log("Lyö");
                hp.TakeDamage(10f);
                attackCoolDown = .75f;
            }
        }

        if (enemy.navMeshAgent.destination != enemy.chaseTarget.position || enemy.navMeshAgent.remainingDistance > 4f)
        {
            ToChaseState();
        }

        if (hp.hitpoints <= 0)
        {
            ToPatrolState();
        }
    }
}
