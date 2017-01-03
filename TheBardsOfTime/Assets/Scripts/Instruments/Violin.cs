using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class Violin : Instrument  {

    public override IEnumerator Attack(List<GameObject>EnemyList)
    {
        Debug.Log("Normal attack");
        CCref.isProcessing = true;
        attack.Stamp = Time.time + attack.Cooldown;
        yield return new WaitForSeconds(attack.CastTime);
        foreach (GameObject go in EnemyList)
        {
            try { go.SendMessageUpwards("TakeDamage", attack.Damage, SendMessageOptions.DontRequireReceiver); Debug.Log(go.name + " takes " + attack.Damage + "damage"); }
            catch { Debug.Log("ATTACK FAILS!"); }
        }
        CCref.isProcessing = false;
    }

    public override IEnumerator Spell(List<GameObject> EnemyList)
    {
        Debug.Log(attack.Damage);
        yield return new WaitForSeconds(2f);
        Debug.Log(attack.Cooldown);
    }

    public override IEnumerator Skill(List<GameObject> EnemyList)
    {
        Debug.Log(attack.Damage);
        yield return new WaitForSeconds(2f);
        Debug.Log(attack.Cooldown);
    }
}
