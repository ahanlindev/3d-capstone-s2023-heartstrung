using UnityEngine;
using DG.Tweening;

/// <summary>
/// Component that, when first enabled, causes the target object 
/// to disappear after a short animation
/// </summary>
public class Disappearer : MonoBehaviour {
    [SerializeField] private GameObject _target;

    void Awake() {
        // if target is absent or inactive, ignore
        if (!_target) { 
            Debug.LogError("Dissapearer not given a target"); 
            return; 
        } else if (!_target.activeSelf) { 
            return; 
        }

        // make target disappear
        DisappearTween(_target);
    }

    private void DisappearTween(GameObject target) {
        Transform targetTf = target.transform;

        DOTween.Sequence()
        // rumble on x and z axes
        .Append(targetTf.DOShakePosition(duration: 2.0f, strength: new Vector3(.2f,0,.2f), vibrato: 20))
        // shrink to nothing
        .Append(targetTf.DOScale(0, 0.5f).SetEase(Ease.InExpo))
        .OnComplete(() => target.SetActive(false));
    }
}