using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour {

    public Image hpSlider;
    public float hitpoints = 150;
    public float maxHitpoints = 150;

	// Use this for initialization
	void Start () {
        UpdateHealthbar();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void UpdateHealthbar()
    {
        float ratio = hitpoints / maxHitpoints;
        hpSlider.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    private void TakeDamage(float damage)
    {
        hitpoints -= damage;
        if (hitpoints < 0)
            hitpoints = 0;
        UpdateHealthbar();
    }
}
