using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterCombat : MonoBehaviour {

	//private string[,] Instruments = { {"Violin", "Cello"}, {"Flute", ""}, {"Tuba", "Trombone"}, {"MarchDrum", ""} };
    private HitDetection[] activeColliders;
    private bool isProcessing = false;
    private bool channeling = false;
    public string activeInstrument;

    private float atkCastTime = .5f;
    private float atkCooldown = .25f;
    private float atkDamage = 60;
    private float atkStamp;

    private float spellCastTime = 1;
    private float spellCooldown = 0;
    private float spellDamage = 30;
    private float spellStamp;

    private float skillCastTime = 0;
    private float skillCooldown = 10;
    private float skillDamage = 10;
    private float skillStamp;

    public Violin vil = new Violin();

	void Start () {
        activeInstrument = "Violin";
        activeColliders = GameObject.Find(activeInstrument).GetComponentsInChildren<HitDetection>();
	}

	void Update () {
        //ehto: Nappi pohjassa + ei ole cooldownissa + muut hyökkäykset eivät ole kesken
        if (Input.GetKeyDown(KeyCode.Alpha1) && atkStamp < Time.time && !isProcessing)
        {
            StartCoroutine(Attack(activeInstrument));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!channeling)
            {
                if (spellStamp < Time.time && !isProcessing)
                {
                    StartCoroutine(Spell(activeInstrument));
                }
            }
            else
                channeling = false;
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && skillStamp < Time.time && !isProcessing)
        {
            StartCoroutine(Skill(activeInstrument));
        }
    }

    //Käy hakemassa listan vihollisista joita löytyy kysytyltä rangella
    List<GameObject> getList(string ListName)
    {
        switch (ListName)
        {
            case "AttackRange": {
                    return activeColliders[0].enemyList;
                }

            case "SpellRange": {
                    return activeColliders[1].enemyList;
                }

            case "SkillRange": {
                    return activeColliders[2].enemyList;
                }

            default: return new List<GameObject>();
        }
    }

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
    }

    void ViolinAttack(){
	}

	void ViolinSkill(){
	}

	void ViolinSpell(){
	}
}
