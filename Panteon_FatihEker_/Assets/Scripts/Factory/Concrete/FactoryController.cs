using System;
using UnityEngine;
public class FactoryController : MonoBehaviour {
    public static FactoryController Instance { get; private set; }
    private Factory _barrackFactory;
    private Factory _powerPlantFactory;
    private Factory _soldierFactory;
    private Factory _upgradeFactory;
    private Factory _healerFactory;
    private void Awake() {
        Instance = this;
        _barrackFactory = new BarrackFactory();
        _powerPlantFactory = new PowerPlantFactory();
        _soldierFactory = new SoldierFactory();
        _upgradeFactory = new UpgradeFactory();
        _healerFactory = new HealerFactory();
    }

    public FactoryBase CreateBuilding(BuildingType type) {
        Factory factory;
        switch (type) {
            case BuildingType.Barrack:
                factory = _barrackFactory;
                break;
            case BuildingType.PowerPlant:
                factory = _powerPlantFactory;
                break;
            case BuildingType.UpgradeCenter:
                factory = _upgradeFactory;
                break;
            case BuildingType.Healing:
                factory = _healerFactory;
                break;
            default:
                throw new ArgumentException("Invalid building type");
        }

        BuildingBase buildingBase = factory.CreateBuilding();
        return buildingBase;
    }

    public FactoryBase CreateSoldier(SoldierType type, int level) {
        Factory factory;
        switch (type) {
            case SoldierType.BasicSoldier:
                factory = _soldierFactory;
                break;
            default:
                throw new ArgumentException("Invalid building type");
        }

        SoldierBase building = factory.CreateSoldier(level);
        return building;
    }
}