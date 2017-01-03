using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class Instrument : ScriptableObject {

    [System.Serializable]
    public class AttackSettings {
        public float CastTime;
        public float Cooldown;
        public float Damage;
        public float Stamp = 0;
    }

    [System.Serializable]
    public class SpellSettings {
        public float CastTime;
        public float Cooldown;
        public float Damage;
        public float Stamp = 0;
    }

    [System.Serializable]
    public class SkillSettings {
        public float CastTime;
        public float Cooldown;
        public float Damage;
        public float Stamp = 0;
    }

    public AttackSettings attack = new AttackSettings();
    public SpellSettings spell = new SpellSettings();
    public SkillSettings skill = new SkillSettings();
    public CharacterCombat CCref;

    void OnEnable()
    {
        attack.Stamp = 0; spell.Stamp = 0; skill.Stamp = 0;
        CCref = GameObject.Find("Player").GetComponent<CharacterCombat>();
    }

    public virtual IEnumerator Attack(List<GameObject> EnemyList) {
        yield return null;
    }

    public virtual IEnumerator Spell(List<GameObject> EnemyList) {
        yield return null;
    }

    public virtual IEnumerator Skill(List<GameObject> EnemyList) {
        yield return null;
    }
}
