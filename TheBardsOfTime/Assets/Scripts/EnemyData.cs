using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class EnemyData : ScriptableObject {

        public float CastTime;
        public float Cooldown;
        public float AttackRange;
        public int Damage;

        public float SearchSpeed;
        public float SearchDuration;
        public float SightRange;
        public float SphereRadius;
    
}
