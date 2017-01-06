using UnityEngine;
using System.Collections;

public class Violin : MonoBehaviour
{
    private Instrument ins;
    private HitDetection[] Colliders;
    private bool isProcessing = false;
    private bool isChanneling = false;

    void Start()
    {
        ins = Resources.Load("Data/ViolinSO") as Instrument;
        Colliders = gameObject.GetComponentsInChildren<HitDetection>();
    }

    void Update()
    {   //Jos nappi on pohjassa ja muut hommat ei oo kesken ja tään hetkinen aika on menny stampista ohi (stamp = hetki kun skilli käytetään + cooldown)
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isProcessing && Time.time > ins.attack.Stamp)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !isProcessing && Time.time > ins.spell.Stamp)
        {
            StartCoroutine(Spell());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !isProcessing && Time.time > ins.skill.Stamp)
        {
            StartCoroutine(Skill());
        }
    }

    IEnumerator Attack()
    {
        if (isChanneling)
            isChanneling = false;
        else {
            isProcessing = true;
            Debug.Log("Normal Attack!");
            ins.attack.Stamp = Time.time + ins.attack.Cooldown;
            yield return new WaitForSeconds(ins.attack.CastTime);
            foreach (GameObject go in Colliders[0].enemyList)
            {
                try { go.SendMessageUpwards("TakeDamage", ins.attack.Damage, SendMessageOptions.DontRequireReceiver); Debug.Log(go.name + " takes " + ins.attack.Damage + "damage"); }
                catch { Debug.Log("ATTACK FAILS!"); }
            }
            isProcessing = false;
        }
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
            while (isChanneling)
            {
                yield return new WaitForSeconds(.25f);
                foreach (GameObject go in Colliders[1].enemyList)
                {
                    try { go.SendMessageUpwards("TakeDamage", ins.spell.Damage, SendMessageOptions.DontRequireReceiver); Debug.Log(go.name + " takes " + ins.spell.Damage + "damage"); }
                    catch { Debug.Log("SPELL FAILS!"); }
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
            foreach (GameObject go in Colliders[2].enemyList)
            {
                float[] bundle = { 5f, ins.skill.Damage };
                try
                {
                    go.SendMessageUpwards("StartDot", bundle, SendMessageOptions.DontRequireReceiver);
                    go.SendMessageUpwards("StartRoot", bundle[0], SendMessageOptions.DontRequireReceiver);
                    Debug.Log(go.name + " takes " + ins.skill.Damage + "damage");
                }
                catch { Debug.Log("SKILL FAILS!"); }
            }
            isProcessing = false;
        }
    }
}
