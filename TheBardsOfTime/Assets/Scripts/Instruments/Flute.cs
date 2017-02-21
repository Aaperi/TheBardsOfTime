using UnityEngine;
using System.Collections;

public class Flute : MonoBehaviour
{
    private CC CCref;
    private Instrument ins;
    private bool isProcessing = false;
    private bool isChanneling = false;

    void Start()
    {
        CCref = FindObjectOfType<CC>();
        ins = Resources.Load("Data/FluteSO") as Instrument;
    }

    void OnEnable()
    {
        if (CCref != null)
            CCref.moveSetting.forwardVel *= 1.5f;
    }

    void OnDisable()
    {
        if (CCref != null)
            CCref.moveSetting.forwardVel /= 1.5f;
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
        yield return true;
        Debug.Log("Flute Spell");
    }

}
