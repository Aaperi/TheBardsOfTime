using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Find")]
public class FindDecision : Decision {

    public override bool Decide(StateController controller)
    {
        bool noEnemyInSight = Find(controller);
        return noEnemyInSight;
    }

    private bool Find(StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        controller.transform.Rotate(0, controller.enemyStats.SearchSpeed * Time.deltaTime, 0);
        return controller.CheckIfCountDownElapsed(controller.enemyStats.SearchDuration);
    }
}
