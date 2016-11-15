using UnityEngine;
using System.Collections;

public class PlayerCombatControls : MonoBehaviour {

	public Collider[] attackHitBoxes;
	private string Instrument = "violin";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F))
			Attack(attackHitBoxes[0]);
		//Shoot(Instrument);	
	}

	void Attack(Collider col)
	{
		Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
		foreach (Collider c in cols)
		{
			if (c.transform.parent == transform)
				continue;
			float[] dot = {.1f, 50, 2};
			c.SendMessageUpwards("StartDot", dot);
			c.SendMessageUpwards("StartRoot", 4);
		}
	}

	void Shoot(string instrument)
	{
		switch (instrument)
		{
		case "violin":
			{
				GameObject temp = Instantiate(Resources.Load("Prefabs/projectile"), GameObject.Find("Shootpoint").transform.position, Quaternion.identity) as GameObject;
				temp.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
				break;
			}
		default: break;
		}
	}
}
