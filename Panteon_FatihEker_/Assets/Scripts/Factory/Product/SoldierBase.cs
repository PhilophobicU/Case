using System;
using System.Collections.Generic;
public class SoldierBase : FactoryBase {
    public SoldierType Type { get; set; }
    public int MaxHealth { get { return 10; }}
    
    public int Damage { get; set; }

    public override List<GridObject> ReturnGrids() {
        var list = new List<GridObject>();
        list.Add(OriginGrid);
        return list;
    }

    public override Enum GetEnumType() {
        return Type;
    }
}