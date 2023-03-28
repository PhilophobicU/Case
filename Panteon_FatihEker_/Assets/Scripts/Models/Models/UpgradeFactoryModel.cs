using System;
using UnityEngine;
public class UpgradeFactoryModel : BaseModel ,IHasRightClick{
    private BuildingBase _upgradeFactoryData;
    private BuildingBase _child;
    private GameObject _selectedVisual;
    public Sprite sprite;


    public override FactoryBase GetFactoryBase() => _upgradeFactoryData;

    public override void SetBuildingModel(FactoryBase factoryBase) => _upgradeFactoryData = (BuildingBase)factoryBase;

    private void Start() {
        GameController.Instance.factoryProductGenerator.FactorySelected += ShowFlag;
        GameController.Instance.factoryProductGenerator.FactoryDeselected += HideFlag;
    }

    public void MarkDestination(bool isUserInput) {
        Vector3 mousePosition = GridUtils.GetMouseWorldPosition();
        var selectedGrid = GridCreation.Instance.GetGrid().GetGridObjectWithPos(mousePosition);
        if (selectedGrid.isWalkable) return;
        if(!Equals(selectedGrid.GetPlacedObject().GetEnumType(),BuildingType.Barrack)) return;
        if ( _selectedVisual == null) {
            if (_child == null) {
                _child = (BuildingBase)selectedGrid.GetPlacedObject(); 
                if (_child.Parent != null) return;      
                _selectedVisual = GridUtils.NewSprite(selectedGrid, sprite,transform);    
                _child.Parent = _upgradeFactoryData;  
                _upgradeFactoryData.FlagGrid = _child.OriginGrid;  
            }
        }
        else {
            if(selectedGrid.GetPlacedObject().Parent != null) return;
            if (_child.Parent == (BuildingBase)selectedGrid.GetPlacedObject()) return;
            _child.Level = 1;
            _child.Parent = null; 
            _child = (BuildingBase)selectedGrid.GetPlacedObject(); 
            _child.Parent = _upgradeFactoryData;  
            _upgradeFactoryData.FlagGrid = _child.OriginGrid; 
            _selectedVisual.transform.position = GridCreation.Instance.GetGrid().GetWorldPosition(selectedGrid.GetPlacedObject().OriginGrid.x, selectedGrid.GetPlacedObject().OriginGrid.y)+Vector3.one /2;
            _upgradeFactoryData.Level = 1;
        }
    }

    void ShowFlag(FactoryBase factoryBase) {
        if (_upgradeFactoryData != factoryBase) return; // Cancel this if you want to see all flags at the same time
        if (_selectedVisual != null) _selectedVisual.gameObject.SetActive(true);
    }
    void HideFlag() {
        if (_selectedVisual != null) _selectedVisual.gameObject.SetActive(false);
    }
    private void OnDestroy() {
        print("Den");
        if(_upgradeFactoryData == null) return;
        if (_upgradeFactoryData.FlagGrid.GetPlacedObject() == null) return;
        _upgradeFactoryData.FlagGrid.GetPlacedObject().Parent = null;
        _upgradeFactoryData.FlagGrid.GetPlacedObject().Level = 1;
    }
}