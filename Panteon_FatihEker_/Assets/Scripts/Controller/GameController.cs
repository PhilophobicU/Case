using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour {
    public static GameController Instance { get; set; }
    [FormerlySerializedAs("factoryBaseCreator")]
    public FactoryProductGenerator factoryProductGenerator;
    private Vector3 _startPos;
    private int _currentLeveL;
    private IMouse _mouseInput;
    
    private void Awake() {
        Instance = this;
        factoryProductGenerator = FactoryProductGenerator.Instance;
        _mouseInput = new MouseInputHandler(factoryProductGenerator);
    }
    private void Update() {
        _mouseInput.Tick();
        if(Input.GetKeyDown(KeyCode.A)) {
            BarrackButton();
        }
    }
    

    public void CreateSoldierButton(int level) {
        _currentLeveL = level;
        BuildingBase basee = (BuildingBase)_mouseInput.SelectedObject.GetComponent<BaseModel>().GetFactoryBase();
        PathRequestManager.RequestPath(basee.OriginGrid.x, basee.OriginGrid.y, basee.FlagGrid.x, basee.FlagGrid.y, Create);
    }
    private void Create(List<GridObject> path, bool isSuccessful) {
        if (isSuccessful) {
            FactoryProductGenerator.Instance.CreateSoldier(_currentLeveL);
        }
        else {
            StartCoroutine(UIManager.Instance.BuildingBlocked());
            StartCoroutine(UIManager.Instance.BuildingBlocked());
        }
    }

    public void BarrackButton() {
        factoryProductGenerator.CreateBarrack();
    }
    public void PowerPlantButton() {
        factoryProductGenerator.CreatePowerPlant();
    }
    public void UpgradeCenter() {
        factoryProductGenerator.CreateUpgradeBuilding();
    }

    public void HealerButton() {
        factoryProductGenerator.CreateHealer();
    }
}