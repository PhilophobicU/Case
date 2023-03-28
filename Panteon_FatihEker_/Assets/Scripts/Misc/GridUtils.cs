using TMPro;
using UnityEngine;

public static class GridUtils
{
    public static TextMeshPro CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = default) {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    
    public static GameObject NewSprite(GridObject selectedGrid,Sprite sprite,Transform transform) {

        var _selectedVisual = new GameObject("Selected", typeof(SpriteRenderer));
        _selectedVisual.transform.parent = transform;
        var a = _selectedVisual.GetComponent<SpriteRenderer>();
        a.sortingOrder = 2;
        a.sprite = sprite;
        _selectedVisual.transform.localScale = Vector3.one * 4; 
        _selectedVisual.transform.position = GridCreation.Instance.GetGrid().GetWorldPosition(selectedGrid.GetPlacedObject().OriginGrid.x, selectedGrid.GetPlacedObject().OriginGrid.y)+Vector3.one /2; // set position for flag

        return _selectedVisual;
    }
    
    // Create Text in the World
    public static TextMeshPro CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMeshPro));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
        //textMesh.rectTransform.anchoredPosition = TextAnchor.MiddleCenter;
        textMesh.alignment = (TextAlignmentOptions)textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    
    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    
    public static void CreateWorldTextPopup(string text, Vector3 localPosition) {
        CreateWorldTextPopup(null, text, localPosition, 10, Color.white, localPosition + new Vector3(0, 10), 1f);
    }

    // Create a Text Popup in the World
    public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime) {
        TextMeshPro textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, 10);
        Transform transform = textMesh.transform;
        Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
        FunctionUpdater.Create(delegate () {
            transform.position += moveAmount * Time.deltaTime;
            popupTime -= Time.deltaTime;
            if (popupTime <= 0f) {
                UnityEngine.Object.Destroy(transform.gameObject);
                return true;
            } else {
                return false;
            }
        }, "WorldTextPopup");
    }
}
