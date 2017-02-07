using UnityEngine;
using System.Collections;

public class BossCombatState : IBossState {

    private readonly StatePatternBoss boss;
    private HPScript hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
    private Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    public BossCombatState(StatePatternBoss statePatternBoss) {
        boss = statePatternBoss;
    }

    public void UpdateState() {
        if (boss.startCasting) {
            ToCastingState();
        } else {
            Look();
            Attack();
        }
    }

    public void OnTriggerEnter(Collider other) {

    }

    public void OnTriggerExit(Collider other) {

    }

    public void ToCastingState() {
        boss.currentState = boss.castingState;
    }

    public void ToCombatState() {
        boss.currentState = boss.combatState;
    }

    public void ToChaseState() {
        boss.currentState = boss.chaseState;
    }

    private void Look() {
        RaycastHit hit;
        if (Physics.Raycast(boss.transform.position, boss.transform.forward, out hit, boss.sightRange, boss.mask) && hit.collider.CompareTag("Player")) {
            boss.chaseTarget = hit.transform;
            ToChaseState();
        } else {
            Vector3 targetDir = player.position - boss.transform.position;
            Vector3 newDir = Vector3.RotateTowards(boss.transform.forward, targetDir, boss.turnSpeed * Time.deltaTime, 0.0f);
            boss.transform.rotation = Quaternion.LookRotation(newDir);
        }

    }

    void Attack() {
        float attcd = boss.bossData.attack.Cooldown;
        attcd -= Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(boss.transform.position, boss.transform.forward, out hit, boss.sightRange, boss.mask) && hit.collider.CompareTag("Player")) {
            if (attcd <= 0) {
                hp.TakeDamage(boss.bossData.attack.Damage);
                attcd = boss.bossData.attack.Cooldown;
            }
        }

        float startCasting;
        startCasting = boss.bossData.spell.Cooldown;
        startCasting -= Time.deltaTime;
        if(startCasting <= 0) {
            ToCastingState();
            startCasting = boss.bossData.spell.Cooldown;
        }
    }
}
