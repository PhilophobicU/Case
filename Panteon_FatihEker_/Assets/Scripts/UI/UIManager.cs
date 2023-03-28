using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    
    public static UIManager Instance;
    [SerializeField] private GameObject produceError;
    [SerializeField] private GameObject roadBlocked;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject upgradesPanel;
    [SerializeField] private GameObject clock;
    [SerializeField] private GameObject Stats;
    [SerializeField] private GameObject production; // Buttons Parent
    [SerializeField] private GameObject flagPlaced;
    [SerializeField] private Image buildingImage;
    [SerializeField] private Image greenFillClock;
    public Button[] levels;
    public Button[] upgrades;
    [SerializeField] private TMP_Text flagNotPlaced;
    [SerializeField] private TMP_Text buildingHealth;
    [SerializeField] private TMP_Text productsTxt; // Fixed
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text buildingLevel;
    [SerializeField] private TMP_Text buildingNameTxt;
    private FactoryBase _selectedBase;
    private bool _processing;


    private void OnFlagIsMissingToggle(bool state) {
        flagPlaced.SetActive(state);
        flagNotPlaced.gameObject.SetActive(!state);
    }
    private void Awake() {
        Instance = this;
    }
    private void Start() {
        FactoryProductGenerator.Instance.FactorySelected += OnFactorySelected;
        FactoryProductGenerator.Instance.FactoryDeselected += OnFactoryDeselected;
        upgrades[0].onClick.AddListener(StartCountdown);
        upgrades[1].onClick.AddListener(StartCountdown);
    }

    public void SubscribeHealthChangedMethod(FactoryBase factoryBase) {
        factoryBase.HealthChanged += FactoryBaseOnHealthChanged;
        factoryBase.OnDeath += FactoryBaseOnDeath;
    }

    private void FactoryBaseOnHealthChanged(FactoryBase factoryBase) {
        if (_selectedBase != factoryBase) return;
        buildingHealth.text = "Health : " + factoryBase.Health;
    }
    private void FactoryBaseOnDeath(FactoryBase factoryBase) {
        if(_selectedBase!= factoryBase)return;
        factoryBase.OnDeath -= FactoryBaseOnDeath;
        OnFactoryDeselected();
    }

    public void OnFactorySelected(FactoryBase building) {
        infoPanel.SetActive(true);
        if (building.GetType() == typeof(BuildingBase)) {
            BuildingBase b = (BuildingBase)building;
            buildingImage.sprite = b.Visual;
            buildingHealth.text = "Health : " + b.Health;
            buildingLevel.text = "Level : " + b.Level;
            buildingNameTxt.text = b.NameString;
            _selectedBase = b;
            switch (b.Type) {
                case BuildingType.Barrack:
                    if (b.FlagGrid == null) {
                        OnFlagIsMissingToggle(false);
                    }
                    else {
                        OnFlagIsMissingToggle(true);
                        production.SetActive(true);
                        productsTxt.gameObject.SetActive(true);
                        buildingLevel.gameObject.SetActive(true);
                        //productsTxt.text = factoryBase.ProductString;
                        for (int i = 0; i < levels.Length; i++) {
                            levels[i].interactable = (b.Level - 1) >= i;
                        }
                    }
                    break;
                case BuildingType.PowerPlant:
                    break;
                case BuildingType.UpgradeCenter:
                    if (b.FlagGrid == null) {
                        OnFlagIsMissingToggle(false);
                    }
                    else {
                        for (int i = 0; i < upgrades.Length; i++) {
                            upgrades[i].interactable = i == b.Level - 1;
                        }
                        OnFlagIsMissingToggle(true);
                        upgradesPanel.SetActive(true);
                        buildingLevel.gameObject.SetActive(true);
                        buildingLevel.text = "Level : " + b.Level;
                    }
                    break;
            }
        }
        if (building.GetType() == typeof(SoldierBase)) {
            OnFlagIsMissingToggle(true);
            SoldierBase s = (SoldierBase)building;
            buildingImage.sprite = s.Visual;
            buildingHealth.text = "Health : " + s.Health;
            buildingLevel.text = "Level : " + s.Level;
            buildingNameTxt.text = s.NameString;
            switch (s.Type) {
                case SoldierType.BasicSoldier:
                    Stats.SetActive(true);
                    damageText.text = "Damage : " + s.Damage;
                    _selectedBase = s;
                    break;
            }
        }
    }

    public void OnFactoryDeselected() {
        production.SetActive(false);
        productsTxt.gameObject.SetActive(false);
        infoPanel.SetActive(false);
        flagNotPlaced.gameObject.SetActive(false);
        Stats.SetActive(false);
        _selectedBase = null;
        buildingLevel.gameObject.SetActive(false);
        upgradesPanel.SetActive(false);
    }


    public IEnumerator SoldierBlocked() {
        roadBlocked.SetActive(true);
        yield return new WaitForSeconds(2f);
        roadBlocked.SetActive(false);
    }

    public IEnumerator BuildingBlocked() {
        produceError.SetActive(true);
        yield return new WaitForSeconds(2f);
        produceError.SetActive(false);
    }

    private void StartCountdown() {
        if(_processing) return;
        StartCoroutine(Countdown());
    }

    public IEnumerator Countdown() {
        _processing = true;
        var UpgradeCenter = FactoryProductGenerator.Instance.GetCurrentBuildingData();
        var UpgradedData = UpgradeCenter.FlagGrid.GetPlacedObject();
        greenFillClock.fillAmount = 0;
        clock.SetActive(true);
        float a = 5;
        while (true) {
            a -= Time.deltaTime;
            if (a < 0) {
                UpgradeCenter.Level++;
                UpgradedData.Level++;
                clock.SetActive(false);
                var currentfactory = FactoryProductGenerator.Instance.GetCurrentBuildingData();
                OnFactoryDeselected();
                if(currentfactory != null)OnFactorySelected(currentfactory);
                _processing = false;
                yield break;
            }
            greenFillClock.fillAmount = math.remap(5, 0, 0, 1, a);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

}