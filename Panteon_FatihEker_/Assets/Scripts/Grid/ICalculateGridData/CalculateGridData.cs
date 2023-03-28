

using UnityEngine;
using UnityEngine.Timeline;
class CalculateGridData :MonoBehaviour , ICalculateGridData {
    private int _mainPixelWidth;
    private int _mainPixelHeight;
    public Vector2Int CalculateData() {
        float desiredPixel = 32;
        _mainPixelWidth = Camera.main.pixelWidth;
        _mainPixelHeight = Camera.main.pixelHeight;
        float gridWidth = Mathf.CeilToInt(_mainPixelWidth / desiredPixel) * .5f; // 0.25 left 0.25 gap total 1920/25 px * 0.5
        float gridHeight = Mathf.CeilToInt(_mainPixelHeight / desiredPixel);
        return new Vector2Int((int)gridWidth, (int)gridHeight);
    }

    public Vector3 GetPosition() {
        return Camera.main.ScreenToWorldPoint(new Vector3(_mainPixelWidth * .25f, 0, 10));;
    }



}