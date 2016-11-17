using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour {

    private float attackCD = 1f;
    private float attackTimer = 0;
    private EnemyBehaviour behaviour;

	// Use this for initialization
	void Start () {
        behaviour = FindObjectOfType<EnemyBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(behaviour.dist < 2 && behaviour.playerInSight)
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= attackCD)
            {
                Attack();
                attackTimer = 0;
            }
        }
	}

    void Attack()
    {
        Debug.Log("Lyö");
    }

}
