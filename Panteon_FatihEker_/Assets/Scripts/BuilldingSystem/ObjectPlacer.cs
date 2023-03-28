using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class ObjectPlacer : MonoBehaviour {

    public static ObjectPlacer Instance { get; private set; }

    private Grid<GridObject> _grid;
    private FactoryBase.Dir _dir;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _grid = GridCreation.Instance.GetGrid();
    }

    public GameObject PlaceUnitToGrid(SoldierBase product, BuildingBase producer) {
        product.OriginGrid = producer.OriginGrid;
        Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(producer.OriginGrid.x, producer.OriginGrid.y);
        GameObject placedObject = ObjectVisualizer.Instance.CreateBuildingVisual(product.Type, placedObjectWorldPosition);
        placedObject.GetComponent<BaseModel>().SetBuildingModel(product);
        placedObject.GetComponent<IHasRightClick>().MarkDestination(false);
        UIManager.Instance.SubscribeHealthChangedMethod(product);
        return placedObject;
    }


    public GameObject PlaceBuildingToGrid(FactoryProductGenerator building) {
        List<GridObject> gridObjectsList = new List<GridObject>();
        BuildingBase factoryBase = (BuildingBase)building.GetCurrentFactoryData();
        Vector3 mousePosition = GridUtils.GetMouseWorldPosition();
        _grid.GetXYFromMousePos(mousePosition, out int x, out int z);
        Vector2Int placedObjectOrigin = new Vector2Int(x, z);
        List<Vector2Int> gridPositionList = factoryBase.GetGridPositionList(placedObjectOrigin, _dir);
        factoryBase.OriginGrid = _grid.GetGridObject(placedObjectOrigin.x, placedObjectOrigin.y);
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList) {
            if (!_grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                canBuild = false;
                break;
            }
        }

        if (canBuild) {
            Vector2Int rotationOffset = factoryBase.GetRotationOffset(_dir);
            Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, rotationOffset.y) * _grid.GetCellSize();
            GameObject placedObject = ObjectVisualizer.Instance.CreateBuildingVisual(factoryBase.Type, placedObjectWorldPosition);

            foreach (Vector2Int gridPosition in gridPositionList) {
                GridObject g = _grid.GetGridObject(gridPosition.x, gridPosition.y);
                g.SetPlacedObject(factoryBase);
                gridObjectsList.Add(g);
            }
            factoryBase.OccupiedGrids = gridObjectsList;
            UIManager.Instance.SubscribeHealthChangedMethod(factoryBase);
            placedObject.GetComponent<BaseModel>().SetBuildingModel(factoryBase);
            return placedObject;
        }
        else {
            // Cannot build here
            GridUtils.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
            return null;
        }
    }

    private void RemoveUnitFromGrid(SoldierBase soldier) {
        soldier.OriginGrid.SetPlacedObject(null);
        soldier = null;
    }


}