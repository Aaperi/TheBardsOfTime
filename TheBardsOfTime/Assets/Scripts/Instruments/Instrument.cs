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
        public float Stamp;
    }

    [System.Serializable]
    public class SpellSettings {
        public float CastTime;
        public float Cooldown;
        public float Damage;
        public float Stamp;
    }

    [System.Serializable]
    public class SkillSettings {
        public float CastTime;
        public float Cooldown;
        public float Damage;
        public float Stamp;
    }

    public AttackSettings attack = new AttackSettings();
    public SpellSettings spell = new SpellSettings();
    public SkillSettings skill = new SkillSettings();

    void OnEnable()
    {
        attack.Stamp = 0; spell.Stamp = 0; skill.Stamp = 0;
    }
}
