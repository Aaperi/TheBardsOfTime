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

    bool HitCheck(GameObject target, float range, float radius)
    {
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
            Debug.Log("Viulu isku, hijaaa!");
            ins.attack.Stamp = Time.time + ins.attack.Cooldown;
            yield return new WaitForSeconds(ins.attack.CastTime);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject go in enemies)
                if (HitCheck(go, ins.attack.Range, ins.attack.Radius)) {
                    go.GetComponent<HPScript>().TakeDamage(ins.attack.Damage);
                }
                    
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
            Debug.Log("ROOTS!");
            ins.skill.Stamp = Time.time + ins.skill.Cooldown;
            yield return new WaitForSeconds(ins.skill.CastTime);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject go in enemies)
                if (HitCheck(go, ins.skill.Range, ins.skill.Radius)) {
                    StartCoroutine(go.GetComponent<HPScript>().DOT(ins.skill.Duration, ins.skill.Damage));
                    StartCoroutine(go.GetComponent<HPScript>().Root(ins.skill.Duration));

                    if (ins.skill.Interrupt && go.GetComponent<StatePatternBoss>() != null) {
                        if (go.GetComponent<StatePatternBoss>().weakness.name == ins.name) {
                            go.GetComponent<StatePatternBoss>().castingState.Interrupt();
                        }
                    }
                }
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
            Debug.Log("Castaaminen Alkaa");
            ins.spell.Stamp = Time.time + ins.spell.Cooldown;
            yield return new WaitForSeconds(ins.spell.CastTime);
            Debug.Log("Channelaus Alkaa");
            while (isChanneling) {
                yield return new WaitForSeconds(.5f);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject go in enemies)
                    if (HitCheck(go, ins.spell.Range, ins.spell.Radius)) {
                        go.GetComponent<HPScript>().TakeDamage(ins.spell.Damage / 2);
                        Debug.Log("vinku vonku");

                        if (ins.spell.Interrupt && go.GetComponent<StatePatternBoss>() != null) {
                            if (go.GetComponent<StatePatternBoss>().weakness.name == ins.name) {
                                go.GetComponent<StatePatternBoss>().castingState.Interrupt();
                            }
                        }
                    }
            }
            Debug.Log("Channelaus Loppuu");
        }
    }
}
