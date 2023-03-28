using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
public class Pathfinding : MonoBehaviour {
    
    private List<GridObject> _closedList;
    private List<GridObject> _openList;
    public static Pathfinding Instance;
    private IPathfindingHelper _pathfindingHelper;
    private int _roadBlockedSafeClutch;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        _pathfindingHelper = new PathfindingCalculations();
    }

    public void StartFindPath(int startX, int startY, int endX, int endY) {
        StartCoroutine(FindPath(startX, startY, endX, endY));
    }

    public IEnumerator FindPath(int startX, int startY, int endX, int endY) {
        List<GridObject> path = new List<GridObject>();
        bool pathSuccess = false;

        GridObject startNode = _pathfindingHelper.GetNode(startX, startY);
        GridObject endNode = _pathfindingHelper.GetNode(endX, endY);

        _openList = new List<GridObject> { startNode };
        _closedList = new List<GridObject>();

        for (int x = 0; x < _pathfindingHelper.GetWidth(); x++) {
            for (int y = 0; y < _pathfindingHelper.GetHeight(); y++) {
                GridObject gridObject = _pathfindingHelper.GetNode(x, y);
                gridObject.gCost = int.MaxValue;
                gridObject.CalculateFCost();
                gridObject.cameFromNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = _pathfindingHelper.CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        while (_openList.Count > 0) {
            GridObject currentNode = _pathfindingHelper.GetLowestFCostNode(_openList);
            if (currentNode == endNode) {
                pathSuccess = true;
                break;
            }
            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (GridObject neighbourNode in _pathfindingHelper.GetNeighbourList(currentNode)) {
                if (_closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable) {
                    _closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + _pathfindingHelper.CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = _pathfindingHelper.CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                    if (!_openList.Contains(neighbourNode)) {
                        _openList.Add(neighbourNode);
                    }
                }
            }

        }
        yield return null;
        if (pathSuccess) {
            path = _pathfindingHelper.CalculatePath(endNode);
            PathRequestManager.Instance.FinishedProcessingPath(path, true);
        }
        else {
            _roadBlockedSafeClutch++;
            var endNeighbourList = _pathfindingHelper.GetNeighbourList(endNode).Where(n => n.isWalkable);
            var startNeighbourList = _pathfindingHelper.GetNeighbourList(startNode).Where(n => !n.isWalkable);
            if (!endNeighbourList.Any() || startNeighbourList.Count() == 8 ||_roadBlockedSafeClutch > 15) {
                _roadBlockedSafeClutch = 0;
                PathRequestManager.Instance.FinishedProcessingPath(path, false);
                yield break;
            }
            GridObject grid = endNeighbourList.First();
            StartFindPath(startX, startY, grid.x, grid.y);
        }
    }
}