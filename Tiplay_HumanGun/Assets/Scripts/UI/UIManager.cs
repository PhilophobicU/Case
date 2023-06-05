using System;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    public static UIManager Instance;
    [SerializeField] private Image moneySprite;
    [SerializeField] private GameObject totalMoneySprite;

    [SerializeField] private Button stickmanButton;
    [SerializeField] private Button incomeButton;

    [SerializeField] private TMP_Text stickmanValueTxt;
    [SerializeField] private TMP_Text stickmanValueLevel;

    [SerializeField] private TMP_Text incomeValueTxt;
    [SerializeField] private TMP_Text incomeLevelTxt;
    [SerializeField] private TMP_Text currentLevelText;

    [SerializeField] private TMP_Text totalMoneyTxt;


    public void CloseButtons() {
        stickmanButton.transform.DOScale(0, .4f).SetEase(Ease.InBack);
        incomeButton.transform.DOScale(0, .4f).SetEase(Ease.InBack);
    }
    private void Awake() {
        Instance = this;
        stickmanButton.onClick.AddListener(STICKMAN_BUTTON);
        incomeButton.onClick.AddListener(INCOME_BUTTON);
        if(!SceneManagement.Instance.IsTutorial()) {
            stickmanButton.transform.DOScale(1, 1).SetEase(Ease.OutBack);
            incomeButton.transform.DOScale(1, 1).SetEase(Ease.OutBack);
        }
    }
    private void Start() {
        UpdateTexts();
    }

    public void UpdateTexts() {
        stickmanValueLevel.text = "LEVEL" + GameManager.Instance.stickmanLevel;
        stickmanValueTxt.text = "$" + GameManager.Instance.stickmanButtonValue;
        incomeLevelTxt.text = "LEVEL" + GameManager.Instance.incomeLevel;
        incomeValueTxt.text = "$" + GameManager.Instance.incomeButtonValue;
        totalMoneyTxt.text = "$" + GameManager.Instance.totalMoney;
        currentLevelText.text = "LEVEL " + GameManager.Instance.currentLevel;
    }

    public void CollectMoneyWorldToUI(Transform worldPos,int moneyAmount) {
        var screenPos = Camera.main.WorldToScreenPoint(worldPos.position);
        Image img = Instantiate(moneySprite, screenPos, quaternion.identity,transform);
        float baseScale = img.transform.localScale.x;
        img.transform.DOScale(baseScale * 2f, .4f).OnComplete(() => {
            img.transform.DOScale(baseScale, .2f);
        });
        img.transform.DOMove(totalMoneySprite.transform.position, 1f).OnComplete(() => {
            Destroy(img.gameObject);
            TotalMoneyScaleUpDown(1.2f,.2f);
            GameManager.Instance.AddMoneyToTotal(moneyAmount);
        });
        UpdateTexts();
    }

    public void TotalMoneyScaleUpDown(float maxScale,float time) {
        totalMoneySprite.transform.DOScale(maxScale, time).OnComplete(() => {
            totalMoneySprite.transform.DOScale(1, time);
        });
    }
    
    
    private void STICKMAN_BUTTON() {
        GameManager.Instance.StickmanPressed(false);
    }
    private void INCOME_BUTTON() {
        GameManager.Instance.IncomePressed(false);
    }
}