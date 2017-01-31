﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HPScript : MonoBehaviour
{
    [HideInInspector]
    public bool rooted = false;
    public Slider HPSlider;
    public float hitpoints;
    public float maxHitpoints;
    public float regenAmount;
    public float regenSpeed = 2;
    public bool iNeedUI;
    MenuScript menu;
    CC player;

    void Start()
    {
        hitpoints = maxHitpoints;
        menu = FindObjectOfType<MenuScript>();
        player = FindObjectOfType<CC>();

        UpdateHealthbar();
    }

    void Update()
    {
        if (gameObject.transform.position.y < -50 || hitpoints <= 0) {
            Death();
            if (iNeedUI) {
                menu.ShowGameOver();
            }
        }

        if (iNeedUI && !player.inCombat && hitpoints < 100) {
            regenSpeed -= Time.deltaTime;
            if(regenSpeed <= 0) {
                RegenHP(regenAmount);
                regenSpeed = 2;
            }
        }
    }

    void Death()
    {
        hitpoints = 0;
        HitDetection[] temp = FindObjectsOfType<HitDetection>();
        foreach (HitDetection HD in temp)
            HD.enemyList.Remove(gameObject);
        TargetManager tamp = FindObjectOfType<TargetManager>();
        tamp.RemoveTarget(gameObject);
        gameObject.SetActive(false);
    }

    private void UpdateHealthbar()
    {
        if(iNeedUI)
        HPSlider.value = hitpoints;

    }

    public void TakeDamage(float damage)
    {
        hitpoints -= damage;
        if (iNeedUI) {
            UpdateHealthbar();
        }

    }

    private void RegenHP(float regenAmount) {
        hitpoints += regenAmount;
        UpdateHealthbar();
    }

    private void StartRoot(float duration)
    {
        StartCoroutine(Root(duration));
    }

    IEnumerator Root(float duration)
    {
        rooted = true;
        GetComponent<NavMeshAgent>().Stop();
        yield return new WaitForSeconds(duration);
        rooted = false;
    }

    private void StartDot(float[] bundle)
    {
        StartCoroutine(DamageOverTime(bundle[0], bundle[1]));
    }

    IEnumerator DamageOverTime(float tics, float damagePerTic)
    {
        int currentCount = 1;
        yield return new WaitForSeconds(.5f);
        while (currentCount < tics) {
            TakeDamage(damagePerTic);
            yield return new WaitForSeconds(1);
            currentCount++;
        }
        TakeDamage(damagePerTic);
        yield return new WaitForSeconds(.5f);
    }
}
