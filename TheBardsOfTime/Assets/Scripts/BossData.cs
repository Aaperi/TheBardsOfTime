using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class BossData : ScriptableObject {

    [System.Serializable]
    public class AttackSettings {
        public float CastTime;
        public float Cooldown;
        public int Damage;
    }

    [System.Serializable]
    public class SpellSettings {
        public float castingRange;
        public float castingRadius;
        public float timeToCasting;
        public float CastTime;
        public int Damage;
    }

    public AttackSettings attack = new AttackSettings();
    public SpellSettings spell = new SpellSettings();
}
