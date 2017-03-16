using UnityEngine;
using System.Collections;

public class CastingState : IBossState {
    private readonly StatePatternBoss boss;
    private HPScript hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
    private Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    public float castSpell;

    public CastingState(StatePatternBoss statePatternBoss) {
        boss = statePatternBoss;
    }

    public void UpdateState() {
        if (boss.startCasting) {
            boss.ps.Play();
            Casting();
        } 
    }

    public void OnTriggerEnter(Collider other) {

    }

    public void OnTriggerExit(Collider other) {

    }

    public void ToCastingState() {

    }

    public void ToCombatState() {
        boss.currentState = boss.combatState;
    }

    public void ToChaseState() {
        boss.currentState = boss.chaseState;
    }

    void Casting() {
        boss.navMeshAgent.Stop();
        castSpell -= Time.deltaTime;
        if(castSpell <= 0) {
            Cast();
            boss.startCasting = false;
            boss.cd = boss.bossData.spell.timeToCasting; // cd = koska bossi aloittaa seuraavan castingin
            castSpell = boss.bossData.spell.CastTime;
        }
    }

    void Cast() {
        Vector3 targetDir = player.position - boss.transform.position;
        if(Vector3.Distance(boss.transform.position, boss.player.transform.position) <= boss.bossData.spell.castingRange && Vector3.Angle(targetDir, boss.transform.forward) <= (boss.bossData.spell.castingRadius / 2)) {
            hp.TakeDamage(boss.bossData.spell.Damage);
        }
        ToChaseState();
    }

    public void Interrupt() {
        if(boss.startCasting) {
            Debug.Log("Interrupted");
            boss.startCasting = false;
            boss.cd = boss.bossData.spell.timeToCasting;
            castSpell = boss.bossData.spell.CastTime;
            boss.ps.Stop();
            ToChaseState();
        }
    }
}
