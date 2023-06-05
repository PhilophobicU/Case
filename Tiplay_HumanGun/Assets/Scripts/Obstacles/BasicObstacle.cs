using UnityEngine;

public class BasicObstacle : MonoBehaviour {
    [SerializeField] private int howManyRemoveFromPlayer = 1;
    public void Crash() {
        bool isOver = false;
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

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent(out PlayerController playerController)) {
            Crash();
        }
    }

}