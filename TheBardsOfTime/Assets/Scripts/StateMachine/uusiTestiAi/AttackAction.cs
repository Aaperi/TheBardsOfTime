using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action {

    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        RaycastHit hit;
        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attack.AttackRange, Color.red);

        if(Physics.SphereCast(controller.eyes.position, controller.enemyStats.search.SphereRadius, controller.eyes.forward, out hit, controller.enemyStats.attack.AttackRange)
           && hit.collider.CompareTag("Player"))
        {
            if (controller.CheckIfCountDownElapsed(controller.enemyStats.attack.Cooldown))
            {
                controller.hpScript.TakeDamage(controller.enemyStats.attack.Damage);
            }
        }
    }
}
