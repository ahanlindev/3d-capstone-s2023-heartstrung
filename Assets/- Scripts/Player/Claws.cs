using UnityEngine;
using DG.Tweening;

// <summary>Intended to be used to represent the hitbox for Kitty's claw attack</summary>
public class Claws : MonoBehaviour
{
    private const float DAMAGE = 10; // TODO make this inspector-visible?

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
        var health = other.gameObject.GetComponent<Health>();

        // no friendly fire
        if (health && !health.CompareTag("Heart")) {
            health.ChangeHealth(-DAMAGE);
        }
    }
}
