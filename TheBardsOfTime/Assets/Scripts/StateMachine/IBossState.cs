using UnityEngine;
using System.Collections;

public interface IBossState {
    void UpdateState();

    void OnTriggerEnter(Collider other);

    void OnTriggerExit(Collider other);

    void ToChaseState();

    void ToCombatState();

    void ToCastingState();
}
