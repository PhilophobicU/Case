using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoldierModel : BaseModel, IHasRightClick {
    [SerializeField] private SpriteRenderer characterVisual;
    private SoldierBase _factoryBase;
    private List<GridObject> _path;
    private bool _isMoving;
    private AttackStates _state;
    private float _countDown;
    private FactoryBase _attackedObject;
    private float _timeBetweenAttacks = 2f;
    private IPathfindingHelper _pathfindingHelper;
    private GridObject _tempDest;

    public override void SetBuildingModel(FactoryBase factoryBase) {
        _factoryBase = (SoldierBase)factoryBase;
        _pathfindingHelper = new PathfindingCalculations();
        characterVisual.sprite = _factoryBase.Visual;
    }

    public override FactoryBase GetFactoryBase() {
        return _factoryBase;
    }

    public void MarkDestination(bool isUserInput) {
        if (_isMoving) return;
        GridObject temp = _factoryBase.OriginGrid;
        GridObject destination = isUserInput ? GridCreation.Instance.GetGrid().GetGridObjectWithPos(GridUtils.GetMouseWorldPosition()) : _factoryBase.Parent.FlagGrid;
        if (destination.isWalkable) {
            UpdateState(AttackStates.notAttacking);
            PathRequestManager.RequestPath(temp.x, temp.y, destination.x, destination.y, FollowPath);
        }
        else {
            GridObject grid = _pathfindingHelper.CalculateClosestGrid(destination.GetPlacedObject().ReturnGrids(), _factoryBase.OriginGrid);
            if (grid == null) {
                FollowPath(null, false);
                return;
            }
            if (_attackedObject != destination.GetPlacedObject()) {
                PathRequestManager.RequestPath(temp.x, temp.y, grid.x, grid.y, FollowPath);
                if (!isUserInput) return;
                _attackedObject = destination.GetPlacedObject();
                _tempDest = destination.GetPlacedObject().OriginGrid;
                UpdateState(AttackStates.walkingToTarget);
            }
        }
    }

    private void UpdateGridValues(GridObject destination) {
        bool dontClearBarrack = _factoryBase.OriginGrid != _factoryBase.Parent.OriginGrid;
        if (dontClearBarrack) _factoryBase.OriginGrid.SetPlacedObject(null);
        _factoryBase.OriginGrid = destination;
        destination.SetPlacedObject(_factoryBase);
    }

    private void Update() {
        if (_attackedObject == null) return;
        if (_state == AttackStates.inRange) {
            _countDown -= Time.deltaTime;
            if (_countDown <= 0) {
                if (_attackedObject.Health <= 0 || _tempDest != _attackedObject.OriginGrid) {
                    _countDown = 0;
                    UpdateState(AttackStates.notAttacking);
                    _attackedObject = null;
                    _tempDest = null;
                    return;
                }
                _attackedObject.HitDamage(_factoryBase.Damage);
                GridUtils.CreateWorldTextPopup(_factoryBase.Damage + " Damage", transform.position);
                _countDown += _timeBetweenAttacks;
            }
        }
    }

    public void FollowPath(List<GridObject> path, bool pathSuccessful) {

        if (pathSuccessful) {
            _path = path;
            StartCoroutine(FollowRoad());
            UpdateGridValues(_path[^1]);
        }
        else {
            StopCoroutine(UIManager.Instance.SoldierBlocked());
            StartCoroutine(UIManager.Instance.SoldierBlocked());
        }
    }

    private IEnumerator FollowRoad() {
        _isMoving = true;
        Vector3 currentWaypointGrid = GridCreation.Instance.GetGrid().GetWorldPosition(_path[0].x, _path[0].y);
        int targetIndex = 0;
        while (true) {
            if (transform.position == currentWaypointGrid) {
                targetIndex++;
                if (targetIndex >= _path.Count) {
                    _isMoving = false;
                    _state = AttackStates.inRange;
                    yield break;
                }
                currentWaypointGrid = GridCreation.Instance.GetGrid().GetWorldPosition(_path[targetIndex].x, _path[targetIndex].y);
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypointGrid, 10 * Time.deltaTime);
            yield return null;
        }
    }

    private void UpdateState(AttackStates newState) {
        _state = newState;
        switch (_state) {

            case AttackStates.notAttacking:
                _attackedObject = null;
                break;
            case AttackStates.walkingToTarget: break;
            case AttackStates.inRange: break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    public SoldierBase GetSoldierBase() {
        return _factoryBase;
    }
}
public enum AttackStates {
    notAttacking,
    walkingToTarget,
    inRange
}