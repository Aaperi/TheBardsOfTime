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
        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.AttackRange, Color.red);

        if(Physics.SphereCast(controller.eyes.position, controller.enemyStats.SphereRadius, controller.eyes.forward, out hit, controller.enemyStats.AttackRange)
           && hit.collider.CompareTag("Player"))
        {
            if (controller.CheckIfCountDownElapsed(controller.enemyStats.Cooldown))
            {
                controller.hpScript.TakeDamage(controller.enemyStats.Damage);
            }
        }
    }
}
