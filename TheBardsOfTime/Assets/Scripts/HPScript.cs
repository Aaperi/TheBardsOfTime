using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPScript : MonoBehaviour {

    public Image HPSlider;
    private float hitpoints;
    public float maxHitpoints;
	public bool iNeedUI;

	// Use this for initialization
	void Start () {
        hitpoints = maxHitpoints;
        UpdateHealthbar();
	}
	
	// Update is called once per frame
	void Update () {
		if (hitpoints < 0) {
			hitpoints = 0;
			Destroy (gameObject);
            GameObject.Find("Player").SendMessage("iDied", gameObject);
		}
	}

    private void UpdateHealthbar()
    {
		if (iNeedUI) {
			float ratio = hitpoints / maxHitpoints;
            HPSlider.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		}
    }

    private void TakeDamage(float damage)
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
