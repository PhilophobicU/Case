using UnityEngine;
using UnityEngine.EventSystems;
public class MouseInputHandler : IMouse {
    public BaseModel SelectedObject { get; private set; }
    private readonly FactoryProductGenerator _factoryProductGenerator;
    private Collider2D _collider2D;
    public MouseInputHandler(FactoryProductGenerator productGenerator) {
        _factoryProductGenerator = productGenerator;
    }
    public void Select(Collider2D collider2D) {
        if (collider2D != null) {
            bool clickedDifferentBuilding = SelectedObject != null && collider2D.GetComponent<BaseModel>() != SelectedObject;
            if (clickedDifferentBuilding) FactoryProductGenerator.Instance.FactoryDeselect();
            SelectedObject = collider2D.GetComponent<BaseModel>();
            if (SelectedObject != null) _factoryProductGenerator.SelectModel(SelectedObject.GetFactoryBase());
        }
        else {
            FactoryProductGenerator.Instance.FactoryDeselect();
            SelectedObject = null;
        }
    }
    public void Tick() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = GridUtils.GetMouseWorldPosition();
            _collider2D = Physics2D.OverlapArea(mousePos, mousePos);
        }
        if (_factoryProductGenerator.factoryCreated && Input.GetMouseButtonUp(0) && !IsMouseOverUI()) {
            SelectedObject = _factoryProductGenerator.PlaceBuilding();
        }
        else if (Input.GetMouseButtonUp(0)) {
            if (IsMouseOverUI()) return;
            Select(_collider2D);
        }
        if (Input.GetMouseButtonDown(1) && SelectedObject != null) {
            if(SelectedObject.TryGetComponent(out IHasRightClick clickable))
                clickable.MarkDestination(true);
            _factoryProductGenerator.SelectModel(SelectedObject.GetFactoryBase());
        }
    }
    private bool IsMouseOverUI() => EventSystem.current.IsPointerOverGameObject();
}