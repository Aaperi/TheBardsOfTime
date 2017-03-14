﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class Instrument : ScriptableObject
{

    [System.Serializable]
    public class AttackSettings
    {
        [HideInInspector]
        public float Stamp;
        public float Range;
        public float Radius;
        public float CastTime;
        public float Cooldown;
        public float Duration;
        public float Potency;
        public int Damage;
    }

    [System.Serializable]
    public class SkillSettings
    {
        [HideInInspector]
        public float Stamp;
        public float Range;
        public float Radius;
        public float CastTime;
        public float Cooldown;
        public float Duration;
        public float Potency;
        public int Damage;
        public bool Interrupt;
    }

    [System.Serializable]
    public class SpellSettings
    {
        [HideInInspector]
        public float Stamp;
        public float Range;
        public float Radius;
        public float CastTime;
        public float Cooldown;
        public float Duration;
        public float Potency;
        public int Damage;
        public bool Interrupt;
    }

    public AttackSettings attack = new AttackSettings();
    public SkillSettings skill = new SkillSettings();
    public SpellSettings spell = new SpellSettings();

    void OnEnable()
    {
        attack.Stamp = 0; spell.Stamp = 0; skill.Stamp = 0;
    }
}
