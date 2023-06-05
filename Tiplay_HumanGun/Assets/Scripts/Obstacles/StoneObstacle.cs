using DG.Tweening;
using TMPro;
using UnityEngine;
public class StoneObstacle : MonoBehaviour,IShootable {

    private int _endHoldedMoney = 2;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private int value;
    [SerializeField] private int howManyRemoveFromPlayer = 3;
    [SerializeField] private Transform holdObject;
    [SerializeField] private bool isEndGameObstacle;

    private void Start() {
        UpdateText();
    }

    public void Hit(int damage) {
        value -= damage;
        if (value <= 0) {
            if (holdObject != null) {
                FlyTowards();
            }
            Destroy(this.gameObject);
        }
        else {
            ScaleUpDown(1.2f, .05f);
            UpdateText();
        }
    }
    private void FlyTowards() {
        float zOffsetFromObstacle = 3f;
        float jumpPower = 1f;
        float duration = .6f;
        int numJump = 1;

        holdObject.transform.parent = null;
        Vector3 FlyToPlayer = PlayerController.Instance.CurrentPos() + Vector3.forward * 2;
        Vector3 flyToForward = transform.position + new Vector3(0, 0.050f - transform.position.y, zOffsetFromObstacle);
        Vector3 fallPoint = isEndGameObstacle ? FlyToPlayer : flyToForward;
        holdObject.transform.DOJump(fallPoint, jumpPower, numJump, duration).OnComplete(() => {
            if (isEndGameObstacle) {
                Destroy(holdObject.gameObject);
                GameManager.Instance.AddMoneyToTotal(_endHoldedMoney);
                UIManager.Instance.TotalMoneyScaleUpDown(1.2f,.2f);
            }
        });
    }

    public void Crash() {
        bool isOver = false;
        if(isEndGameObstacle) return;
        if(PlayerController.Instance.transform.childCount <= howManyRemoveFromPlayer) {
            howManyRemoveFromPlayer = PlayerController.Instance.transform.childCount;
            isOver = true;
            if (GunTransfromController.Instance.StickmanListCount() == 0) {
                GameManager.Instance.Fail();
                return;
            }
        }
        var lastMembers = GunTransfromController.Instance.LastMembers(howManyRemoveFromPlayer);
        foreach (Transform t in lastMembers) {
            t.parent = null;
            GunTransfromController.Instance.RemoveFromRunnersGroup(lastMembers);
            t.GetComponent<CollectibleBots>().FallFromPlayer();
        }
        if (isOver) {
            if(PlayerController.Instance.transform.childCount <= howManyRemoveFromPlayer) {
                GameManager.Instance.Fail();
            }
        }
    }

    private void UpdateText() {
        valueText.text = value.ToString();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out PlayerController playerController)) {
            Crash();
            if (isEndGameObstacle) {
                GameManager.Instance.EndGame();
            }
        }
    }
    public bool Targetable() {
        return value >= 1;
    }

    public Transform ReturnTransfrom() {
        return transform;
    }


    private void ScaleUpDown(float scaleMax, float scaleTime) {
        transform.DOScale(scaleMax, scaleTime).OnComplete(() => { transform.DOScale(1, scaleTime); });
    }
    
    
}