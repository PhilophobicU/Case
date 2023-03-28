using UnityEngine;

public class FactoryBaseDestroy : MonoBehaviour {

    public void Sub(FactoryBase factoryBase) {
        if(factoryBase.GetType() == typeof(BuildingBase)) factoryBase.OnDeath += BuildingDestroyer;
        if(factoryBase.GetType() == typeof(SoldierBase)) factoryBase.OnDeath += UnitDestroyer;
    }
    public void UnitDestroyer(FactoryBase soldier) {
        soldier.OriginGrid.SetPlacedObject(null);
        soldier.OnDeath -= UnitDestroyer;
    }
    public void BuildingDestroyer(FactoryBase building) {
        var buildingBase = (BuildingBase)building;
        foreach (GridObject node in buildingBase.OccupiedGrids) {
            node.SetPlacedObject(null);
        }
        buildingBase.OnDeath -= BuildingDestroyer;
    }
    
}