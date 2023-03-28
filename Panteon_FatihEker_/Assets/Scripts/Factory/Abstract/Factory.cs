using System.Collections.Generic;
using UnityEngine;
public abstract class Factory {
    public virtual BuildingBase CreateBuilding() {
        return null;
    }
    public virtual SoldierBase CreateSoldier(int level) {
        return null;
    }
}

public class BarrackFactory : Factory {

    public override BuildingBase CreateBuilding() {
        BuildingBase buildingBase = new BuildingBase() {
            Level = 1,
            FlagGrid = null,
            OriginGrid = null,
            Type = BuildingType.Barrack,
            Health = 100,
            NameString = "Soldier Barrack",
            Prefab = (GameObject)Resources.Load("BarrackModel"),
            Visual = (Sprite)Resources.Load("Sprites/Barrack"),
            Width = 4,
            Height = 4,
            IsFlagNeeded = true,
            IsHasProduct = true
        };
        return buildingBase;
    }
}
public class PowerPlantFactory : Factory {
    public override BuildingBase CreateBuilding() {
        BuildingBase buildingBase = new BuildingBase {
            Level = 1,
            OriginGrid = null,
            FlagGrid = null,
            Type = BuildingType.PowerPlant,
            Health = 50,
            NameString = "Power Plant",
            Prefab = (GameObject)Resources.Load("PowerPlantModel"),
            Visual = (Sprite)Resources.Load("Sprites/energy"),
            Width = 2,
            Height = 3,
            IsFlagNeeded = false,
            IsHasProduct = false
        };
        return buildingBase;
    }
}

public class SoldierFactory : Factory {
    public override SoldierBase CreateSoldier(int level) {
        
        SoldierBase soldierBase = new SoldierBase {
            Parent = null,
            OriginGrid = null,
            Type = SoldierType.BasicSoldier,
            Health = 10,
            NameString = "Soldier",
            Prefab = (GameObject)Resources.Load("SoldierLevel1Model"),
            
            Width = 1,
            Height = 1,
            Level = level
        };
        switch (level) {
            case 1:
                soldierBase.Damage = 2;
                soldierBase.Visual = (Sprite)Resources.Load("Sprites/ironSword");
                break;
            case 2:
                soldierBase.Damage = 5;
                soldierBase.Visual = (Sprite)Resources.Load("Sprites/goldSword");
                break;
            case 3:
                soldierBase.Damage = 10;
                soldierBase.Visual = (Sprite)Resources.Load("Sprites/diamondSword");
                break;
        }

        return soldierBase;
    }
}

public class UpgradeFactory : Factory {
    public override BuildingBase CreateBuilding() {
        BuildingBase buildingBase = new BuildingBase {
            Level = 1,
            OriginGrid = null,
            FlagGrid = null,
            Type = BuildingType.UpgradeCenter,
            Health = 75,
            NameString = "Upgrade Factory",
            Prefab = (GameObject)Resources.Load("UpgradeFactoryPrefab"),
            Visual = (Sprite)Resources.Load("Sprites/research3"),
            Width = 5,
            Height = 3,
            IsFlagNeeded = true,
            IsHasProduct = false
        };
        return buildingBase;
    }
}

public class HealerFactory : Factory {
    public override BuildingBase CreateBuilding() {
        BuildingBase buildingBase = new BuildingBase {
            Level = 1,
            OriginGrid = null,
            FlagGrid = null,
            Type = BuildingType.Healing,
            Health = 20,
            NameString = "Healer",
            Prefab = (GameObject)Resources.Load("Healer"),
            Visual = (Sprite)Resources.Load("Sprites/healer"),
            Width = 2,
            Height = 2,
            IsFlagNeeded = false,
            IsHasProduct = false
        };
        return buildingBase;
    }
}