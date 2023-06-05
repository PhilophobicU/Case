using System;
using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour {
    [SerializeField] private GameObject stickmanPrefab;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private int value;
    private bool _triggered = false;
    private void Start() {
        valueText.text = value < 0 ? value.ToString() : "+" + value;
    }

    public void Interact() {
        if (!_triggered) {
            _triggered = true;
            if (value > 0) {

                if (!PlayerController.Instance.IsLevelLimitReached()) {
                    AddMoneyInsteadStickman(value);
                }
                else {
                    int howManySoldierDoWeNeedToLimit = value <= PlayerController.Instance.DifferenceBetweenRunnersAndAllowed() ? value : PlayerController.Instance.DifferenceBetweenRunnersAndAllowed();
                    int moneyIndex = value;
                    for (int i = 0; i < howManySoldierDoWeNeedToLimit; i++) {
                        GameObject g = Instantiate(stickmanPrefab, transform.position + transform.up, Quaternion.identity);
                        bool firstTime = GunTransfromController.Instance.StickmanListCount() < 2;
                        if (firstTime) {
                            GunTransfromController.Instance.AddToRunnersGroup(g.transform);
                            GunTransfromController.Instance.AddToRunnersGroup(PlayerController.Instance.transform.GetChild(0));
                            moneyIndex -= 2;
                        }
                        GunTransfromController.Instance.AddToRunnersGroup(g.transform);
                        moneyIndex--;
                    }
                    if(moneyIndex > 0)AddMoneyInsteadStickman(moneyIndex);
                    GunTransfromController.Instance.ReOrderList(GunTransfromController.Instance.GetList());
                }
            }
            else {
                int posValue = Mathf.Abs(value);
                bool isOver = false;
                if (PlayerController.Instance.transform.childCount <= posValue) {
                    posValue = PlayerController.Instance.transform.childCount;
                    isOver = true;
                }
                if (isOver) {
                    GameManager.Instance.Fail();
                    return;
                }

                var lastMembers = GunTransfromController.Instance.LastMembers(Mathf.Abs(posValue));
                foreach (Transform t in lastMembers) {
                    t.parent = null;
                    GunTransfromController.Instance.RemoveFromRunnersGroup(lastMembers);
                    t.GetComponent<CollectibleBots>().FallFromPlayer();
                }

            }
        }
    }
    private void AddMoneyInsteadStickman(int val) {
        for (int i = 0; i < val; i++) {
            int gateEarning = 1;
            UIManager.Instance.CollectMoneyWorldToUI(transform, gateEarning);
        }
    }
}