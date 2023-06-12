using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemInfo : MonoBehaviour {

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text gemTypeTxt;
    [SerializeField] private TMP_Text gemCountTxt;
    [SerializeField] private GemSO gemSo;
    public GemSO Type => gemSo;
    public void SetValues(GemSO s) {
        gemSo = s;
        image.sprite = s.gemIcon;
        gemTypeTxt.text = s.gemName;
        gemCountTxt.text = s.collectedAmount.ToString();
        gemSo.collectedAmount = PlayerPrefs.GetInt(gemSo.gemName);
    }

    public void UpdateValue() {
        gemCountTxt.text = gemSo.collectedAmount.ToString();
        PlayerPrefs.SetInt(gemSo.gemName, gemSo.collectedAmount);
    }

}