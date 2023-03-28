using UnityEngine;
public class BarrackModel : BaseModel,IHasRightClick,IFlag {

    private BuildingBase _barrackData;
    private GameObject _flagVisual;
    private GridObject _flagGrid;

    private void Start() {
        GameController.Instance.factoryProductGenerator.FactorySelected += ShowFlag;
        GameController.Instance.factoryProductGenerator.FactoryDeselected += HideFlag;
    }
    public void ShowFlag(FactoryBase factoryBase) {
        if (_barrackData != factoryBase || _flagVisual == null) return; // Cancel this if you want to see all flags at the same time
        if (_flagVisual != null) _flagVisual.SetActive(true);
    }
    public void HideFlag() {
        if(_flagVisual == null) return;
        if (_flagVisual != null) _flagVisual.SetActive(false);
    }

    public void UnSub() {
        GameController.Instance.factoryProductGenerator.FactorySelected -= ShowFlag;
        GameController.Instance.factoryProductGenerator.FactoryDeselected -= HideFlag;
    }

    public bool IsFlagPlaced() {
        return _flagGrid != null;
    }
    public void MarkDestination(bool isUserInput) {
        Vector3 mousePosition = GridUtils.GetMouseWorldPosition();
        _flagGrid = GridCreation.Instance.GetGrid().GetGridObjectWithPos(mousePosition);
        if (!_flagGrid.CanBuild()) return;
        if (!IsFlagPlaced() || IsFlagPlaced() && _barrackData.FlagGrid != _flagGrid) _barrackData.FlagGrid = _flagGrid; // Reload flag grid
        if (_flagVisual == null) _flagVisual = (GameObject)Instantiate(Resources.Load("FlagObject"),transform); // create visual if its not already created
        if (_flagVisual != null && _flagGrid.isWalkable) _flagVisual.transform.position = GridCreation.Instance.GetGrid().GetGridWorldPosFromMousePos() + Vector3.one /2; // set position for flag
    }

    public override FactoryBase GetFactoryBase() {
        return _barrackData;
    }
    public override void SetBuildingModel(FactoryBase factoryBase) {
        if (factoryBase.GetType() != typeof(BuildingBase)) Debug.LogError("There is a issue with SetBuildingType");
        else {
            _barrackData = (BuildingBase)factoryBase;
        }
    }
}