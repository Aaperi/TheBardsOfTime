using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HPScript : MonoBehaviour {

    public Slider HPSlider;
    public float hitpoints;
    public float maxHitpoints;
	public bool iNeedUI;

	void Start () {
        hitpoints = maxHitpoints;
        UpdateHealthbar();
	}
	
	void Update () {
		if (hitpoints <= 0) {
			hitpoints = 0;
            HitDetection[] temp = FindObjectsOfType<HitDetection>();
            foreach(HitDetection HD in temp)
                    HD.enemyList.Remove(gameObject);
            gameObject.SetActive(false);
        }
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

	private void StartRoot(float duration){
		StartCoroutine (Root (duration));
	}

	IEnumerator Root(float duration){
		GetComponent<Rigidbody> ().isKinematic = true;
		yield return new WaitForSeconds (duration);
		GetComponent<Rigidbody> ().isKinematic = false;
	}
		
	private void StartDot(float[] bundle){
		StartCoroutine(DamageOverTime(bundle[0], bundle[1]));
	}

	IEnumerator DamageOverTime(float tics, float damagePerTic)
	{
		int currentCount = 1;
        yield return new WaitForSeconds(.5f);
		while(currentCount < tics)
		{
            TakeDamage(damagePerTic);
            yield return new WaitForSeconds(1);
            currentCount++;
		}
        TakeDamage(damagePerTic);
        yield return new WaitForSeconds(.5f);
    }
}
