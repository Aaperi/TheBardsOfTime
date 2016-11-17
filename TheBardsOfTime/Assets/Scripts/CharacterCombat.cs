using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterCombat : MonoBehaviour {

	private string[,] Instruments = { {"Violin", "Cello"}, {"Flute", ""}, {"Tuba", "Trombone"}, {"MarchDrum", ""} };
    private List<GameObject> InAttackRange = new List<GameObject>();
    private List<GameObject> InSpellRange = new List<GameObject>();
    private List<GameObject> InSkillRange = new List<GameObject>();
    private Collider[] activeColliders;
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

	void Start () {
        activeInstrument = "Violin";
        activeColliders = GameObject.Find(Instruments[0,0]).GetComponentsInChildren<MeshCollider>();
	}

	void Update () {
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
    
    void AttackRangeUpdate (List<GameObject> temp){InAttackRange = temp;}
    void SpellRangeUpdate(List<GameObject> temp){InSpellRange = temp;}
    void SkillRangeUpdate(List<GameObject> temp){InSkillRange = temp;}

    void iDied(GameObject temp)
    {
        InAttackRange.Remove(temp);
        InSpellRange.Remove(temp);
        InSkillRange.Remove(temp);
    }

    IEnumerator Attack(string instrument)
    {
        isProcessing = true;
        switch (instrument)
        {
            case "Violin": {
                    yield return new WaitForSeconds(atkCastTime);
                    foreach (GameObject go in InAttackRange)
                        if(InAttackRange.Contains(go))
                            go.SendMessageUpwards("TakeDamage", atkDamage);
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
                        foreach (GameObject go in InSpellRange)
                            if (InSpellRange.Contains(go)){
                                Debug.Log("vinkuvonku");
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
                    yield return new WaitForSeconds(skillCastTime);
                    foreach (GameObject go in InSkillRange)
                        if (InSkillRange.Contains(go)) {
                            float[] bundle = { 5f, 10 };
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
