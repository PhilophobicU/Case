using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public abstract class FactoryBase {
    public BaseModel Model;
    public abstract List<GridObject> ReturnGrids();

    public abstract Enum GetEnumType();


    public event Action<FactoryBase> HealthChanged;
    public event Action<FactoryBase> OnDeath;
    public BuildingBase Parent { get; set; }
    public int Level { get; set; }

    private int _health;
    public GridObject OriginGrid { get; set; }
    public int Health {
        get {
            return _health;
        }
        set {
            _health = value;
            if (_health <= 0) {
                OnDeath?.Invoke(this);
                OnDeath = null;
                HealthChanged = null;
            }
            else {

                HealthChanged?.Invoke(this);
            }
        }
    }
    public string NameString { get; set; }
    public GameObject Prefab { get; set; }
    public Sprite Visual { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    
    public enum Dir {
        Down,
        Left,
        Up,
        Right,
    }

    public Vector2Int GetRotationOffset(Dir dir) {
        switch (dir) {
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, Width);
            case Dir.Up: return new Vector2Int(Width, Height);
            case Dir.Right: return new Vector2Int(Height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir) {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir) {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < Height; x++) {
                    for (int y = 0; y < Width; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }

    public void HitDamage(int damage) {
        Health = Mathf.Max(_health - damage, 0);
    }
}


public enum SoldierType {
    BasicSoldier
}

public enum BuildingType {
    Barrack,
    PowerPlant,
    UpgradeCenter,
    Healing
}