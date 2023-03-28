using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PathfindingCalculations : IPathfindingHelper {
    
    private int MOVE_STRAIGHT_COST = 10;
    private int MOVE_DIAGONAL_COST = 14;
    private Grid<GridObject> _grid;

    public PathfindingCalculations() {
        _grid = GridCreation.Instance.GetGrid();
    }
    
    public GridObject GetLowestFCostNode(List<GridObject> pathNodeList) {
        GridObject lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

   

    public List<GridObject> CalculatePath(GridObject endNode) {
        List<GridObject> path = new List<GridObject>();
        path.Add(endNode);
        GridObject currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    public int CalculateDistanceCost(GridObject a, GridObject b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public GridObject GetNode(int x, int y) {
        return _grid.GetGridObject(x, y);
    }

    public List<GridObject> GetNeighbourList(GridObject currentNode) {
        List<GridObject> neighbourList = new List<GridObject>();
        for (int i = 0; i < 1; i++) {
            if (currentNode.x - 1 >= 0) {
                //Left
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
                //LeftDown
                if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                //LeftUp
                if (currentNode.y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
            if (currentNode.x + 1 < _grid.GetWidth()) {
                //Right
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
                //Right Down
                if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
                //Right Up
                if (currentNode.y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }
            //Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
            //up
            if (currentNode.y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }
        return neighbourList;
    }

    public int GetWidth() {
        return _grid.GetWidth();
    }

    public int GetHeight() {
        return _grid.GetHeight();
    }

    public List<GridObject> EnvironmentGrids(List<GridObject> occupiedGrids) {
        List<GridObject> calculatedGrids = new List<GridObject>();
        foreach (GridObject grid in occupiedGrids) {
            var tempNeighbourList = GetNeighbourList(grid).Where(g => g.isWalkable);
            foreach (GridObject neighbour in tempNeighbourList) {
                if (!calculatedGrids.Contains(neighbour)) {
                    calculatedGrids.Add(neighbour);
                }
            }
        }
        return calculatedGrids;
    }

    public GridObject CalculateClosestGrid(List<GridObject> occupiedGrids, GridObject currentGrid) {
        List<GridObject> calculatedGrids = new List<GridObject>();
        GridObject lowestGrid = null;
        int lowestCost = int.MaxValue;
        foreach (GridObject gridObject in occupiedGrids) {
            var tempList = GetNeighbourList(gridObject);
            foreach (GridObject grid in tempList) {
                if (!calculatedGrids.Contains(grid)) {
                    calculatedGrids.Add(grid);
                }
            }
            foreach (GridObject grid in calculatedGrids.Where(g => g.isWalkable)) {
                int calculatedValue = CalculateDistanceCost(grid, currentGrid);
                if (calculatedValue < lowestCost) {
                    lowestGrid = grid;
                    lowestCost = calculatedValue;
                }
            }
        }
        return lowestGrid;
    }
}