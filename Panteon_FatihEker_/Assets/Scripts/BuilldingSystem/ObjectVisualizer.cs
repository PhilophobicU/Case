using System;
using UnityEngine;
public class ObjectVisualizer : MonoBehaviour {
    public static ObjectVisualizer Instance;
    private FactoryBase _barrackBase;
    private FactoryBase _powerPlantBase;
    private FactoryBase _upgradeBase;
    private FactoryBase _soldierBase;
    private FactoryBase _healerBase;
    private Transform _visual;
    private void Awake() {
        Instance = this;
        _visual = null;
        _barrackBase = FactoryController.Instance.CreateBuilding(BuildingType.Barrack);
        _powerPlantBase = FactoryController.Instance.CreateBuilding(BuildingType.PowerPlant);
        _upgradeBase = FactoryController.Instance.CreateBuilding(BuildingType.UpgradeCenter);
        _soldierBase = FactoryController.Instance.CreateSoldier(SoldierType.BasicSoldier,1);
        _healerBase = FactoryController.Instance.CreateBuilding(BuildingType.Healing);
    }

    public GameObject CreateBuildingVisual(Enum type, Vector3 pos) {

        GameObject prefab = null;

        if (type is BuildingType) {
            var buildingType = (BuildingType)type;
            switch (buildingType) {
                case BuildingType.Barrack:
                    prefab = _barrackBase.Prefab;
                    break;
                case BuildingType.PowerPlant:
                    prefab = _powerPlantBase.Prefab;
                    break;
                case BuildingType.UpgradeCenter:
                    prefab = _upgradeBase.Prefab;
                    break;
                case BuildingType.Healing:
                    prefab = _healerBase.Prefab;
                    break;
                default:
                    throw new ArgumentException("Invalid building type");
            }
        }
        else if (type is SoldierType) {
            var soldierType = (SoldierType)type;
            switch (soldierType) {
                case SoldierType.BasicSoldier:
                    prefab = _soldierBase.Prefab;
                    break;
                default:
                    throw new ArgumentException("Invalid soldier type");
            }
        }
        return Instantiate(prefab, pos, Quaternion.identity);
    }

    public void DestroyBuildingVisual(FactoryBase building) {
        IFlag flag = building.Model.GetComponent<IFlag>();
        if (flag != null) flag.UnSub();
        print(building.Model.gameObject.name);
        Destroy(building.Model.gameObject);
    }
    private void Start() {
        GameController.Instance.factoryProductGenerator.OnSelectedChanged += RefreshVisual;
    }
    private void LateUpdate() {
        Vector3 targetPosition = GridCreation.Instance.GetGrid().GetGridWorldPosFromMousePos();
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

        //transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem2D.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }

    private void RefreshVisual() {
        if (_visual != null) {
            Destroy(_visual.gameObject);
            _visual = null;
        }
        FactoryBase factoryBase = FactoryProductGenerator.Instance.GetCurrentFactoryData();
        if (factoryBase != null) {
            _visual = Instantiate(factoryBase.Prefab.transform, Vector3.zero, Quaternion.identity);
            factoryBase.OnDeath += DestroyBuildingVisual;
            _visual.parent = transform;
            _visual.localPosition = Vector3.zero;
            _visual.localEulerAngles = Vector3.zero;
        }
    }
}