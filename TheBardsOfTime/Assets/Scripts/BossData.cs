using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class BossData : ScriptableObject {

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

    public AttackSettings attack = new AttackSettings();
    public SpellSettings spell = new SpellSettings();

    void OnEnable() {
        attack.Stamp = 0; spell.Stamp = 0;
    }
}
