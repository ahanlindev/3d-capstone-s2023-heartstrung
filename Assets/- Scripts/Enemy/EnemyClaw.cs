using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class EnemyClaw : MonoBehaviour
{
    [SerializeField] private Collider clawHitbox;

    private void Awake() {
        if (!clawHitbox) { Debug.LogError("Claws cannot find a collider component"); }
        clawHitbox.isTrigger = true;
        clawHitbox.enabled = false;
    }

    public void Claw(float duration) {
        // activate and grow hitbox
        clawHitbox.enabled = true;
        DOVirtual.DelayedCall(duration, () => clawHitbox.enabled = false);
    }

    private void OnCollisionEnter(Collision other) {
        
        var Dodge = other.gameObject.GetComponent<Heart>();
        if (Dodge) {
            BattleManager.Instance.KittyTakeDmg();
        }
    }
}
