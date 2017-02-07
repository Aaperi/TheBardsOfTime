using UnityEngine;
using System.Collections;

public class CastingState : IBossState {
    private readonly StatePatternBoss boss;
    private HPScript hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
    private Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    public CastingState(StatePatternBoss statePatternBoss) {
        boss = statePatternBoss;
    }

    public void UpdateState() {
        if (boss.startCasting) {
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
        float castSpell = boss.bossData.spell.CastTime;
        castSpell -= Time.deltaTime;

        if(castSpell <= 0) {
            Cast();
            boss.startCasting = false;
            boss.cd = boss.timeToCasting;
            castSpell = boss.bossData.spell.CastTime;
        }

    }

    void Cast() {
        hp.TakeDamage(boss.bossData.spell.Damage);
        ToChaseState();
    }
}
