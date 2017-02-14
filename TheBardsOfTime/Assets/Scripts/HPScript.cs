using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HPScript : MonoBehaviour
{
    [HideInInspector]
    public bool rooted = false;
    [HideInInspector]
    public bool iNeedUI;
    [HideInInspector]
    public int hitpoints;
    public Slider HPSlider;
    public int maxHitpoints;
    public int regenAmount;
    public float regenSpeed = 2;
    MenuScript menu;
    CC player;
    MeshRenderer mesh;

    bool vilkkuminen = false;
    float vilkkumisAika;

    void Start()
    {
        hitpoints = maxHitpoints;
        menu = FindObjectOfType<MenuScript>();
        player = FindObjectOfType<CC>();
        mesh = GetComponent<MeshRenderer>();
        UpdateHealthbar();

        vilkkumisAika = .75f;
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
            if (regenSpeed <= 0) {
                RegenHP(regenAmount);
                regenSpeed = 2;
            }
        }

        if (vilkkuminen) {
            vilkkumisAika -= Time.deltaTime;
            mesh.material.color = Color.white;
            if (vilkkumisAika <= 0) {
                if (iNeedUI) {
                    mesh.material.color = Color.blue;
                } else
                    mesh.material.color = Color.red;

                vilkkuminen = false;
                vilkkumisAika = .25f;
            }
        }
    }

    void Death()
    {
        Debug.Log(gameObject.name + " has died. lol!");

        hitpoints = 0;
        HitDetection[] temp = FindObjectsOfType<HitDetection>();
        foreach (HitDetection HD in temp)
            HD.enemyList.Remove(gameObject);
        TargetManager tamp = FindObjectOfType<TargetManager>();
        tamp.RemoveTarget(gameObject);
        gameObject.SetActive(false);

        player.inCombat = false;
    }

    private void UpdateHealthbar()
    {
        if (iNeedUI)
            HPSlider.value = hitpoints;
    }

    private void RegenHP(int regenAmount)
    {
        hitpoints += regenAmount;
        UpdateHealthbar();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took: " + damage + "damage");
        hitpoints -= damage;
        vilkkuminen = true;
        if (iNeedUI) {
            UpdateHealthbar();
        }
    }

    public IEnumerator Slow(float duration, float percentage)
    {
        Debug.Log(gameObject.name + " slowed down by " + percentage + "%");
        GetComponent<NavMeshAgent>().speed *= (100 - percentage) / 100;
        yield return new WaitForSeconds(duration);
        GetComponent<NavMeshAgent>().speed /= (100 - percentage) / 100;
    }

    public IEnumerator Root(float duration)
    {
        Debug.Log(gameObject.name + " became rooted in place");
        float temp = GetComponent<NavMeshAgent>().speed;
        GetComponent<NavMeshAgent>().speed = 0f;
        yield return new WaitForSeconds(duration);
        GetComponent<NavMeshAgent>().speed = temp;
    }

    public IEnumerator DOT(float duration, int damage)
    {
        Debug.Log(gameObject.name + " started burning");
        int currentCount = 1;
        int ticDamage = damage / (int)duration;
        yield return new WaitForSeconds(.5f);
        while (currentCount < duration) {
            currentCount++;
            TakeDamage(ticDamage);
            yield return new WaitForSeconds(1);
        }
        TakeDamage(ticDamage);
        yield return new WaitForSeconds(.5f);
    }
}
