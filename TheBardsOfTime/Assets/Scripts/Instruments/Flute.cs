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
        else {
            float dur = 0;
            Debug.Log("Hiirulaiset!");
            ins.skill.Stamp = Time.time + ins.skill.Cooldown + ins.skill.Duration;
            while (dur < ins.skill.Duration) {
                Debug.Log("Squeek!");
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject go in enemies)
                    if (HitCheck(go, ins.skill.Range, ins.skill.Radius)) {
                        go.GetComponent<HPScript>().TakeDamage(ins.skill.Damage / 2);
                        StartCoroutine(go.GetComponent<HPScript>().Slow(.5f, ins.skill.Potency));
                    }
                yield return new WaitForSeconds(.5f);
                dur += .5f;
            }
            Debug.Log("Hei hei hiiret");
        }
    }

    IEnumerator SpellCou()
    {
        if (isChanneling)
            isChanneling = false;
<<<<<<< HEAD
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
                    if (HitCheck(go, ins.spell.Range, ins.spell.Radius))
                        temp.Add(go);
                temp.Sort(delegate (GameObject a, GameObject b) {
                    float distA = Vector3.Distance(a.transform.position, player.transform.position);
                    float distB = Vector3.Distance(b.transform.position, player.transform.position);
                    return distA.CompareTo(distB);
                });
                if (temp.Count > 0)
                    temp[0].GetComponent<HPScript>().TakeDamage(ins.spell.Damage / 2);
            }
            Debug.Log("Channelaus Loppuu");
        }
=======
        yield return true;
        Debug.Log("Flute Spell");
>>>>>>> parent of 9d0f1ee... rupesin tekeen huiluu
    }

}
