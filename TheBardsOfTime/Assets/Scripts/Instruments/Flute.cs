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
    [HideInInspector]
    public ParticleSystem psys;
    private ParticleSystem.ShapeModule sh;

    void Awake() {
        psys = GameObject.Find("Player").GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        CCref = FindObjectOfType<CC>();
        ins = Resources.Load("Data/FluteSO") as Instrument;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sh = psys.shape;
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
        yield return true;
    }

    IEnumerator SkillCou()
    {
        sh.shapeType = ParticleSystemShapeType.Circle;
        sh.arc = ins.skill.Radius;
        sh.radius = ins.skill.Range;
        psys.transform.localEulerAngles = new Vector3(90, 0, 0);

        if (isChanneling) {
            isChanneling = false;
            yield return true;
        } else {
            Debug.Log("Flute Skill");
            yield return new WaitForSeconds(ins.skill.CastTime);
            psys.Play();
            Debug.Log("Hiirulaiset!");
            ins.skill.Stamp = Time.time + ins.skill.Cooldown + ins.skill.Duration;
            float dur = 0;
            while (dur < ins.skill.Duration) {
                Debug.Log("Squeek!");
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject go in enemies)
                    if (HitCheck(go, "Skill")) {
                        go.GetComponent<HPScript>().TakeDamage(ins.skill.Damage / 2);
                        StartCoroutine(go.GetComponent<HPScript>().Slow(.5f, ins.skill.Potency));

                        if (ins.skill.Interrupt && go.GetComponent<StatePatternBoss>() != null) {
                            if (go.GetComponent<StatePatternBoss>().weakness.name == ins.name) {
                                go.GetComponent<StatePatternBoss>().castingState.Interrupt();
                            }
                        }
                    }
                yield return new WaitForSeconds(.5f);
                dur += .5f;
            }
            psys.Stop();
            Debug.Log("Hei hei hiiret");
        }
    }

    IEnumerator SpellCou()
    {
        sh.shapeType = ParticleSystemShapeType.Circle;
        sh.arc = 0;
        sh.radius = ins.spell.Range;
        psys.transform.localEulerAngles = new Vector3(0, -90, 0);

        if (isChanneling) {
            isChanneling = false;
            yield return true;
        } else {
            isChanneling = true;
            Debug.Log("Flute Spell");
            ins.spell.Stamp = Time.time + ins.spell.Cooldown;
            yield return new WaitForSeconds(ins.spell.CastTime);
            psys.Play();
            Debug.Log("Huilutus Alkaa");
            while (isChanneling) {
                yield return new WaitForSeconds(.5f);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                List<GameObject> temp = new List<GameObject>();
                foreach (GameObject go in enemies)
                    if (HitCheck(go, "Spell")) {
                        temp.Add(go);

                        if (ins.spell.Interrupt && go.GetComponent<StatePatternBoss>() != null) {
                            if (go.GetComponent<StatePatternBoss>().weakness.name == ins.name) {
                                go.GetComponent<StatePatternBoss>().castingState.Interrupt();
                            }
                        }
                    }

                temp.Sort(delegate (GameObject a, GameObject b) {
                    float distA = Vector3.Distance(a.transform.position, player.transform.position);
                    float distB = Vector3.Distance(b.transform.position, player.transform.position);
                    return distA.CompareTo(distB);
                });
                if (temp.Count > 0)
                    temp[0].GetComponent<HPScript>().TakeDamage(ins.spell.Damage / 2);
            }
            psys.Stop();
            Debug.Log("Huilutus Loppuu");
        }
    }

}
