using System;
using System.Collections.Generic;
using UnityEngine;
public class BuildingBase : FactoryBase {
    public List<GridObject> OccupiedGrids { get; set; } 
    public BuildingType Type { get; set; }
    public GridObject FlagGrid { get; set; }
    public bool IsHasProduct { get; set; }
    public bool IsFlagNeeded { get; set; }
    

    public override List<GridObject> ReturnGrids() {
        return OccupiedGrids;
    }

    public override Enum GetEnumType() {
        return Type;
    }
}