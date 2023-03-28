using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class HealerModel : BaseModel {
    private BuildingBase _healerData;
    private int _healPerSec = 1;
    private List<GridObject> _environmentGrids;
    private int _startCount;
    private float _timeBetweenHeals = 2;
    private float _time;
    private bool _checkable;

    private void OnPlaced() {
       var pathfindingHelper = new PathfindingCalculations();
       _environmentGrids = new List<GridObject>();
       _environmentGrids = pathfindingHelper.EnvironmentGrids(_healerData.OccupiedGrids);
       _startCount = _environmentGrids.Count;
       _checkable = true;
       pathfindingHelper = null;
    }

    private void Update() {     // event maybe
        if (!_checkable) return;
        if (_startCount == _environmentGrids.Count(g => g.isWalkable)) {
            return;
        }
        
        
        
        _time -= Time.deltaTime;
        if(_time <= 0) {
            foreach (GridObject grid in _environmentGrids.Where(g => !g.isWalkable && g.GetPlacedObject().Height == 1)) {
                SoldierBase a= (SoldierBase)grid.GetPlacedObject();
                if (a.Health < a.MaxHealth) {
                    a.Health += _healPerSec;
                }
            }
            _time += _timeBetweenHeals;
        }
        

    }


    public override FactoryBase GetFactoryBase() {
        return _healerData;
    }
    public override void SetBuildingModel(FactoryBase factoryBase) {
        _healerData = (BuildingBase)factoryBase;
        OnPlaced();
    }
}