using UnityEngine;
using UnityEngine.Serialization;

public class Money : MonoBehaviour {
    private int _moneyAmount = 1;
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out PlayerController playerController)) {
            UIManager.Instance.CollectMoneyWorldToUI(transform,_moneyAmount);
            Destroy(this.gameObject);
        }
    }

}