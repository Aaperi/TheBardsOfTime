using UnityEngine;
using System.Collections;

public class Violin : MonoBehaviour
{
    private Instrument ins;
    private Transform player;
    private bool isProcessing = false;
    private bool isChanneling = false;

    void Start()
    {
        ins = Resources.Load("Data/ViolinSO") as Instrument;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Attack()
    {
        if (!isProcessing && Time.time > ins.attack.Stamp) {
            StartCoroutine(AttackCou());
        }
    }

    void Skill()
    {
        if (!isProcessing && Time.time > ins.skill.Stamp) {
            StartCoroutine(SkillCou());
        }
    }

    void Spell()
    {
        if (!isProcessing && Time.time > ins.spell.Stamp) {
            StartCoroutine(SpellCou());
        }
    }

    bool HitCheck(GameObject target, string type)
    {
        float range = 0; float radius = 0;
        if (type == "Attack") { range = ins.attack.Range; radius = ins.attack.Radius; }
        if (type == "Skill") { range = ins.skill.Range; radius = ins.skill.Radius; }
        if (type == "Spell") { range = ins.spell.Range; radius = ins.spell.Radius; }

        if (Vector3.Distance(player.position, target.transform.position) <= range) {
            Vector3 targetDir = target.transform.position - player.position;
            if (Vector3.Angle(targetDir, player.forward) <= radius / 2) {
                return true;
            } else
                return false;
        } else
            return false;
    }

    IEnumerator AttackCou()
    {
        if (isChanneling) {
            isChanneling = false;
            yield return true;
        } else {
            isProcessing = true;
            Debug.Log("Violin Attack");
            yield return new WaitForSeconds(ins.attack.CastTime);
            Debug.Log("Viulu isku, hijaaa!");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject go in enemies)
                if (HitCheck(go, "Attack"))
                    go.GetComponent<HPScript>().TakeDamage(ins.attack.Damage);
            ins.attack.Stamp = Time.time + ins.attack.Cooldown;
            isProcessing = false;
        }
    }

    IEnumerator SkillCou()
    {
        if (isChanneling) {
            isChanneling = false;
            yield return true;
        } else {
            isProcessing = true;
            Debug.Log("Violin Skill");
            yield return new WaitForSeconds(ins.skill.CastTime);
            Debug.Log("ROOTS!");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject go in enemies)
                if (HitCheck(go, "Skill")) {
                    StartCoroutine(go.GetComponent<HPScript>().DOT(ins.skill.Duration, ins.skill.Damage));
                    StartCoroutine(go.GetComponent<HPScript>().Root(ins.skill.Duration));

                    if (ins.skill.Interrupt && go.GetComponent<StatePatternBoss>() != null) {
                        if (go.GetComponent<StatePatternBoss>().weakness.name == ins.name) {
                            go.GetComponent<StatePatternBoss>().castingState.Interrupt();
                        }
                    }
                }
            ins.skill.Stamp = Time.time + ins.skill.Cooldown;
            isProcessing = false;
        }
    }

    IEnumerator SpellCou()
    {
        if (isChanneling) {
            isChanneling = false;
            yield return true;
        } else {
            isChanneling = true;
            Debug.Log("Violin Spell");
            yield return new WaitForSeconds(ins.spell.CastTime);
            Debug.Log("Channelaus Alkaa");
            while (isChanneling) {
                yield return new WaitForSeconds(.5f);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject go in enemies)
                    if (HitCheck(go, "Spell")) {
                        go.GetComponent<HPScript>().TakeDamage(ins.spell.Damage / 2);
                        Debug.Log("vinku vonku");

                        if (ins.spell.Interrupt && go.GetComponent<StatePatternBoss>() != null) {
                            if (go.GetComponent<StatePatternBoss>().weakness.name == ins.name) {
                                go.GetComponent<StatePatternBoss>().castingState.Interrupt();
                            }
                        }
                    }
            }
            ins.spell.Stamp = Time.time + ins.spell.Cooldown;
            Debug.Log("Channelaus Loppuu");
        }
    }
}
