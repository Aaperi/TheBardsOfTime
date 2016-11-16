using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour {

    public Image hpSlider;
    private float hitpoints = 150;
    public float maxHitpoints = 150;
	public bool iNeedUI;

	// Use this for initialization
	void Start () {
        UpdateHealthbar();
	}
	
	// Update is called once per frame
	void Update () {
		if (hitpoints < 0) {
			hitpoints = 0;
			Destroy (gameObject);
		}
	}

    private void UpdateHealthbar()
    {
		if (iNeedUI) {
			float ratio = hitpoints / maxHitpoints;
			hpSlider.rectTransform.localScale = new Vector3 (ratio, 1, 1);
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
		
	private void StartDot(float[] dot){
		StartCoroutine(DamageOverTime(dot[0], dot[1], dot[2]));
	}

	IEnumerator DamageOverTime(float damageDuration, float damageCount, float damageAmount)
	{
		int currentCount = 0;
		while(currentCount < damageCount)
		{
			hitpoints -= damageAmount;
			UpdateHealthbar ();
			yield return new WaitForSeconds(damageDuration);
			currentCount++;
		}
	}
}
