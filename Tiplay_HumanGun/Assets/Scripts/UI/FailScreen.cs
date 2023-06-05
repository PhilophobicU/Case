using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailScreen : MonoBehaviour {

    [SerializeField] private Image background;
    [SerializeField] private Button button;

    private void Start() {
        button.onClick.AddListener(RetryButton);
        GameManager.Instance.OnGameEndWithFail += Appear;
    }

    private void Appear(object sender, EventArgs e) {
        DOVirtual.Float(0, 1, 1, v => {
            GetComponent<CanvasGroup>().alpha = v;
        }).OnComplete(() => {
        button.transform.DOScale(1, 1).SetEase(Ease.OutBack);
        });
    }
    
    
    private void RetryButton() {
        SceneManagement.Instance.RestartScene();
    }

}