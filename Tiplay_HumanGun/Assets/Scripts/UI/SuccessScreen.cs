using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SuccessScreen : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Button button;
    private void Start() {
        button.onClick.AddListener(NextLevelButton);
        GameManager.Instance.OnGameEndWithSuccess += Appear;
    }

    private void Appear(object sender, EventArgs e) {
        DOVirtual.Float(0, 1, 1, v => {
            GetComponent<CanvasGroup>().alpha = v;
        }).OnComplete(() => {
            button.transform.DOScale(1, 1).SetEase(Ease.OutBack);
        });
    }
    
    
    private void NextLevelButton() {
        GameManager.Instance.IncreaseLevel();
        SceneManagement.Instance.NextScene();
    }
}
