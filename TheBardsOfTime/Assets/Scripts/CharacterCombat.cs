using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterCombat : MonoBehaviour {

    //private string[,] Instruments = { {"Violin", "Cello"}, {"Flute", ""}, {"Tuba", "Trombone"}, {"MarchDrum", ""} };
    public HitDetection[] activeColliders;
    public bool isProcessing = true;
    public bool isChanneling = false;
    public string activeInstrument;

    public Instrument equippedWeapon;

	void Start () {
        activeInstrument = "ViolinSO";
        equippedWeapon = Resources.Load("Data/" + activeInstrument) as Instrument;
        activeColliders = GameObject.Find(activeInstrument).GetComponentsInChildren<HitDetection>();
	}

	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isProcessing /*&& !isChanneling && equippedWeapon.attack.Stamp < Time.time*/)
        {
            Debug.Log(equippedWeapon.attack.Stamp + " < " + Time.time);
            /*StartCoroutine(equippedWeapon.Attack(activeColliders[0].enemyList));
            Debug.Log(isProcessing);*/
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !isProcessing && !isChanneling && equippedWeapon.spell.Stamp < Time.time)
        {
        }

        /*
        if (Input.GetKeyDown(KeyCode.Alpha3) && !isProcessing)
        {
            StartCoroutine(Skill(activeInstrument));
        }*/
    }

    /*
    IEnumerator Attack(string instrument)
    {
        isProcessing = true;
        switch (instrument)
        {
            case "Violin": {
                    Debug.Log("Normal attack");
                    yield return new WaitForSeconds(atkCastTime);
                    foreach (GameObject go in getList("AttackRange")){
                        try { go.SendMessageUpwards("TakeDamage", atkDamage, SendMessageOptions.DontRequireReceiver);  Debug.Log(go.name+" takes "+atkDamage+"damage"); }
                        catch { Debug.Log("ATTACK FAILS!"); }
                    }
                    atkStamp = Time.time + atkCooldown;
                    break;
                }
            default: break;
        }
        isProcessing = false;
	}

    IEnumerator Spell(string instrument)
    {
        isProcessing = true;
        switch (instrument)
        {
            case "Violin":
                {
                    Debug.Log("vingutus alkaa!");
                    channeling = true;
                    yield return new WaitForSeconds(spellCastTime);
                    while (channeling)
                    {
                        yield return new WaitForSeconds(1f);
                        Debug.Log("vinkuvonku");
                        foreach (GameObject go in getList("SpellRange")){
                            go.SendMessageUpwards("TakeDamage", spellDamage);
                        }
                    }
                    spellStamp = Time.time + spellCooldown;
                    Debug.Log("vingutus loppuu!");
                    break;
                }
            default: Debug.Log("something went wrong"); break;
        }
        isProcessing = false;
    }

    IEnumerator Skill(string instrument)
    {
        isProcessing = true;
        switch (instrument)
        {
            case "Violin":
                {
                    Debug.Log("Roots!");
                    yield return new WaitForSeconds(skillCastTime);
                    foreach (GameObject go in getList("SkillRange")){
                            float[] bundle = { 5f, skillDamage };
                            go.SendMessageUpwards("StartDot", bundle);
                            go.SendMessageUpwards("StartRoot", bundle[0]);
                        }
                    skillStamp = Time.time + skillCooldown;
                    break;
                }
            default: break;
        }
        isProcessing = false;
    }*/
}
