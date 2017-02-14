using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HPScript : MonoBehaviour
{
    [HideInInspector]
    public bool rooted = false;
    public Slider HPSlider;
    public int hitpoints;
    public int maxHitpoints;
    public int regenAmount;
    public float regenSpeed = 2;
    public bool iNeedUI;
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
        hitpoints -= damage;
        vilkkuminen = true;
        if (iNeedUI) {
            UpdateHealthbar();
        }
    }

    public IEnumerator Root(float duration)
    {
        rooted = true;
        GetComponent<NavMeshAgent>().Stop();
        yield return new WaitForSeconds(duration);
        rooted = false;
    }

    public IEnumerator DOT(int duration, int damage)
    {
        int currentCount = 1;
        yield return new WaitForSeconds(.5f);
        while (currentCount < duration) {
            TakeDamage(damage / duration);
            yield return new WaitForSeconds(1);
            currentCount++;
        }
        TakeDamage(damage / duration);
        yield return new WaitForSeconds(.5f);
    }
}
