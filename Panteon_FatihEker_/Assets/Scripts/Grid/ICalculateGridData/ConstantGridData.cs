using UnityEngine;
class ConstantGridData : MonoBehaviour, ICalculateGridData {

    private const int MainPixelWidth = 1920;
    private const int MainPixelHeight = 1080;

    public Vector2Int CalculateData()  {
        int desiredPixel = 32;
        float gridWidth = (MainPixelWidth / desiredPixel) * .5f; // 0.25 left 0.25 gap total 1920/25 px * 0.5
        float gridHeight = MainPixelHeight / desiredPixel;
        return new Vector2Int((int)gridHeight, (int)gridHeight);
    }

    public Vector3 GetPosition() {
        return Camera.main.ScreenToWorldPoint(new Vector3(MainPixelWidth * .25f, 0, 10));
    }



}