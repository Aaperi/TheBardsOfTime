using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HPScript : MonoBehaviour
{

    public Slider HPSlider;
    public float hitpoints;
    public float maxHitpoints;
    public bool iNeedUI;
    public Transform trans;
    private bool isRooted = false;
    MenuScript menu;

    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
        hitpoints = maxHitpoints;
        UpdateHealthbar();
        menu = FindObjectOfType<MenuScript>();
    }

    void Update()
    {
        if (gameObject.transform.position.y < -50 || hitpoints <= 0) {
            Death();
            if (iNeedUI) {
                menu.ShowGameOver();
            }
        }
        if (isRooted) {
            gameObject.GetComponent<Transform>().position = trans.position;
        }
        gameObject.GetComponent<Transform>().position = trans.position;

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
        if (iNeedUI) {
            HPSlider.value = hitpoints;
        }
    }

    public void TakeDamage(float damage)
    {
        hitpoints -= damage;
        UpdateHealthbar();
    }

    private void StartRoot(float duration)
    {
        StartCoroutine(Root(duration));
    }

    IEnumerator Root(float duration)
    {
        trans = gameObject.GetComponent<Transform>();
        isRooted = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(duration);
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        isRooted = false;
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
