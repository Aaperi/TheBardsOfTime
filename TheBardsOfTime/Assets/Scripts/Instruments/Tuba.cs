using UnityEngine;
using System.Collections;

public class Tuba : MonoBehaviour
{
    private Instrument ins;
    private Transform player;
    private bool isProcessing = false;
    private bool isChanneling = false;

    void Start()
    {
        ins = Resources.Load("Data/TubaSO") as Instrument;
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
            Debug.Log("Tuba Attack");
            yield return true;
        }
    }

    IEnumerator SkillCou()
    {
        if (isChanneling) {
            isChanneling = false;
            yield return true;
        } else {
            Debug.Log("Tuba Skill");
            yield return true;
        }
    }

    IEnumerator SpellCou()
    {
        if (isChanneling) {
            isChanneling = false;
            yield return true;
        } else {
            Debug.Log("Tuba Spell");
            yield return true;
        }
    }
}
