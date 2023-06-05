using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject stickmanPrefab;
    public static GameManager Instance;
    public event EventHandler OnGameEndWithSuccess;
    public event EventHandler OnGameEndWithFail;
    public int incomeLevel = 1;
    public int incomeButtonValue = 10;
    public int stickmanLevel = 1;
    public int stickmanButtonValue = 10;
    public int totalMoney;
    public int currentLevel;
    private bool _isGameStarted;
    private bool _isGameEnded;

    private void Awake() {
        Instance = this;
        GetPrefs();
    }
    private void Start() {
        LoadFromSave();
    }
    private void Update() {
        if (!_isGameStarted && Input.GetMouseButtonDown(0) && !IsMouseOverUI()) StartGame();
    }
    private void StartGame() {
        _isGameStarted = true;
        PlayerController.Instance.RunAnimation(_isGameStarted);
        UIManager.Instance.CloseButtons();
        CameraHandler.Instance.Switch(CameraType.Playing);
    }
    public bool IsGameStarted() {
        return _isGameStarted;
    }
#if UNITY_EDITOR
    private bool IsMouseOverUI() => EventSystem.current.IsPointerOverGameObject();
    #else
    public bool IsMouseOverUI() => EventSystem.current.IsPointerOverGameObject(0);
#endif
    public void EndGame() {
        OnGameEndWithSuccess?.Invoke(this, EventArgs.Empty);
        _isGameEnded = true;
        _isGameStarted = false;
        CameraHandler.Instance.Switch(CameraType.End);
    }

    public void Fail() {
        _isGameEnded = true;
        _isGameStarted = false; 
        OnGameEndWithFail?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGameEnded() {
        return _isGameEnded;
    }

    public void IncomePressed(bool isLoadingAtBegining) {
        if (!isLoadingAtBegining) {
            if (totalMoney - incomeButtonValue < 0) return;
            totalMoney -= incomeButtonValue;
            PlayerPrefs.SetInt("TOTAL_MONEY", totalMoney);
            incomeLevel++;
            PlayerPrefs.SetInt("INCOME_LEVEL", incomeLevel);
            incomeButtonValue *= 2;
            PlayerPrefs.SetInt("INCOME_BUTTON_VALUE", incomeButtonValue);
        }
        UIManager.Instance.UpdateTexts();
        
    }
    public void LoadFromSave() {
        stickmanLevel = PlayerPrefs.GetInt("STICKMEN_LEVEL", 1);
        if (stickmanLevel - 1 == 0) return;
        for (int i = 0; i < stickmanLevel - 1; i++) {
            StickmanPressed(true);
        }
        UIManager.Instance.UpdateTexts();
    }
    private void GetPrefs() {
        stickmanButtonValue = PlayerPrefs.GetInt("STICK_BUTTON_VALUE", 10);
        totalMoney = PlayerPrefs.GetInt("TOTAL_MONEY", 0);
        currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 1);
        incomeLevel = PlayerPrefs.GetInt("INCOME_LEVEL",1);
        incomeButtonValue = PlayerPrefs.GetInt("INCOME_BUTTON_VALUE",10);
    }

    public void StickmanPressed(bool isLoadingAtBegining) {
        if (GunTransfromController.Instance.StickmanListCount() > PlayerController.Instance.allowedTransformCount) return;
        if (!isLoadingAtBegining) {
            if (totalMoney - stickmanButtonValue < 0) return;
            totalMoney -= stickmanButtonValue;
            PlayerPrefs.SetInt("TOTAL_MONEY", totalMoney);
            stickmanLevel++;
            PlayerPrefs.SetInt("STICKMEN_LEVEL", stickmanLevel);
            stickmanButtonValue *= 2;
            PlayerPrefs.SetInt("STICK_BUTTON_VALUE", stickmanButtonValue);
        }
        GameObject t = Instantiate(stickmanPrefab, isLoadingAtBegining ? GunTransfromController.Instance.GetList().stickmans[PlayerController.Instance.transform.childCount - 1].localPositions : transform.position + transform.up, isLoadingAtBegining ? Quaternion.Euler(GunTransfromController.Instance.GetList().stickmans[PlayerController.Instance.transform.childCount - 1].eulerAngles) : Quaternion.identity);
        if (GunTransfromController.Instance.StickmanListCount() < 2) {
            FirstTime(t);
            return;
        }
        GunTransfromController.Instance.AddToRunnersGroup(t.transform);
        UIManager.Instance.UpdateTexts();
    }
    private void FirstTime(GameObject t) {
        GunTransfromController.Instance.AddToRunnersGroup(t.transform);
        GunTransfromController.Instance.AddToRunnersGroup(PlayerController.Instance.transform.GetChild(0));
        PlayerController.Instance.RunningAnimationOff();
        PlayerPrefs.SetInt("STICKMEN_LEVEL", stickmanLevel);
        UIManager.Instance.UpdateTexts();
    }

    public void AddMoneyToTotal(int addAmount) {
        totalMoney += addAmount * incomeLevel;
        
        PlayerPrefs.SetInt("TOTAL_MONEY", totalMoney);
        UIManager.Instance.UpdateTexts();
    }
    public void IncreaseLevel() {
        currentLevel++;
        PlayerPrefs.SetInt("CURRENT_LEVEL",currentLevel);
    }
}