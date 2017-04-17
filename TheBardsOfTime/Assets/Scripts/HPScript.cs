using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HPScript : MonoBehaviour
{
    [HideInInspector]
    public bool iNeedUI;
    [HideInInspector]
    public int hitpoints;
    public Slider HPSlider;
    public int maxHitpoints;
    public int regenAmount;
    public float regenSpeed = 2;
    public float amp = 1;
    MenuScript menu;
    GameManager gm;
    CC player;
    MeshRenderer mesh;
	UIActions uia;

    bool statusEffect = false;
    bool vilkkuminen = false;
    float vilkkumisAika;

    void Start()
    {
        hitpoints = maxHitpoints;
        menu = FindObjectOfType<MenuScript>();
        player = FindObjectOfType<CC>();
        mesh = GetComponent<MeshRenderer>();
        gm = FindObjectOfType<GameManager>();
		uia = FindObjectOfType<UIActions> ();

        vilkkumisAika = .75f;

        if (iNeedUI && gm.playerHp > 0) {
            hitpoints = gm.playerHp;
            UpdateHealthbar();
        }

        UpdateHealthbar();
    }

    void Update()
    {
        if (gameObject.transform.position.y < -50 || hitpoints <= 0) {
            Death();
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
        if (gameObject.tag == "Enemy") {
            if (statusEffect)
                mesh.material.color = Color.grey;
            else
                mesh.material.color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            TakeDamage(10);
        }
    }

    void Death()
    {
        Debug.Log(gameObject.name + " has died. lol!");

        hitpoints = 0;
		if (iNeedUI) {
			uia.GameOver();
		}

        gameObject.SetActive(false);

        player.inCombat = false;
    }

    private void UpdateHealthbar()
    {
        if (iNeedUI) {
            HPSlider.value = hitpoints;
            gm.playerHp = hitpoints;
        }
    }

    private void RegenHP(int regenAmount)
    {
        hitpoints += regenAmount;
        UpdateHealthbar();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took: " + (int)(damage * amp) + "damage");
        hitpoints -= (int)(damage * amp);
        vilkkuminen = true;
        UpdateHealthbar();
    }

    public IEnumerator Slow(float duration, float percentage)
    {
        statusEffect = true;
        if (GetComponent<StatePatternEnemy>()) {
            Debug.Log(name + " slowed down by " + percentage + "%");
            GetComponent<NavMeshAgent>().speed *= (100 - percentage) / 100;
            yield return new WaitForSeconds(duration);
            GetComponent<NavMeshAgent>().speed /= (100 - percentage) / 100;
        } else if (tag.Contains("Player")) {
            Debug.Log("You are being slowed by " + percentage + "%");
            GetComponent<CC>().moveSetting.forwardVel *= (100 - percentage) / 100;
            yield return new WaitForSeconds(duration);
            GetComponent<CC>().moveSetting.forwardVel /= (100 - percentage) / 100;
        }
        statusEffect = false;
    }

    public IEnumerator Root(float duration)
    {
        statusEffect = true;
        if (GetComponent<StatePatternEnemy>()) {
            Debug.Log(name + " became rooted in place");
            float temp = GetComponent<NavMeshAgent>().speed;
            GetComponent<NavMeshAgent>().speed = 0f;
            yield return new WaitForSeconds(duration);
            GetComponent<NavMeshAgent>().speed = temp;
        } else if (tag.Contains("Player")) {
            Debug.Log("You became rooted in place for " + duration + "s");
            float temp = GetComponent<CC>().moveSetting.forwardVel;
            GetComponent<CC>().moveSetting.forwardVel = 0f;
            yield return new WaitForSeconds(duration);
            GetComponent<CC>().moveSetting.forwardVel = temp;
        }
        statusEffect = false;
    }

    public IEnumerator Stun(float duration)
    {
        statusEffect = true;
        if (GetComponent<StatePatternEnemy>()) {
            Debug.Log(name + " got stunned");
            StatePatternEnemy temp = GetComponent<StatePatternEnemy>();
            temp.enabled = false;
            yield return new WaitForSeconds(duration);
            temp.enabled = true;
        } else if (tag.Contains("Player")) {
            Debug.Log("You got stunned");
            CC temp = GetComponent<CC>();
            temp.enabled = false;
            yield return new WaitForSeconds(duration);
            temp.enabled = true;
        }
        statusEffect = false;
    }

    public IEnumerator Amplify(float duration, float amplifier)
    {
        statusEffect = true;
        amp = amplifier;
        Debug.Log("Uaargh, " + gameObject.name + " takes " + amplifier + "x damage");
        yield return new WaitForSeconds(duration);
        amp = 1;
        statusEffect = false;
    }

    public IEnumerator DOT(float duration, int damage)
    {
        statusEffect = true;
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
        statusEffect = false;
    }
}
