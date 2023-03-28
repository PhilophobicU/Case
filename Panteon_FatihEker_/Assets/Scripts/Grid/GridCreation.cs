using UnityEngine;

public class GridCreation : MonoBehaviour {
     static Grid<GridObject> _grid;
    public static GridCreation Instance;
    private ICalculateGridData _gridData;
    void Awake() {
        Instance = this;
        Camera.main.orthographicSize = (Camera.main.pixelHeight / 2f) / 32f;
        _gridData = GetComponent<ICalculateGridData>();
        float cellSize = 1;
        int x = _gridData.CalculateData().x;
        int y = _gridData.CalculateData().y;
        _grid = new Grid<GridObject>(x, y, cellSize, _gridData.GetPosition(), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    public Grid<GridObject> GetGrid() {
        return _grid;
    }
}