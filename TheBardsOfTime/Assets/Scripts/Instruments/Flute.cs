using UnityEngine;
using System.Collections;

public class Flute : MonoBehaviour {
    private CC CCref;
    private float normalSpeed;
    private Instrument ins;
    private HitDetection[] Colliders;
    private bool isProcessing = false;
    private bool isChanneling = false;

    void Start()
    {
        CCref = GameObject.Find("Player").GetComponent<CC>();
        normalSpeed = CCref.moveSetting.forwardVel;
        ins = Resources.Load("Data/FluteSO") as Instrument;
        Colliders = gameObject.GetComponentsInChildren<HitDetection>();
    }

    void Update()
    {   //Jos nappi on pohjassa ja muut hommat ei oo kesken ja tään hetkinen aika on menny stampista ohi (stamp = hetki kun skilli käytetään + cooldown)
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isProcessing && Time.time > ins.attack.Stamp) {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !isProcessing && Time.time > ins.spell.Stamp) {
            StartCoroutine(Spell());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !isProcessing && Time.time > ins.skill.Stamp) {
            StartCoroutine(Skill());
        }

        //nostaa pelaajan nopeutta 50%
        CCref.moveSetting.forwardVel = normalSpeed * 1.5f;
    }

    void OnDisable()
    {
        CCref.moveSetting.forwardVel = normalSpeed;
    }

    IEnumerator Attack()
    {
        if (isChanneling)
            isChanneling = false;
        yield return true;
    }

    IEnumerator Spell()
    {
        if (isChanneling)
            isChanneling = false;
        else {
            isChanneling = true;
            Debug.Log("Channelaus Alkaa");
            ins.spell.Stamp = Time.time + ins.spell.Cooldown;
            yield return new WaitForSeconds(ins.spell.CastTime);
            while (isChanneling) {
                yield return new WaitForSeconds(.25f);
                foreach (GameObject go in Colliders[1].enemyList) {
                    try { go.SendMessageUpwards("TakeDamage", ins.spell.Damage, SendMessageOptions.DontRequireReceiver); Debug.Log(go.name + " takes " + ins.spell.Damage + "damage"); } catch { Debug.Log("SPELL FAILS!"); }
                }
            }
            Debug.Log("Channelaus Loppuu");
        }
    }

    IEnumerator Skill()
    {
        if (isChanneling)
            isChanneling = false;
        else {
            isProcessing = true;
            Debug.Log("Roots!");
            ins.skill.Stamp = Time.time + ins.skill.Cooldown;
            yield return new WaitForSeconds(ins.skill.CastTime);
            foreach (GameObject go in Colliders[2].enemyList) {
                float[] bundle = { 5f, ins.skill.Damage };
                try {
                    go.SendMessageUpwards("StartDot", bundle, SendMessageOptions.DontRequireReceiver);
                    go.SendMessageUpwards("StartRoot", bundle[0], SendMessageOptions.DontRequireReceiver);
                    Debug.Log(go.name + " takes " + ins.skill.Damage + "damage");
                } catch { Debug.Log("SKILL FAILS!"); }
            }
            isProcessing = false;
        }
    }
}
