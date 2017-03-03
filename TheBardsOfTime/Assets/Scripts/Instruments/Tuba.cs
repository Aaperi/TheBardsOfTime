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

    void OnDisable()
    {
        if (transform.root.gameObject.GetComponent<HPScript>() != null)
            transform.root.gameObject.GetComponent<HPScript>().amp = 1;
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
        if (!isChanneling) {
            isProcessing = true;
            Debug.Log("Tuba Attack");
            yield return new WaitForSeconds(ins.attack.CastTime);
            Debug.Log("Täältä pesee senkin vatipää!");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject go in enemies)
                if (HitCheck(go, "Attack")) {
                    go.GetComponent<HPScript>().TakeDamage(ins.attack.Damage);
                    StartCoroutine(go.GetComponent<HPScript>().Stun(ins.attack.Duration));
                }
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
            Debug.Log("Tuba Skill");
            yield return new WaitForSeconds(ins.skill.CastTime);
            isChanneling = true;
            Debug.Log("Kilpi!");
            transform.root.gameObject.GetComponent<HPScript>().amp = ins.skill.Amplifier;
            while (isChanneling) { yield return new WaitForSeconds(.1f); }
            transform.root.gameObject.GetComponent<HPScript>().amp = 1;
            Debug.Log("Kilpi pois!");
        }
    }

    IEnumerator SpellCou()
    {
        if (!isChanneling) {
            isProcessing = true;
            Debug.Log("Tuba Spell");
            yield return new WaitForSeconds(ins.spell.CastTime);
            Debug.Log("Sadepilvi!");
            ins.spell.Stamp = Time.time + ins.spell.Cooldown;
            isProcessing = false;
            float dur = 0;
            while (dur < ins.spell.Cooldown) {
                Debug.Log("Drip!");
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject go in enemies)
                    if (HitCheck(go, "Spell"))
                        StartCoroutine(go.GetComponent<HPScript>().Amplify(ins.spell.Duration, ins.spell.Amplifier));
                yield return new WaitForSeconds(.5f);
                dur += .5f;
            }
            Debug.Log("Sade tyyntyi");
        }
    }
}
