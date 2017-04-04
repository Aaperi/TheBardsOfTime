using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class EnemyData : ScriptableObject {

    [System.Serializable]
    public class AttackSettings {
        public float CastTime;
        public float Cooldown;
        public float AttackRange;
        public int Damage;
    }

    [System.Serializable]
    public class SearchSettings {
        public float SearchSpeed;
        public float SearchDuration;
        public float SightRange;
    }

    public AttackSettings attack = new AttackSettings();
    public SearchSettings search = new SearchSettings();
}
