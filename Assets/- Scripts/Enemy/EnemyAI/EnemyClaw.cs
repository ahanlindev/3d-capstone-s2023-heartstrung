using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class EnemyClaw : MonoBehaviour
{
    [SerializeField] private Collider clawHitbox;

    private float _damage = 10.0f;

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
    private void OnTriggerEnter(Collider other) {
        //Debug.Log($"Enemy claws collided with {other.gameObject.name}");
        var player = other.gameObject.GetComponent<Health>();
        if (player) {
            player.ChangeHealth(-_damage);
        }
    }
}
