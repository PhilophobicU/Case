    using System;
    using UnityEngine;
    public class FactoryProductGenerator : MonoBehaviour  {

        public static FactoryProductGenerator Instance { get; set; }
        public event Action<FactoryBase> FactorySelected;
        public event Action FactoryDeselected;
        public event Action OnSelectedChanged;
        private FactoryBase _factoryBaseToPlaced;
        private BuildingBase _selectedBase;
        private SoldierBase _selectedSoldier;
        public bool factoryCreated = false;
        

        private void Awake() {
            Instance = this;
        }

        public BaseModel PlaceBuilding() {
            ObjectPlacer objectPlacer = ObjectPlacer.Instance;
            GameObject placedObject = objectPlacer.PlaceBuildingToGrid(this);
            if (placedObject == null) return null;
            var buildingModel = placedObject.GetComponent<BaseModel>();
            _factoryBaseToPlaced.Model = buildingModel;
            SelectModel(_factoryBaseToPlaced);
            ClearPlacedObject();
            RefreshSelectedObjectType();
            return buildingModel;
        }


        public void CreateBarrack() {
            FactoryDeselect();
            factoryCreated = true;
            _factoryBaseToPlaced =  FactoryController.Instance.CreateBuilding(BuildingType.Barrack);
            GetComponent<FactoryBaseDestroy>().Sub(_factoryBaseToPlaced);
            RefreshSelectedObjectType();
        }

        public void CreatePowerPlant() {
            FactoryDeselect();
            factoryCreated = true;
            _factoryBaseToPlaced = FactoryController.Instance.CreateBuilding(BuildingType.PowerPlant);
            GetComponent<FactoryBaseDestroy>().Sub(_factoryBaseToPlaced);
            RefreshSelectedObjectType();
        }
        public void CreateUpgradeBuilding() {
            FactoryDeselect();
            factoryCreated = true;
            _factoryBaseToPlaced = FactoryController.Instance.CreateBuilding(BuildingType.UpgradeCenter);
            GetComponent<FactoryBaseDestroy>().Sub(_factoryBaseToPlaced);
            RefreshSelectedObjectType();
        }
        
        
        public void CreateSoldier(int level) {
            factoryCreated = true; 
            _factoryBaseToPlaced = FactoryController.Instance.CreateSoldier(SoldierType.BasicSoldier,level);
            _factoryBaseToPlaced.Parent = _selectedBase;
            GameObject g = ObjectPlacer.Instance.PlaceUnitToGrid((SoldierBase)_factoryBaseToPlaced,_selectedBase);
            _factoryBaseToPlaced.Model = g.GetComponent<SoldierModel>();
            FactorySelected?.Invoke(_selectedBase);
            _factoryBaseToPlaced.OnDeath += ObjectVisualizer.Instance.DestroyBuildingVisual;
            GetComponent<FactoryBaseDestroy>().Sub(_factoryBaseToPlaced);
            ClearPlacedObject();
        }

        public void CreateHealer() {
            FactoryDeselect();
            factoryCreated = true;
            _factoryBaseToPlaced =  FactoryController.Instance.CreateBuilding(BuildingType.Healing);
            GetComponent<FactoryBaseDestroy>().Sub(_factoryBaseToPlaced);
            RefreshSelectedObjectType();
        }
        

        public void SelectModel(FactoryBase factoryBase) {
            FactorySelected?.Invoke(factoryBase);
            if (factoryBase.GetType() == typeof(SoldierBase)) _selectedSoldier = (SoldierBase)factoryBase;
            if (factoryBase.GetType() == typeof(BuildingBase)) _selectedBase = (BuildingBase)factoryBase;
        }
        
        public void FactoryDeselect() {
            FactoryDeselected?.Invoke();
            _selectedBase = null;
            _selectedSoldier = null;
        }
        public void RefreshSelectedObjectType() {
            OnSelectedChanged?.Invoke();
        }
        public void ClearPlacedObject() {
            _factoryBaseToPlaced = null;
            factoryCreated = false;
        }

        public FactoryBase GetCurrentFactoryData() {
            return _factoryBaseToPlaced;
        }
        
        public SoldierBase GetCurrentSoldierData() {
            return _selectedSoldier;
        }
        public BuildingBase GetCurrentBuildingData() {
            return _selectedBase;
        }
    }
