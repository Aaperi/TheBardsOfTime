using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flute : MonoBehaviour
{
    private CC CCref;
    private Instrument ins;
    private Transform player;
    private bool isProcessing = false;
    private bool isChanneling = false;

    void Start()
    {
        CCref = FindObjectOfType<CC>();
        ins = Resources.Load("Data/FluteSO") as Instrument;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnEnable()
    {
        if (CCref != null)
            CCref.moveSetting.forwardVel *= 1.2f;
    }

    void OnDisable()
    {
        if (CCref != null)
            CCref.moveSetting.forwardVel /= 1.2f;
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
        if (isChanneling)
            isChanneling = false;
        yield return true;
        Debug.Log("Flute Attack");
    }

    IEnumerator SkillCou()
    {
        if (isChanneling)
            isChanneling = false;
        yield return true;
        Debug.Log("Flute Skill");
    }

    IEnumerator SpellCou()
    {
        if (isChanneling)
            isChanneling = false;
        else {
            isChanneling = true;
            Debug.Log("Castaaminen Alkaa");
            ins.spell.Stamp = Time.time + ins.spell.Cooldown;
            yield return new WaitForSeconds(ins.spell.CastTime);
            Debug.Log("Channelaus Alkaa");
            while (isChanneling) {
                yield return new WaitForSeconds(.5f);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                List<GameObject> temp = new List<GameObject>();
                foreach (GameObject go in enemies)
                    if (HitCheck(go, ins.spell.Range, ins.spell.Radius)) {
                        temp.Add(go);
                    }
                temp.Sort(delegate (GameObject a, GameObject b) {
                    float distA = Vector3.Distance(a.transform.position, player.transform.position);
                    float distB = Vector3.Distance(b.transform.position, player.transform.position);
                    return distA.CompareTo(distB);
                });
                if (temp.Count > 0)
                    temp[0].GetComponent<HPScript>().TakeDamage(ins.spell.Damage);
            }
            Debug.Log("Channelaus Loppuu");
        }
    }

}
