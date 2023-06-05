using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectibleBots : MonoBehaviour {

    public int convertedMoneyAmount = 1;

    public bool isMoney;
    public void FallFromPlayer() {
        transform.DOLocalJump(transform.localPosition + (Vector3.up/2), .8f, 1, 1);
        transform.DOScale(Vector3.zero, 1f);
        Destroy(gameObject, 3f);
    }

    public void Recoil(float recoilDampen,float durationHalfway) {
        var localpos = transform.localPosition;
        transform.DOLocalMove(localpos + (Vector3.back / recoilDampen), durationHalfway).OnComplete(() => {
            transform.DOLocalMove(localpos, durationHalfway);
        });
    }
}
