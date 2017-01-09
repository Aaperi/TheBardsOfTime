using UnityEngine;
using System.Collections;

public class Flute : MonoBehaviour
{
    private CC CCref;
    private float normalSpeed;
    private Instrument ins;
    private HitDetection[] Colliders;
    private bool isProcessing = false;
    private bool isChanneling = false;
    private bool isEquipped = false;

    void Start()
    {
        CCref = GameObject.Find("Player").GetComponent<CC>();
        normalSpeed = CCref.moveSetting.forwardVel;
        ins = Resources.Load("Data/FluteSO") as Instrument;
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
