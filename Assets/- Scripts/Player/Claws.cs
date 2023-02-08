using UnityEngine;
using DG.Tweening;

// <summary>Intended to be used to represent the hitbox for Kitty's claw attack</summary>
public class Claws : MonoBehaviour
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

    private void OnTriggerEnter(Collider other) {
        //Debug.Log($"Claws collided with {other.gameObject.name}");
        var enemy = other.gameObject.GetComponent<Health>();
        if (enemy) {
            enemy.ChangeHealth(-10.0f);
        }

        var hurtingbush = other.gameObject.GetComponent<hurtingBush>();
        if (hurtingbush)
        {
            hurtingbush.bushDie();
        }
    }
}
