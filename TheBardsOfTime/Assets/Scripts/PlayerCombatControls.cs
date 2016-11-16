using UnityEngine;
using System.Collections;

public class PlayerCombatControls : MonoBehaviour {

	public Collider[] attackHitBoxes;

	private string Instrument = "violin";
	private float ViolinCD = .75f;
	private float ViolinSkillCD = 10; //duration 3sec

	void Start () {
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.F))
			Attack(attackHitBoxes[0]);
	}

	void Attack(Collider col)
	{
		Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
		foreach (Collider c in cols)
		{
			if (c.transform.parent == transform)
				continue;
			c.SendMessageUpwards ("TakeDamage", 60);
		}
	}

	void Skill(){
	}

	void Spell(){
	}

	void ViolinAttack(){
	}

	void ViolinSkill(){
	}

	void ViolinSpell(){
	}
}
