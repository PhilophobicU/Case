using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UIManager : MonoBehaviour {

    [SerializeField] private TMP_Text totalGoldTxt;
    [SerializeField] private CollectController _collect;
    [SerializeField] private GemInfo gemInfoPrefab;
    [SerializeField] private GameObject StatPanel;
    [SerializeField] private RectTransform Content;
    [SerializeField] private Button hideButton;
    [SerializeField] private Button showButton;
    private GemInfo[] _gemInfos;

    public event Action OnUpdated;


    private int _totalGold = 0;
    private void Start() {
        _gemInfos = new GemInfo[StatManager.Instance.stats.Length];
        CreateScroll();
        OnUpdated?.Invoke();
        _totalGold = PlayerPrefs.GetInt("TOTAL_GOLD",0);
        totalGoldTxt.text = _totalGold.ToString();
        _collect.OnSell += UpdateText;
        hideButton.onClick.AddListener(ShowHidePanel);
        showButton.onClick.AddListener(ShowHidePanel);
    }
    private void UpdateText(Gem gem) {
        OnUpdated?.Invoke();
        _totalGold += (int)(gem.GemType.startingSellValue + (gem.transform.localScale.x * 100));
        totalGoldTxt.text = _totalGold.ToString();
        PlayerPrefs.SetInt("TOTAL_GOLD", _totalGold);
    }

    private void CreateScroll() {
        for (int i = 0; i < _gemInfos.Length; i++) {
            GemInfo gemInfo = Instantiate(gemInfoPrefab, Content);
            gemInfo.SetValues(StatManager.Instance.stats[i]);
            _gemInfos[i] = gemInfo;
            OnUpdated += gemInfo.UpdateValue;
        }
        StatPanel.transform.localScale = Vector3.zero;
    }
    private void ShowHidePanel() {
        int scale = (int)StatPanel.transform.localScale.x;
        StatPanel.transform.DOScale(scale == 1 ? 0 : 1, 1).SetEase(scale == 1 ? Ease.InBack : Ease.OutBack);
    }

}