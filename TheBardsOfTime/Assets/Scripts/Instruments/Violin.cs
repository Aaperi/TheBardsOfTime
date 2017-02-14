using UnityEngine;
using System.Collections;

public class Violin : MonoBehaviour
{
    private Instrument ins;
    private HitDetection[] Colliders;
    private bool isProcessing = false;
    private bool isChanneling = false;
    private bool isEquipped = false;

    void Start()
    {
        ins = Resources.Load("Data/ViolinSO") as Instrument;
        Colliders = gameObject.GetComponentsInChildren<HitDetection>();
    }

    void Equip()
    {
        isEquipped = true;
    }

    void UnEquip()
    {
        isEquipped = false;
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

    IEnumerator AttackCou()
    {
        if (isChanneling)
            isChanneling = false;
        else {
            isProcessing = true;
            Debug.Log("Normal Attack!");
            ins.attack.Stamp = Time.time + ins.attack.Cooldown;
            yield return new WaitForSeconds(ins.attack.CastTime);
            foreach (GameObject go in Colliders[0].enemyList) {
                try { go.SendMessageUpwards("TakeDamage", ins.attack.Damage, SendMessageOptions.DontRequireReceiver); Debug.Log(go.name + " takes " + ins.attack.Damage + "damage"); } catch { Debug.Log("ATTACK FAILS!"); }
            }
            isProcessing = false;
        }
    }

    IEnumerator SkillCou()
    {
        if (isChanneling)
            isChanneling = false;
        else {
            isProcessing = true;
            Debug.Log("Roots!");
            ins.skill.Stamp = Time.time + ins.skill.Cooldown;
            yield return new WaitForSeconds(ins.skill.CastTime);
            foreach (GameObject go in Colliders[1].enemyList) {
                try {
                    go.GetComponent<HPScript>().DOT(5, ins.skill.Damage);
                    go.GetComponent<HPScript>().Root(5f);
                    Debug.Log(go.name + " takes " + ins.skill.Damage / 5 + "damage");
                } catch { Debug.Log("SKILL FAILS!"); }
            }
            isProcessing = false;
        }
    }

    IEnumerator SpellCou()
    {
        if (isChanneling)
            isChanneling = false;
        else {
            isChanneling = true;
            Debug.Log("Channelaus Alkaa");
            ins.spell.Stamp = Time.time + ins.spell.Cooldown;
            yield return new WaitForSeconds(ins.spell.CastTime);
            while (isChanneling) {
                yield return new WaitForSeconds(.5f);
                foreach (GameObject go in Colliders[2].enemyList) {
                    try {
                        go.GetComponent<HPScript>().TakeDamage(ins.spell.Damage / 2);
                        Debug.Log(go.name + " takes " + ins.spell.Damage / 2 + "damage");
                    } catch { Debug.Log("SPELL FAILS!"); }
                }
            }
            Debug.Log("Channelaus Loppuu");
        }
    }
}
