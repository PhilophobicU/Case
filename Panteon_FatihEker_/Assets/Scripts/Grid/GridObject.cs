using UnityEngine;
public class GridObject {
    private Grid<GridObject> grid;
    public int x;
    public int y;
    public FactoryBase placedObject;
    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public GridObject cameFromNode;

    public GridObject(Grid<GridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        placedObject = null;
        isWalkable = true;

    }

    public Vector2Int GetXYfromGrid() {
        return new Vector2Int(x, y);
    }

    public override string ToString() {
        return x + ", " + y + "\n" + placedObject;
    }

    public void SetPlacedObject(FactoryBase placedObject) {
        bool gridFilled = placedObject != null;
        this.placedObject = placedObject;
        this.isWalkable = !gridFilled;
        grid.TriggerGridObjectChanged(x, y);
    }

    public FactoryBase GetPlacedObject() {
        return placedObject;
    }

    public bool CanBuild() {
        return placedObject == null;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }
}